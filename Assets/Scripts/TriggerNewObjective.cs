using UnityEngine;

public class TriggerNewObjective : MonoBehaviour
{
    public string newObjectiveText;

    private void OnTriggerEnter(Collider other)
    {
        ObjectiveManager.objectiveManager.UpdateObjectiveText(newObjectiveText);
        gameObject.SetActive(false);

    }
}
