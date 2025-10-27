using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class PhonePuzzle : MonoBehaviour
{
    public GameObject[] phoneList;
    public AudioClip[] phoneAnswerClips;
    public int phoneIndex = 0;

    public AudioSource currentAudioSource;
    public PhoneFilterEffect phoneFilterEffect;

    public InteractObject doorToOpen;

    public AudioSource monsterAudioSource;
    public AudioSource monsterAudioSource2;

    public Light[] redLights;

    public void StartPhonePuzzle()
    {
        PlayPhoneRing();

        gameObject.GetComponent<BoxCollider>().enabled = false;

        ObjectiveManager.objectiveManager.UpdateObjectiveText("Quick!! Silence that phone!! The monster might hear it!!");

    }

    public void AnswerPhone()
    {
        
        StartCoroutine(WaitForAudio());
    }

    IEnumerator WaitForAudio()
    {
        currentAudioSource.Stop();
        currentAudioSource.gameObject.GetComponent<PhoneFilterEffect>().enableFilter = true;
        currentAudioSource.gameObject.GetComponent<AudioHighPassFilter>().enabled = true;
        currentAudioSource.gameObject.GetComponent<AudioLowPassFilter>().enabled = true;
        currentAudioSource.gameObject.GetComponent<AudioDistortionFilter>().enabled = true;
        currentAudioSource.gameObject.GetComponent<AudioEchoFilter>().enabled = true;

        currentAudioSource.volume = 0.1f;
        currentAudioSource.spatialBlend = 0f;

        currentAudioSource.clip = phoneAnswerClips[phoneIndex];
        currentAudioSource.loop = false;

        yield return new WaitForSeconds(0.5f);

        currentAudioSource.Play();

        phoneList[phoneIndex].GetComponent<BoxCollider>().enabled = false;

        while (currentAudioSource.isPlaying)
        {
            yield return null; // Wait for the next frame
        }

        yield return new WaitForSeconds(1);

        if (phoneIndex == 0)
        {
            monsterAudioSource.Play();
        }

        if (phoneIndex == 2)
        {
            monsterAudioSource2.Play();
        }

        phoneIndex++;

        if (phoneIndex >= phoneList.Length)
        {
            doorToOpen.requiresObject = false;

            for (int i = 0; i < redLights.Length; i++)
            {
                redLights[i].enabled = true;
            }
        } else
        {
            PlayPhoneRing();
        }



    }


    public void PlayPhoneRing()
    {
        Debug.Log("Playing next phone");
        currentAudioSource = phoneList[phoneIndex].GetComponent<AudioSource>();
        currentAudioSource.enabled = true;

        phoneList[phoneIndex].GetComponent<BoxCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        StartPhonePuzzle();
    }
}
