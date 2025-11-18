using System.Collections;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{

    public AudioSource redLightScream;

    public AudioClip nextFloorRedLightsVoiceLine;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        ObjectiveManager.objectiveManager.UpdateObjectiveText("Why are the lights red?? That can't be good...");
        Player.player.PlayVoiceLine(nextFloorRedLightsVoiceLine);
        StartCoroutine(DelayedMonsterScream());
    }

    IEnumerator DelayedMonsterScream()
    {
        yield return new WaitForSeconds(3f);

        redLightScream.Play();
    }
}
