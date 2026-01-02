using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "EnableCutsceneAndWait", menuName = "Scriptable Objects/EnableCutsceneAndWait")]
public class EnableCutsceneAndWait : EventAction
{

    public SceneReference timeline;

    public override IEnumerator Execute()
    {
        var obj = timeline.Get<GameObject>();

        obj.SetActive(true);

        CutsceneComplete cutsceneComplete = obj.GetComponent<CutsceneComplete>();
        while (!cutsceneComplete.cutsceneComplete)
        {
            yield return null; // wait one frame
        }

        obj.SetActive(false);

        yield return null;
    }
}
