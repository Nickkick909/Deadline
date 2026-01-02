using System.Collections;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{

    public Transform[] elevatorFloorPositions;

    public evelator_controll controller;


    public Transform CabinTransform;
    public float ShakeAmplitude = 0.02f;
    public float ShakeFrequency = 20f;

    private Vector3 originalPos;

    bool elevatorMoving = false;

    bool looping = false;

    public int loopNumber = 0;

    public int blackoutTimeDelay = 10;
    public StoryEvent echoFloor;

    private void OnTriggerEnter(Collider other)
    {
        // close the doors

        // pay hum sound

        // vibrate the player

        // move the elevator

        if (other.CompareTag("Player"))
        {
            Player.player.currentElevator = CabinTransform.parent;
            Player.player.previousElevatorPosition = transform.parent.position;

            if (looping)
            {
                StartCoroutine(MoveElevator(3, 3));
                loopNumber++;
                Debug.Log("Next Loop");

            }
            else
            {
                StartCoroutine(MoveElevator(4, 3));

            }

            if (loopNumber == 1) 
            {
                // Time delay
                Debug.Log("loop number 1");
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!looping)
            {
                echoFloor.Play();
            }
            //other.transform.SetParent(null);
            Player.player.currentElevator = null;
            StartCoroutine(ResetElevator());

            
        }
    }



    IEnumerator MoveElevator(int currentFloor, int newFloor)
    {
        if (!looping)
        {
            yield return StartCoroutine(controller.HandleDoorClose(currentFloor));
        }

        elevatorMoving = true;


        if (currentFloor == newFloor)
        {
            StartCoroutine(controller.PlayElevatorMovingSounds());
            yield return new WaitForSeconds(controller.TimePerFloor);
            controller.StopElevatorMovingSounds();

        } else
        {
            yield return StartCoroutine(controller.MoveToFloor(newFloor));

        }

        elevatorMoving = false;

        yield return StartCoroutine(controller.HandleDoorOpen(newFloor));

    }

    IEnumerator ResetElevator()
    {
        yield return new WaitForSeconds(7f);

        StartCoroutine(controller.HandleDoorClose(3));

        looping = true;
    }


    private void Start()
    {
        if (CabinTransform != null)
            originalPos = CabinTransform.localPosition;
    }

    private void LateUpdate()
    {
        if (elevatorMoving && CabinTransform != null)
        {
            
            float x = Mathf.PerlinNoise(Time.time * ShakeFrequency, 0) - 0.5f;
            float y = Mathf.PerlinNoise(0, Time.time * ShakeFrequency) - 0.5f;
            if (looping) 
            {
                CabinTransform.localPosition = originalPos + new Vector3(x, y, 0) * ShakeAmplitude * 10;
            //    float rot = (Mathf.PerlinNoise(Time.time * ShakeFrequency, 1f) - 0.5f)
            //* ShakeAmplitude * 10 * 2f;

            //    CabinTransform.localRotation = Quaternion.Euler(0, 0, rot);
            }
            else
            {
                CabinTransform.localPosition = originalPos + new Vector3(x, y, 0) * ShakeAmplitude;

            }
        }
        else if (CabinTransform != null)
        {
            CabinTransform.localPosition = originalPos; // reset when stopped
        }
    }
}
