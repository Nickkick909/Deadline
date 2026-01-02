using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [System.Serializable]
    public class DoorGet
    {
        public GameObject Door;
        public int CloseValue;
        public int OpenValue;
        public bool isDoorOpen;
        public GameObject RotationOrigin;
    }

    public List<DoorGet> UseDoors = new List<DoorGet>();

    public bool door_in_use;
    public Coroutine DoorStartUsing;

    // ----------------------------
    // Normalizes any angle to 0-360 range
    // ----------------------------
    private float NormalizeAngle(float a)
    {
        a %= 360f;
        if (a < 0) a += 360f;
        return a;
    }

    // ----------------------------
    // Shortest angular difference
    // ----------------------------
    private bool IsCloseToAngle(float current, float target, float tolerance = 0.5f)
    {
        return Mathf.Abs(Mathf.DeltaAngle(current, target)) <= tolerance;
    }

    // ======================================================================

    public void MoveMyDoor()
    {
        foreach (var door in UseDoors)
        {
            if (door.Door == gameObject)
            {
                if (!door.isDoorOpen && !door_in_use)
                {
                    door_in_use = true;
                    door.isDoorOpen = true;
                    DoorStartUsing = StartCoroutine(OpenDoor(door.OpenValue, door.Door, door.RotationOrigin));
                }
                else if (door.isDoorOpen && !door_in_use)
                {
                    door_in_use = true;
                    door.isDoorOpen = false;
                    DoorStartUsing = StartCoroutine(CloseDoor(door.CloseValue, door.Door, door.OpenValue, door.RotationOrigin));
                }
            }
        }
    }

    public void ActionDoor()
    {
        foreach (var door in UseDoors)
        {
            door.Door.GetComponent<Door>().MoveMyDoor();
        }
    }

    // ======================================================================
    // OPEN DOOR
    // ======================================================================
    public IEnumerator OpenDoor(int targetAngle, GameObject currentDoor, GameObject RotationOri)
    {
        float target = NormalizeAngle(targetAngle);

        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            float current = NormalizeAngle(RotationOri.transform.localEulerAngles.z);

            // Rotate toward target
            float direction = Mathf.DeltaAngle(current, target) > 0 ? 1f : -1f;
            RotationOri.transform.Rotate(new Vector3(0, 0, direction * 95f * Time.deltaTime * 2f));

            current = NormalizeAngle(RotationOri.transform.localEulerAngles.z);

            // Stop when close enough
            if (IsCloseToAngle(current, target, 1f))
            {
                RotationOri.transform.localEulerAngles =
                    new Vector3(RotationOri.transform.localEulerAngles.x,
                                RotationOri.transform.localEulerAngles.y,
                                target);

                door_in_use = false;
                yield break;
            }
        }
    }

    // ======================================================================
    // CLOSE DOOR
    // ======================================================================
    public IEnumerator CloseDoor(int targetAngle, GameObject currentDoor, int openValue, GameObject RotationOri)
    {
        float target = NormalizeAngle(targetAngle);

        while (true)
        {
            yield return new WaitForSeconds(0.008f);

            float current = NormalizeAngle(RotationOri.transform.localEulerAngles.z);

            // Determine direction back to the closed position
            float direction = Mathf.DeltaAngle(current, target) > 0 ? 1f : -1f;

            // Rotate toward target
            RotationOri.transform.Rotate(new Vector3(0, 0, direction * 95f * Time.deltaTime * 2f));

            current = NormalizeAngle(RotationOri.transform.localEulerAngles.z);

            // Stop when close enough
            if (IsCloseToAngle(current, target, 1f))
            {
                RotationOri.transform.localEulerAngles =
                    new Vector3(RotationOri.transform.localEulerAngles.x,
                                RotationOri.transform.localEulerAngles.y,
                                target);

                door_in_use = false;
                yield break;
            }
        }
    }
}
