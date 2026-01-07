using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "WaitForlookAt", menuName = "Scriptable Objects/WaitForlookAt")]
public class WaitForlookAt : EventAction
{
    public SceneReference lookAtObj;
    public SceneReference lookAtObj2;
    public bool disableColider = false;

    public override IEnumerator Execute()
    {
        GameObject obj = lookAtObj.Get<GameObject>();
        GameObject obj2 = null;
        if (lookAtObj2.key != null) 
        {
             obj2 = lookAtObj2.Get<GameObject>();
        }
        
        Debug.Log("Waiting for player to look at " + obj.name + " " + obj2?.name);


        while (InteractWithObject.interactWithObject.currentLookTarget != obj &&  InteractWithObject.interactWithObject.currentLookTarget != obj2)
        {
            yield return null; // wait one frame
        }

        if (disableColider)
        {
            obj.GetComponent<Collider>().enabled = false;
            obj2.GetComponent<Collider>().enabled = false;
        }
        // Once they are looking at it, continue
        yield return null;

        Debug.Log("Player looked at " + obj.name + " " + obj2?.name);


    }
}
