using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "WaitForlookAt", menuName = "Scriptable Objects/WaitForlookAt")]
public class WaitForlookAt : EventAction
{
    public SceneReference lookAtObj;
    public bool disableColider = false;

    public override IEnumerator Execute()
    {
        var obj = lookAtObj.Get<GameObject>();
        Debug.Log("Waiting for player to look at " + obj.name);


        while (InteractWithObject.interactWithObject.currentLookTarget != obj)
        {
            yield return null; // wait one frame
        }

        if (disableColider)
        {
            obj.GetComponent<Collider>().enabled = false;
        }
        // Once they are looking at it, continue
        yield return null;

        Debug.Log("Player looked at " + obj.name);


    }
}
