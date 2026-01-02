using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "UpdateObjectiveText", menuName = "Scriptable Objects/UpdateObjectiveText")]
public class UpdateObjectiveText : EventAction
{
    public string newText;

    public override IEnumerator Execute()
    {
        ObjectiveManager.objectiveManager.UpdateObjectiveText(newText);
        yield return null;
    }
}
