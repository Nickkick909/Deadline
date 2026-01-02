using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class evelator_controll : MonoBehaviour
{
    public List<int> sequenceElevator = new List<int>();
    public bool Elevator_in_run;
    public float[] FloorHighs;
    public GameObject ElevatorCabin;

    public GameObject[] FloorNumbers;
    public int CurrentFloorNumber;

    public float DoorOpenTime;

    public GameObject[] Door_outside_left;
    public float[] Door_outside_left_close_value;
    public float[] Door_outside_left_open_value;
    public GameObject[] Door_outside_right;
    public float[] Door_outside_right_close_value;
    public float[] Door_outside_right_open_value;

    public GameObject Door_inside_right;
    public float Door_inside_right_close_value;
    public float Door_inside_right_open_value;
    public GameObject Door_inside_left;
    public float Door_inside_left_close_value;
    public float Door_inside_left_open_value;

    Coroutine EvelatorDo;
    public bool Doors_finished;

    public float TimePerFloor = 5f; // 5 seconds per floor


    public AudioSource elevatorMusic;
    public AudioSource elevatorHum;
    public AudioSource elevatorDing;
    public AudioSource elevatorDoors;


    // --- Add floor task ---
    public void AddTaskEve(string name)
    {
        int floor = -1;
        switch (name)
        {
            case "Button floor 1": floor = 0; break;
            case "Button floor 2": floor = 1; break;
            case "Button floor 3": floor = 2; break;
            case "Button floor 4": floor = 3; break;
            case "Button floor 5": floor = 4; break;
            case "Button floor 6": floor = 5; break;
        }

        if (floor >= 0)
        {
            sequenceElevator.Add(floor);

            if (!Elevator_in_run)
            {
                Elevator_in_run = true;
                EvelatorDo = StartCoroutine(executeTask());
            }
        }
    }

    // --- Main elevator coroutine ---
    public IEnumerator executeTask()
    {
        while (sequenceElevator.Count > 0)
        {
            int targetFloor = sequenceElevator[sequenceElevator.Count - 1];
            float targetHeight = FloorHighs[targetFloor];

            // Move elevator smoothly
            while (Mathf.Abs(ElevatorCabin.transform.localPosition.y - targetHeight) > 0.01f)
            {
                float direction = Mathf.Sign(targetHeight - ElevatorCabin.transform.localPosition.y);
                ElevatorCabin.transform.Translate(Vector3.up * direction * Time.deltaTime);
                yield return null;
            }

            ElevatorCabin.transform.localPosition = new Vector3(
                ElevatorCabin.transform.localPosition.x,
                targetHeight,
                ElevatorCabin.transform.localPosition.z
            );

            // Open doors
            Doors_finished = false;
            yield return StartCoroutine(HandleDoorOpen(targetFloor));

            // Wait until doors finish closing
            yield return new WaitUntil(() => Doors_finished);

            sequenceElevator.RemoveAt(sequenceElevator.Count - 1);

            ChangeFloorNumbers();
        }

        Elevator_in_run = false;
        // No StopCoroutine needed
    }

    public IEnumerator MoveToFloor(int floor)
    {
        StartCoroutine(PlayElevatorMovingSounds());

        float startY = ElevatorCabin.transform.localPosition.y;
        float targetY = FloorHighs[floor-1];
        float elapsed = 0f;

        // If already at the floor, just wait the "travel time" to keep consistency
        if (Mathf.Approximately(startY, targetY))
        {
            yield return new WaitForSeconds(TimePerFloor);
            yield break;
        }

        while (elapsed < TimePerFloor)
        {
            float t = elapsed / TimePerFloor; // normalized time 0->1
            float newY = Mathf.Lerp(startY, targetY, t);
            ElevatorCabin.transform.localPosition = new Vector3(
                ElevatorCabin.transform.localPosition.x,
                newY,
                ElevatorCabin.transform.localPosition.z
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure exact final position
        ElevatorCabin.transform.localPosition = new Vector3(
            ElevatorCabin.transform.localPosition.x,
            targetY,
            ElevatorCabin.transform.localPosition.z
        );

        // Update floor display
        ChangeFloorNumbers();

        StopElevatorMovingSounds();
    }

    public IEnumerator PlayElevatorMovingSounds()
    {
        elevatorHum.Play();

        elevatorMusic.volume = 0f;
        elevatorMusic.Play();

        float time = 0f;
        float duration = 0.5f;
        float targetVolume = 0.5f;
        while (time < duration)
        {
            time += Time.deltaTime;
            elevatorMusic.volume = Mathf.Lerp(0f, targetVolume, time / duration);
            yield return null;
        }

        elevatorMusic.volume = targetVolume;
    }

    public void StopElevatorMovingSounds()
    {
        elevatorHum.Stop();
        elevatorMusic.Stop();
    }

    // --- Door open/close coroutine ---
    public IEnumerator HandleDoorOpen(int WhichFloor)
    {
        
        elevatorDing.Play();

        yield return new WaitForSeconds(2f);

        elevatorDoors.Play();

        float elapsed = 0f;

        // Store starting positions
        Vector3 insideLeftStart = Door_inside_left.transform.localPosition;
        Vector3 insideRightStart = Door_inside_right.transform.localPosition;
        Vector3 outsideLeftStart = Door_outside_left[WhichFloor - 1].transform.localPosition;
        Vector3 outsideRightStart = Door_outside_right[WhichFloor - 1].transform.localPosition;

        // Target positions for opening
        Vector3 insideLeftTarget = new Vector3(Door_inside_left_open_value, insideLeftStart.y, insideLeftStart.z);
        Vector3 insideRightTarget = new Vector3(Door_inside_right_open_value, insideRightStart.y, insideRightStart.z);
        Vector3 outsideLeftTarget = new Vector3(Door_outside_left_open_value[WhichFloor - 1], outsideLeftStart.y, outsideLeftStart.z);
        Vector3 outsideRightTarget = new Vector3(Door_outside_right_open_value[WhichFloor - 1], outsideRightStart.y, outsideRightStart.z);

        // Smoothly open doors
        while (elapsed < DoorOpenTime)
        {
            float t = elapsed / DoorOpenTime;
            Door_inside_left.transform.localPosition = Vector3.Lerp(insideLeftStart, insideLeftTarget, t);
            Door_inside_right.transform.localPosition = Vector3.Lerp(insideRightStart, insideRightTarget, t);
            Door_outside_left[WhichFloor - 1].transform.localPosition = Vector3.Lerp(outsideLeftStart, outsideLeftTarget, t);
            Door_outside_right[WhichFloor - 1].transform.localPosition = Vector3.Lerp(outsideRightStart, outsideRightTarget, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure final positions
        Door_inside_left.transform.localPosition = insideLeftTarget;
        Door_inside_right.transform.localPosition = insideRightTarget;
        Door_outside_left[WhichFloor-1].transform.localPosition = outsideLeftTarget;
        Door_outside_right[WhichFloor-1].transform.localPosition = outsideRightTarget;

        Doors_finished = true;
        elevatorDoors.Stop();
    }

    public IEnumerator HandleDoorClose(int WhichFloor)
    {
        elevatorDoors.PlayOneShot(elevatorDoors.clip);
        float elapsed = 0f;

        // Store starting positions
        Vector3 insideLeftStart = Door_inside_left.transform.localPosition;
        Vector3 insideRightStart = Door_inside_right.transform.localPosition;
        Vector3 outsideLeftStart = Door_outside_left[WhichFloor - 1].transform.localPosition;
        Vector3 outsideRightStart = Door_outside_right[WhichFloor - 1].transform.localPosition;

        // Target positions for closing
        Vector3 insideLeftTarget = new Vector3(Door_inside_left_close_value, insideLeftStart.y, insideLeftStart.z);
        Vector3 insideRightTarget = new Vector3(Door_inside_right_close_value, insideRightStart.y, insideRightStart.z);
        Vector3 outsideLeftTarget = new Vector3(Door_outside_left_close_value[WhichFloor - 1], outsideLeftStart.y, outsideLeftStart.z);
        Vector3 outsideRightTarget = new Vector3(Door_outside_right_close_value[WhichFloor - 1], outsideRightStart.y, outsideRightStart.z);

        // Smoothly close doors
        while (elapsed < DoorOpenTime)
        {
            float t = elapsed / DoorOpenTime;
            Door_inside_left.transform.localPosition = Vector3.Lerp(insideLeftStart, insideLeftTarget, t);
            Door_inside_right.transform.localPosition = Vector3.Lerp(insideRightStart, insideRightTarget, t);
            Door_outside_left[WhichFloor - 1].transform.localPosition = Vector3.Lerp(outsideLeftStart, outsideLeftTarget, t);
            Door_outside_right[WhichFloor - 1].transform.localPosition = Vector3.Lerp(outsideRightStart, outsideRightTarget, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure final positions
        Door_inside_left.transform.localPosition = insideLeftTarget;
        Door_inside_right.transform.localPosition = insideRightTarget;
        Door_outside_left[WhichFloor - 1].transform.localPosition = outsideLeftTarget;
        Door_outside_right[WhichFloor - 1].transform.localPosition = outsideRightTarget;

        Doors_finished = true;

        elevatorDoors.Stop();
    }

    // --- Update floor number display ---
    public void ChangeFloorNumbers()
    {
        for (int i = 0; i < FloorHighs.Length; i++)
        {
            if (Mathf.Abs(ElevatorCabin.transform.localPosition.y - FloorHighs[i]) < 0.1f)
            {
                CurrentFloorNumber = i + 1;
                break;
            }
        }

        foreach (GameObject Numberassemble in FloorNumbers)
        {
            for (int i = 0; i < Numberassemble.transform.childCount; i++)
            {
                Numberassemble.transform.GetChild(i).gameObject.SetActive(i == CurrentFloorNumber - 1);
            }
        }
    }
}
