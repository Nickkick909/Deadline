using TMPro;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{

    public GameObject objectiveUI;
    public TMP_Text objectiveText;

    public static ObjectiveManager objectiveManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectiveManager = this;
    }

    public void UpdateObjectiveText(string text)
    {
        objectiveText.text = text;
    }
}
