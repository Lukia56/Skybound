using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("メニュー項目")]
    public TMP_Text[] menuItems;

    [Header("選択時の色")]
    public Color selectedColor = Color.yellow;

    [Header("ポーズ画面")]
    public GameObject pausePanel;

    [Header("Resume")]
    public TMP_Text resumeText;

    private Color[] normalColors;

    private int selectedIndex = 0;

    private bool isPaused = false;


    // Start
    void Start()
    {
        // メニュー項目がない場合
        if (menuItems == null || menuItems.Length == 0)
        {
            Debug.LogWarning("menuItemsが設定されていません。");
            return;
        }

        // 元の色を保存
        normalColors = new Color[menuItems.Length];

        for (int i = 0; i < menuItems.Length; i++)
        {
            if (menuItems[i] == null)
                continue;

            normalColors[i] = menuItems[i].color;

            // TMP_Textをクリック可能にする
            AddClickHandler(menuItems[i], i);
        }


        // ポーズ画面を非表示
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }


        // Resumeをクリック可能にする
        if (resumeText != null)
        {
            AddResumeClickHandler();
        }


        // 最初の項目を選択
        SelectItem(0);
    }


    // Update
    void Update()
    {
        if (Keyboard.current == null)
            return;


        // =========================
        // Escape
        // =========================

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // ポーズ中でなければポーズ
            if (!isPaused)
            {
                PauseGame();
            }
        }


        // =========================
        // ポーズ中の操作
        // =========================

        if (isPaused)
        {
            // ↑
            if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                selectedIndex--;

                if (selectedIndex < 0)
                {
                    selectedIndex = menuItems.Length - 1;
                }

                SelectItem(selectedIndex);
            }


            // ↓
            if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                selectedIndex++;

                if (selectedIndex >= menuItems.Length)
                {
                    selectedIndex = 0;
                }

                SelectItem(selectedIndex);
            }


            // Enter
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                ExecuteItem(selectedIndex);
            }
        }
    }


    // =========================
    // メニュー項目を選択
    // =========================

    public void SelectItem(int index)
    {
        if (menuItems == null || menuItems.Length == 0)
            return;

        if (index < 0 || index >= menuItems.Length)
            return;


        selectedIndex = index;


        for (int i = 0; i < menuItems.Length; i++)
        {
            if (menuItems[i] == null)
                continue;


            if (i == selectedIndex)
            {
                // 選択中
                menuItems[i].color = selectedColor;
            }
            else
            {
                // 選択されていない
                menuItems[i].color = normalColors[i];
            }
        }
    }


    // =========================
    // TMP_Textをクリック可能にする
    // =========================

    void AddClickHandler(TMP_Text text, int index)
    {
        EventTrigger trigger =
            text.gameObject.GetComponent<EventTrigger>();


        if (trigger == null)
        {
            trigger = text.gameObject.AddComponent<EventTrigger>();
        }


        // -------------------------
        // マウスカーソルが乗った
        // -------------------------

        EventTrigger.Entry enterEntry =
            new EventTrigger.Entry();

        enterEntry.eventID =
            EventTriggerType.PointerEnter;


        enterEntry.callback.AddListener((data) =>
        {
            // マウスを乗せた項目を選択
            SelectItem(index);
        });


        trigger.triggers.Add(enterEntry);


        // -------------------------
        // クリック
        // -------------------------

        EventTrigger.Entry clickEntry =
            new EventTrigger.Entry();

        clickEntry.eventID =
            EventTriggerType.PointerClick;


        clickEntry.callback.AddListener((data) =>
        {
            // クリックした項目を選択
            SelectItem(index);

            // その項目を実行
            ExecuteItem(index);
        });


        trigger.triggers.Add(clickEntry);
    }


    // =========================
    // メニュー項目を実行
    // =========================

    void ExecuteItem(int index)
    {
        if (index < 0 || index >= menuItems.Length)
            return;

        if (menuItems[index] == null)
            return;

        Debug.Log("選択: " + menuItems[index].text);

        // Resumeだった場合
        if (menuItems[index] == resumeText)
        {
            ResumeGame();
            return;
        }
    }


    // =========================
    // ポーズ開始
    // =========================

    void PauseGame()
    {
        isPaused = true;


        // ポーズ画面を表示
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }


        // ゲームを停止
        Time.timeScale = 0f;


        // 最初のメニュー項目を選択
        SelectItem(0);
    }


    // =========================
    // Resumeクリック設定
    // =========================

    void AddResumeClickHandler()
    {
        EventTrigger trigger =
            resumeText.gameObject.GetComponent<EventTrigger>();


        if (trigger == null)
        {
            trigger = resumeText.gameObject.AddComponent<EventTrigger>();
        }


        EventTrigger.Entry clickEntry =
            new EventTrigger.Entry();

        clickEntry.eventID =
            EventTriggerType.PointerClick;


        clickEntry.callback.AddListener((data) =>
        {
            ResumeGame();
        });


        trigger.triggers.Add(clickEntry);


        // マウスを乗せたらResumeを選択
        EventTrigger.Entry enterEntry =
            new EventTrigger.Entry();

        enterEntry.eventID =
            EventTriggerType.PointerEnter;


        enterEntry.callback.AddListener((data) =>
        {
            resumeText.color = selectedColor;
        });


        trigger.triggers.Add(enterEntry);
    }


    // =========================
    // ポーズ解除
    // =========================

    void ResumeGame()
    {
        isPaused = false;


        // ポーズ画面を非表示
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }


        // ゲーム再開
        Time.timeScale = 1f;
    }
}