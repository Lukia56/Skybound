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

    private InputAction upInput;
    private InputAction downInput;
    private InputAction submitInput;
    private InputAction pauseInput;

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

        upInput = InputSystem.actions.FindAction("Up");
        downInput = InputSystem.actions.FindAction("Down");
        submitInput = InputSystem.actions.FindAction("Submit");
        pauseInput = InputSystem.actions.FindAction("Pause");
    }


    // Update
    void Update()
    {
        if (Keyboard.current == null)
            return;


        // =========================
        // Escape
        // =========================

        //if (Keyboard.current.escapeKey.wasPressedThisFrame)
        if (pauseInput.WasPressedThisFrame())
        {
            // ポーズ中でなければポーズ
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }


        // =========================
        // ポーズ中の操作
        // =========================

        if (isPaused)
        {
            // ↑
            //if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            if (upInput.WasPressedThisFrame())
            {
                selectedIndex--;

                if (selectedIndex < 0)
                {
                    selectedIndex = menuItems.Length - 1;
                }

                SelectItem(selectedIndex);
            }


            // ↓
            //if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            if (downInput.WasPressedThisFrame())
            {
                selectedIndex++;

                if (selectedIndex >= menuItems.Length)
                {
                    selectedIndex = 0;
                }

                SelectItem(selectedIndex);
            }


            // Enter
            //if (Keyboard.current.enterKey.wasPressedThisFrame)
            if (submitInput.WasPressedThisFrame())
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


        // 別途作成した関数を実行====
        Execute(menuItems[index]);
        // ==========================


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

    // ==========================================
    // 以下はゲーム組み込みのため緊急で髙木が実装
    // ==========================================

    /// <summary>
    /// リスタートテキスト
    /// </summary>
    [SerializeField] TMP_Text retryText=null;
    /// <summary>
    /// タイトル遷移テキスト
    /// </summary>
    [SerializeField] TMP_Text toTitleText=null;
    /// <summary>
    /// 項目実行処理
    /// チームメンバー担当部分の明確化のため別で記述
    /// </summary>
    /// <param name="text"></param>
    private void Execute(TMP_Text text)
    {
        // ポーズ解除
        ResumeGame();

        if (text == retryText)
        {
            StageRetry();
        }
        else if (text == toTitleText)
        {
            ToTitle();
        }
    }
    /// <summary>
    /// タイトルシーンに遷移
    /// </summary>
    private void ToTitle()
    {
        StageSceneManager.instance.LoadTitleScene();
    }
    /// <summary>
    /// ステージやり直し
    /// </summary>
    private void StageRetry()
    {
        StageSceneManager.instance.LoadCurrentStage();
    }

}