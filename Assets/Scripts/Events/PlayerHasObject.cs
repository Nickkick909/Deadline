using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHasObject", menuName = "Scriptable Objects/PlayerHasObject")]
public class PlayerHasObject : EventAction
{
    public SceneReference objectToHave;
    

    public override IEnumerator Execute()
    {
        var obj = objectToHave.Get<GameObject>();
        Debug.Log("Waiting for player to get item " + obj.name);


        while (!Player.player.CheckIfPlayerHasItem(obj))
        {
            yield return null; // wait one frame
        }


        // Once they are looking at it, continue
        yield return null;

        Debug.Log("Player has item" + obj.name);


    }
}
