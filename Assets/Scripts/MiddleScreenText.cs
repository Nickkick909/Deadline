using TMPro;
using UnityEngine;

public class MiddleScreenText : MonoBehaviour
{

    public delegate void UpdateMiddleScreenText(string newText);
    public static UpdateMiddleScreenText updateMiddleScreenText;

    [SerializeField] private TMP_Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.text = "";
    }

    private void OnEnable()
    {
        updateMiddleScreenText += SetText;
    }

    private void OnDisable()
    {
        updateMiddleScreenText -= SetText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetText(string newText)
    {
        text.text = newText;
    }
}
