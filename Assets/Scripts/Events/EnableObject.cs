using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "EnableObject", menuName = "Scriptable Objects/EnableObject")]
public class EnableObject : EventAction
{
    public SceneReference objRef;
    public bool enable = true;

    public override IEnumerator Execute()
    {
        var obj = objRef.Get<GameObject>();
        obj.SetActive(enable);
        yield return null;
    }
}
