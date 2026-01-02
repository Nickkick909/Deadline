using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "WaitForBool", menuName = "Scriptable Objects/WaitForBool")]
public class WaitForBool : EventAction
{
    public SceneReference objRef;
    public string componentName;
    public string fieldName;
    public bool targetValue;

    public override IEnumerator Execute()
    {
        var obj = objRef.Get<GameObject>();
        var component = obj.GetComponent(componentName);
        var field = component.GetType().GetField(fieldName);

        if (field == null)
        {
            Debug.LogError($"Field {fieldName} not found on {component.name}");
            yield break;
        }

        yield return new WaitUntil(() =>
            (bool)field.GetValue(component) == targetValue
        );

        Debug.Log("Next step!");
        Debug.Log("Done waiting for " + fieldName + " to equal " + targetValue);
    }
}
