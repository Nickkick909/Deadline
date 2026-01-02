using UnityEngine;

public class PortalToElevator : MonoBehaviour
{
    public Transform portalDestination;
    public Transform portalOrigin;

    public float rotationOffset;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController controller = other.GetComponent<CharacterController>();
            controller.enabled = false;
            Debug.Log("TELEPORT PLAYER");
            other.gameObject.transform.position = new Vector3(portalDestination.position.x, other.transform.position.y, portalDestination.position.z);

            other.transform.eulerAngles = new Vector3(other.transform.eulerAngles.x, other.transform.eulerAngles.y - rotationOffset, other.transform.transform.eulerAngles.z);

            controller.enabled = true;

        }
    }
}
