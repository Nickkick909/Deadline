using System.Collections;
using UnityEngine;

public class BreakerPuzzle : MonoBehaviour
{

    public bool[] breakerStates = new bool[8];

    public GameObject[] breakers = new GameObject[8];

    public InteractObject breakerFinal;

    public Material greenLight;
    public Material redLight;

    public InteractObject doorToOpen;
    public InteractObject doorToOpen2;

    public AudioClip breakerAudio;
    public AudioClip breakerOnAudio;
    public AudioSource breakerAudioSource;

    private bool init = false;
    public bool completed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        breakerStates = new bool[] { false, false, false, false, true, false, false, true };

        for (int i = 0; i < breakerStates.Length; i++)
        {
            UpdateBreakerVisuals(i);
        }

        init = true;
    }

   void CheckSolved()
    {
        for (int i = 0; i < breakerStates.Length; i++)
        {
            if (!breakerStates[i])
            {
                return;
            }
        }

        redLight.EnableKeyword("_EMISSION");

        for (int i = 0; i < breakerStates.Length; i++)
        {
            breakers[i].GetComponentInParent<BoxCollider>().enabled = false;
        }
        breakerFinal.gameObject.GetComponent<BoxCollider>().enabled = true;

        ObjectiveManager.objectiveManager.UpdateObjectiveText("Pull the main breaker!!");


    }

    public void ToggleBreaker(int i)
    {
        breakerStates[i] = !breakerStates[i];

        if (i > 0)
        {
            breakerStates[i - 1] = !breakerStates[i - 1];
            UpdateBreakerVisuals(i - 1);
        }

        if (i < breakerStates.Length - 1)
        {
            breakerStates[i + 1] = !breakerStates[i + 1];
            UpdateBreakerVisuals(i + 1);
        }

        // Move breaker visual
        UpdateBreakerVisuals(i);
        

        CheckSolved();
    }

    void UpdateBreakerVisuals(int i)
    {
        if (init)
        {
            breakerAudioSource.PlayOneShot(breakerAudio);
        }

        if (breakerStates[i])
        {
            breakers[i].transform.localPosition = new Vector3(-0.0700608864f, 0f, 0.1277868f);
        }
        else
        {
            breakers[i].transform.localPosition = new Vector3(0.0702856332f, 0f, 0.127786413f);
        }
    }


    public IEnumerator MoveMainBreaker()
    {
        breakerAudioSource.PlayOneShot(breakerAudio);
        float t = 0;
        while (t < 0.25f)
        {
            t += Time.deltaTime;
            //playerCamera.localPosition = Vector3.Lerp(startLocalPos, startLocalPos + new Vector3(0, 1, 0), t / 0.25f);
            breakerFinal.gameObject.transform.GetChild(0).localRotation = Quaternion.Euler(Vector3.Slerp(new Vector3(90, 0, 0),
                new Vector3(30, 0, 0),
                t / 0.25f));
            yield return null;
        }

        greenLight.EnableKeyword("_EMISSION");

        if (doorToOpen != null)
        {
            doorToOpen.requiresObject = false;
        }

        if (doorToOpen2 != null)
        {
            doorToOpen2.requiresObject = false;
        }


        ObjectiveManager.objectiveManager.UpdateObjectiveText("Power restored to the stair doors. I can go down now.");
        breakerAudioSource.PlayOneShot(breakerOnAudio);

        completed = true;

    }

}
