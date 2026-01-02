using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "WaitEvent", menuName = "Scriptable Objects/WaitEvent")]

public class WaitEvent : EventAction
{
    public float waitTime;
    public override IEnumerator Execute()
    {
        Debug.Log("Waiting for " + waitTime + " seconds");
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Wait completed");
    }
}
