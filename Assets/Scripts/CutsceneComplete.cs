using UnityEngine;

public class CutsceneComplete : MonoBehaviour
{

    public bool returnCamToPlayer = false;
    public bool cutsceneComplete = false;

    public void CutsceneEnded()
    {
        Debug.Log("Cutscene over");
        cutsceneComplete = true;

        if (returnCamToPlayer )
        {
            Camera.main.transform.localPosition = Vector3.zero;
        }

        
    }
}
