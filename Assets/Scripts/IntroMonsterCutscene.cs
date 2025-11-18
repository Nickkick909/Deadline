using System.Collections;
using UnityEngine;

public class IntroMonsterCutscene : MonoBehaviour
{
    public Transform playerCamera;
    public Transform monsterTarget;
    public Transform monsterTarget2;
    public Transform monsterTarget3;
    public Transform monsterTarget4;
    public Transform monster;
    public float panDuration = 3f;
    public float returnDuration = 3f;
    private bool isRunning;

    public InteractWithObject interactWithObject;

    public GameObject[] introLights;

    public Animator singleLight;

    public PauseMenuManager pauseMenuManager;

    public AudioSource monsterAudio;
    public AudioSource monsterBreathingAudio;
    public AudioClip monsterBang;
    public AudioClip monsterScream;

    public GameObject doorToClose;
    public GameObject doorToClose2;

    private void Start()
    {
        monsterBreathingAudio.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayCutscene();
    }

    public void PlayCutscene()
    {
        pauseMenuManager.canPause = false;
        StartCoroutine(CutsceneRoutine());
    }

    IEnumerator CutsceneRoutine()
    {

        for (int i = 0; i < introLights.Length; i++)
        {
            introLights[i].SetActive(true);
        }

        monsterBreathingAudio.enabled = true;
        
        Player.player.blockMovement = true;

        Player.player.footstepsSFX.enabled = false;
        interactWithObject.enabled = false;
        //OffsetFlashlight.playerFlashlight.TurnOffFlashlight();
        isRunning = true;

        monsterAudio.PlayOneShot(monsterBang);
        yield return new WaitForSeconds(0.5f);

        Vector3 startLocalPos = playerCamera.localPosition;
        Quaternion startLocalRot = playerCamera.localRotation;

        Vector3 direction = (monster.position - transform.position);
        direction.y = 0f; // Keep player upright
        Quaternion targetRotation = Quaternion.LookRotation(direction);


       
        monster.GetComponent<Animator>().SetInteger("IdleIndex", 2);
        // Whip the player towards the sound.
        float t = 0;
        while (t < 0.25f)
        {
            t += Time.deltaTime;
            //playerCamera.localPosition = Vector3.Lerp(startLocalPos, startLocalPos + new Vector3(0, 1.5f, 0), t / 0.25f);
            Player.player.transform.rotation = Quaternion.Slerp(Player.player.transform.rotation, targetRotation, t / 0.25f);
            yield return null;
        }

        OffsetFlashlight.playerFlashlight.dontFollowCam = true;
        Vector3 targetLocalPos = playerCamera.parent.InverseTransformPoint(monsterTarget.position);
        Vector3 targetLocalPos2 = playerCamera.parent.InverseTransformPoint(monsterTarget2.position);
        Vector3 targetLocalPos3 = playerCamera.parent.InverseTransformPoint(monsterTarget3.position);
        Vector3 targetLocalPos4 = playerCamera.parent.InverseTransformPoint(monsterTarget4.position);

        Quaternion targetLocalRotMonster = Quaternion.LookRotation(
            playerCamera.parent.InverseTransformPoint(monster.position));
        Quaternion targetLocalRot = Quaternion.LookRotation(
            playerCamera.parent.InverseTransformPoint(monster.position) - targetLocalPos
        );
        Quaternion targetLocalRot2 = Quaternion.LookRotation(
            playerCamera.parent.InverseTransformPoint(monster.position) - targetLocalPos2
        );
        Quaternion targetLocalRot3 = Quaternion.LookRotation(
            playerCamera.parent.InverseTransformPoint(monster.position) - targetLocalPos3
        );
        Quaternion targetLocalRot4 = Quaternion.LookRotation(
            playerCamera.parent.InverseTransformPoint(monster.position) - targetLocalPos4
        );

        Vector3 nextLocalPos = playerCamera.localPosition;
        Quaternion nextLocalRot = playerCamera.localRotation;

        yield return new WaitForSeconds(0.5f);

        // Go up first
        t = 0;
        while (t < 0.25f)
        {
            t += Time.deltaTime;
            playerCamera.localPosition = Vector3.Lerp(startLocalPos, startLocalPos + new Vector3(0, 1.5f, 0), t / 0.25f);
            //playerCamera.rotation = Quaternion.Slerp(startLocalRot, targetLocalRot, t / 0.25f);
            yield return null;
        }

        singleLight.SetTrigger("IntroMonster");

        // --- PAN TO MONSTER ---
        t = 0f;
        while (t < panDuration)
        {
            t += Time.deltaTime;
            float progress = t / panDuration;
            playerCamera.localPosition = Vector3.Lerp(startLocalPos + new Vector3(0, 1.5f, 0), targetLocalPos, progress);
            playerCamera.localRotation = Quaternion.Slerp(nextLocalRot, targetLocalRot, progress);
            yield return null;
        }

        yield return new WaitForSeconds(2);

        nextLocalPos = playerCamera.localPosition;
        nextLocalRot = playerCamera.localRotation;

        t = 0f;
        while (t < panDuration)
        {
            t += Time.deltaTime;
            float progress = t / panDuration;
            playerCamera.localPosition = Vector3.Lerp(nextLocalPos, targetLocalPos2, progress);
            playerCamera.localRotation = Quaternion.Slerp(nextLocalRot, targetLocalRot2, progress);
            yield return null;
        }

        // Close doors behind player

        doorToClose.GetComponent<Door>().ActionDoor();
        doorToClose.GetComponent<InteractObject>().requiresObject = true;
        doorToClose2.GetComponent<InteractObject>().requiresObject = true;

        nextLocalPos = playerCamera.localPosition;
        nextLocalRot = playerCamera.localRotation;

        t = 0f;
        while (t < panDuration)
        {
            t += Time.deltaTime;
            float progress = t / panDuration;
            playerCamera.localPosition = Vector3.Lerp(nextLocalPos, targetLocalPos3, progress);
            playerCamera.localRotation = Quaternion.Slerp(nextLocalRot, targetLocalRot3, progress);
            yield return null;
        }

        nextLocalPos = playerCamera.localPosition;
        nextLocalRot = playerCamera.localRotation;

        t = 0f;
        while (t < panDuration)
        {
            t += Time.deltaTime;
            float progress = t / panDuration;
            playerCamera.localPosition = Vector3.Lerp(nextLocalPos, targetLocalPos4, progress);
            playerCamera.localRotation = Quaternion.Slerp(nextLocalRot, targetLocalRot4, progress);
            yield return null;
        }

        nextLocalPos = playerCamera.localPosition;
        nextLocalRot = playerCamera.localRotation;

        t = 0f;
        while (t < panDuration)
        {
            t += Time.deltaTime;
            float progress = t / panDuration;
            playerCamera.localPosition = Vector3.Lerp(nextLocalPos, targetLocalPos, progress);
            playerCamera.localRotation = Quaternion.Slerp(nextLocalRot, targetLocalRot, progress);
            yield return null;
        }


        // --- MONSTER ACTIONS ---
        monster.GetComponent<Animator>().SetInteger("IdleIndex", 1);
        monster.GetComponent<Animator>().SetInteger("IdleIndex", 0);
        //yield return StartCoroutine(WaitForAnimation(monster.GetComponent<Animator>(), "idle1"));

        //Quaternion monsterStartRotation = monster.rotation;

        //Vector3 directionMonster = (Player.player.transform.position - monster.position);
        //direction.y = 0f; // Keep player upright
        //targetRotation = Quaternion.LookRotation(directionMonster);
        //t = 0f;
        //while (t < panDuration)
        //{
        //    t += Time.deltaTime;
        //    float progress = t / panDuration;
        //    monster.rotation = Quaternion.Slerp(monsterStartRotation, targetRotation, progress);
        //    yield return null;
        //}

        

        monster.GetComponent<Animator>().SetTrigger("RageTrigger");
        
        yield return StartCoroutine(WaitForAnimation(monster.GetComponent<Animator>(), "rage"));

        t = 0f;
        while (t < 0.25f)
        {
            t += Time.deltaTime;
            monster.localEulerAngles = Vector3.Lerp(new Vector3(0, 240, 0), new Vector3(0, 360, 0), t / 0.25f);
            yield return null;
        }

        monster.GetComponent<Animator>().SetInteger("RunIndex", 2);






        float runSpeed = 6f;         // units per second
        float runTime = 2f;          // how long it should move before disappearing or stopping
        float elapsed = 0f;

        // While the animation plays, move the monster forward
        while (elapsed < runTime)
        {
            elapsed += Time.deltaTime;
            monster.Translate(Vector3.forward * runSpeed * Time.deltaTime);
            yield return null;
        }

        // Optionally disable or hide monster after run
        monster.gameObject.SetActive(false);




        // --- RETURN TO PLAYER ---
        t = 0f;
        while (t < returnDuration)
        {
            t += Time.deltaTime;
            float progress = t / returnDuration;
            playerCamera.localPosition = Vector3.Lerp(targetLocalPos, startLocalPos, progress);
            playerCamera.localRotation = Quaternion.Slerp(targetLocalRot, startLocalRot, progress);
            yield return null;
        }

        // restore exact starting pose
        playerCamera.localPosition = startLocalPos;
        playerCamera.localRotation = startLocalRot;


        Player.player.blockMovement = false;
        //if (flashWasOn)
        //{
        //    OffsetFlashlight.playerFlashlight.TurnOnFlashlight();

        //}
        OffsetFlashlight.playerFlashlight.dontFollowCam = false;
        interactWithObject.enabled = true;
        isRunning = false;

        ObjectiveManager.objectiveManager.UpdateObjectiveText("WHAT WAS THAT!! I have to find a way downstairs and OUT of this building, fast!!");

        for (int i = 0; i < introLights.Length; i++)
        {
            introLights[i].SetActive(false);
        }

        pauseMenuManager.canPause = true;
        Player.player.footstepsSFX.enabled = true;


        Destroy(gameObject);
    }

    IEnumerator WaitForAnimation(Animator animator, string stateName)
    {
        
        // Wait until the animator is in the desired state
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            yield return null;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("rage"))
        {
            monsterAudio.PlayOneShot(monsterScream);
        }

        monster.GetComponent<Animator>().SetInteger("IdleIndex", 0);

        // Wait until it has finished playing
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;
    }
}
