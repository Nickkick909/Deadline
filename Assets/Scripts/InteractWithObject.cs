using UnityEngine;

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
        Physics.Raycast(playerCamera.position, forward, out hit, interactRange, interactMask);

        Debug.DrawRay(playerCamera.position, forward, Color.green);

        //Debug.Log("Hit: " + hit.transform.gameObject.name);

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

                // Check for key input
                if (Input.GetKeyDown(currentObject.interactKey))
                {
                    currentObject.HandleInteract();
                }


            }
        } else
        {
            currentObject?.RemoveHighLight();

        }


        

    }

}
