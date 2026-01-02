using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayVoiceLineEvent", menuName = "Scriptable Objects/PlayVoiceLineEvent")]
public class PlayVoiceLineEvent : EventAction
{
    public AudioClip clip;
    public bool waitFullClip = false;

    public override IEnumerator Execute()
    {
        Player.player.PlayVoiceLine(clip);
        if (waitFullClip )
        {
            yield return new WaitForSeconds(clip.length);
        }
        else
        {
            yield return null;
        }
    }
}
