using UnityEngine;
using UnityEngine.InputSystem;

public class InteractWithObject : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField] float interactRange = 5f;
    [SerializeField] LayerMask interactMask;

    public delegate void CanInteractWithObject();
    public static CanInteractWithObject canInteractWithObject;

    public delegate void NotInteractWithObject();
    public static NotInteractWithObject notInteractWithObject;

    InteractObject currentObject;

    private InputAction interactButton;

    private void Awake()
    {
        interactButton = InputSystem.actions.FindAction("Interact");
    }

    void Start()
    {
        
    }


    void Update()
    {
        HandleInteract();   
    }

    void HandleInteract()
    {
        Vector3 forward = playerCamera.TransformDirection(Vector3.forward) * 10;
        RaycastHit hit;
        Physics.Raycast(playerCamera.position, forward, out hit, interactRange);

        //Debug.DrawRay(playerCamera.position, forward, Color.green);

        //Debug.Log("Hit: " + hit.transform.gameObject.name);

        if ((1 << hit.transform?.gameObject?.layer) != interactMask)
        {
            currentObject?.RemoveHighLight();
            return;
        }

        if (hit.transform != null)
        {
            InteractObject interactTemp = hit.transform.gameObject.GetComponent<InteractObject>();

            if (interactTemp != null)
            {
                interactTemp.HighLightObject();

                if (currentObject != null && currentObject != interactTemp)
                {
                    currentObject.RemoveHighLight();
                }

                currentObject = interactTemp;

                


            }
        } else
        {
            currentObject?.RemoveHighLight();

        }

        // Check for key input
        if (interactButton.WasPressedThisFrame())
        {
            currentObject?.HandleInteract();
        }




    }

}
