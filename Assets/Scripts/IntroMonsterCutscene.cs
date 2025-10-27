using System.Collections;
using UnityEngine;

public class IntroMonsterCutscene : MonoBehaviour
{
    public Transform playerCamera;
    public Transform monsterTarget;
    public Transform monster;
    public float panDuration = 3f;
    public float returnDuration = 3f;
    private bool isRunning;

    public InteractWithObject interactWithObject;

    

    private void OnTriggerEnter(Collider other)
    {
        PlayCutscene();
    }

    public void PlayCutscene()
    {
        
        StartCoroutine(CutsceneRoutine());
    }

    IEnumerator CutsceneRoutine()
    {
        OffsetFlashlight.playerFlashlight.dontFollowCam = true;
        Player.player.blockMovement = true;
        interactWithObject.enabled = false;
        //OffsetFlashlight.playerFlashlight.TurnOffFlashlight();
        isRunning = true;

        Vector3 startLocalPos = playerCamera.localPosition;
        Quaternion startLocalRot = playerCamera.localRotation;

        //Debug.Log("START POS:" + startPos);

        Vector3 targetLocalPos = playerCamera.parent.InverseTransformPoint(monsterTarget.position);
        Quaternion targetLocalRot = Quaternion.LookRotation(
            playerCamera.parent.InverseTransformPoint(monster.position) - targetLocalPos
        );
        monster.GetComponent<Animator>().SetInteger("IdleIndex", 2);
        // Go up first
        float t = 0;
        while (t < 0.25f)
        {
            t += Time.deltaTime;
            playerCamera.localPosition = Vector3.Lerp(startLocalPos, startLocalPos + new Vector3(0, 1, 0), t / 0.25f);
            //playerCamera.rotation = Quaternion.Slerp(startLocalRot,
            //    targetLocalRot,
            //    t / 0.25f);
            yield return null;
        }

        // --- PAN TO MONSTER ---
        t = 0f;
        while (t < panDuration)
        {
            t += Time.deltaTime;
            float progress = t / panDuration;
            playerCamera.localPosition = Vector3.Lerp(startLocalPos + new Vector3(0, 1, 0), targetLocalPos, progress);
            playerCamera.localRotation = Quaternion.Slerp(startLocalRot, targetLocalRot, progress);
            yield return null;
        }

        // --- MONSTER ACTIONS ---
        monster.GetComponent<Animator>().SetInteger("IdleIndex", 1);
        //yield return new WaitForSeconds(2f);
        yield return StartCoroutine(WaitForAnimation(monster.GetComponent<Animator>(), "idle1"));

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


        Destroy(gameObject);
    }

    IEnumerator WaitForAnimation(Animator animator, string stateName)
    {
        // Wait until the animator is in the desired state
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            yield return null;

        // Wait until it has finished playing
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;
    }
}
