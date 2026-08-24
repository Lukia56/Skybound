using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    TMP_Text TMP_Text;
    Color color;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TMP_Text = GetComponent<TMP_Text>();
        if (TMP_Text != null)
        {
            color = TMP_Text.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter()
    {
        if (TMP_Text != null)
        {
            TMP_Text.color = Color.yellow;
        }
    }

    public void OnPointerExit()
    {
        if (TMP_Text != null)
        {
            TMP_Text.color = color;
        }
    }
}
