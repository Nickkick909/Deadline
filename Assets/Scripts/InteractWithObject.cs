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

    // Optional optimization: check ray every X seconds instead of every frame
    [SerializeField] private float rayCheckInterval = 0.02f; // 50 Hz
    private float rayCheckTimer = 0f;

    private void Awake()
    {
        interactButton = InputSystem.actions.FindAction("Interact");
    }

    void Start()
    {
        
    }


    void Update()
    {
        rayCheckTimer += Time.deltaTime;
        if (rayCheckTimer >= rayCheckInterval)
        {
            rayCheckTimer = 0f;
            HandleRaycast();
        }

        // Interact input can be checked every frame
        if (interactButton.WasPressedThisFrame())
        {
            currentObject?.HandleInteract();
        }
    }

    void HandleRaycast()
    {
        Vector3 forward = playerCamera.forward;
        RaycastHit hit;
        Physics.Raycast(playerCamera.position, forward, out hit, interactRange);

        // Keep your original layer check
        if ((1 << hit.transform?.gameObject?.layer) != interactMask)
        {
            currentObject?.RemoveHighLight();
            currentObject = null;
            return;
        }

        InteractObject hitObject = hit.transform.GetComponent<InteractObject>();

        // Only update highlight if the object changed
        if (hitObject != currentObject)
        {
            currentObject?.RemoveHighLight();
            if (hitObject != null)
            {
                hitObject.HighLightObject();
            }
            currentObject = hitObject;
        }

        //// Check for key input
        //if (interactButton.WasPressedThisFrame())
        //{
        //    currentObject?.HandleInteract();
        //}




    }

}
