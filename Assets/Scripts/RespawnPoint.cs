using System.Collections;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{

    public AudioSource redLightScream;

    public AudioClip nextFloorRedLightsVoiceLine;

    public StoryEvent startFourthFloor;


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
        gameObject.GetComponent<BoxCollider>().enabled = false;
        startFourthFloor.Play();
        //ObjectiveManager.objectiveManager.UpdateObjectiveText("Why are the lights red?? That can't be good...");
        //Player.player.PlayVoiceLine(nextFloorRedLightsVoiceLine);
        //StartCoroutine(DelayedMonsterScream());
    }

    IEnumerator DelayedMonsterScream()
    {
        yield return new WaitForSeconds(3f);

        redLightScream.Play();
    }
}
