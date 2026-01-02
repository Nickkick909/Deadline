using UnityEngine;
using System.Collections;

public class SmartDoor : MonoBehaviour
{
    [Header("Setup")]
    public Transform hinge;                  // Rotation origin (RotationOrigin object)
    public float openAngle = 90f;            // How far it opens (positive or negative)
    public float speed = 180f;               // Degrees per second
    public Transform player;                 // Assign player camera

    private bool isOpen = false;
    private bool isMoving = false;

    private Quaternion closedRot;

    void Start()
    {
        closedRot = hinge.localRotation;
    }

    public void ToggleSmartDoor()
    {
        if (isMoving)
            return;

        // Determine which side of the door the player is on
        Vector3 doorRight = hinge.right;                       // because Z rotation  right vector determines swing
        Vector3 toPlayer = (player.position - hinge.position).normalized;

        float dot = Vector3.Dot(doorRight, toPlayer);

        // dot > 0  player is on the right side of door
        // dot < 0  left side
        float targetAngle = (dot > 0) ? openAngle : -openAngle;

        Quaternion targetRot = closedRot * Quaternion.Euler(0f, 0f, targetAngle);

        StartCoroutine(RotateDoor(targetRot));
    }

    private IEnumerator RotateDoor(Quaternion targetRotation)
    {
        isMoving = true;

        while (Quaternion.Angle(hinge.localRotation, targetRotation) > 0.1f)
        {
            hinge.localRotation = Quaternion.RotateTowards(
                hinge.localRotation,
                targetRotation,
                speed * Time.deltaTime
            );

            yield return null;
        }

        hinge.localRotation = targetRotation;
        isOpen = !isOpen;
        isMoving = false;
    }

    public IEnumerator ForceClose()
    {
        if (!isMoving)
            yield return RotateDoor(closedRot);
    }
}
