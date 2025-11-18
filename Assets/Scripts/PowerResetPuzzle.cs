using System.Collections;
using TMPro;
using UnityEngine;

public class PowerResetPuzzle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public BreakerPuzzle breakerPuzzle;
    [SerializeField] string typedCode;
    [SerializeField] string codeAnswer;

    [SerializeField] TMP_Text typedOutput;

    [SerializeField] BoxCollider[] keypadButtons;

    bool keypadInited = false;

    void Start()
    {
        for (int i = 0; i < keypadButtons.Length; i++)
        {
            keypadButtons[i].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!keypadInited && breakerPuzzle.completed == true)
        {
            for (int i = 0; i < keypadButtons.Length; i++)
            {
                keypadButtons[i].enabled = true;
            }

            keypadInited = true;
        }
    }

    public void KeyPressed(string code)
    {
        // Play key pressed noise


        typedCode += code;
        typedOutput.text = typedCode;

        if (typedCode == codeAnswer)
        {
            // Correct
        }

        if (typedCode.Length >= codeAnswer.Length)
        {
            StartCoroutine(WrongCodeEntered());

            // Play wrong code noise

            // Flash the code and then make it clear

        }
    }

    IEnumerator WrongCodeEntered()
    {
        yield return new WaitForSeconds(2f);

        typedCode = "";
        typedOutput.text = typedCode;
    }
}
