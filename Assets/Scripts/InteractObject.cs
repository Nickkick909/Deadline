using UnityEngine;

public class InteractObject : MonoBehaviour
{
    [SerializeField] Outline outline;
    bool isTargeted = false;
    [SerializeField] InteractType interactType;
    [SerializeField] ItemType itemType;

    public KeyCode interactKey;

    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        InteractWithObject.canInteractWithObject += HighLightObject;
    }

    private void OnDisable()
    {
        InteractWithObject.canInteractWithObject -= HighLightObject;
    }

    public void HighLightObject()
    {
        isTargeted = true;
        if (outline != null)
            outline.enabled = true;
    }

    public void RemoveHighLight()
    {
        isTargeted = false;
        if(outline != null) 
            outline.enabled = false;
    }

    public virtual void HandleInteract()
    {
        if (interactType == InteractType.Pickup)
        {
            //InventoryObject item = new InventoryObject();
            //item.quantity = 1;
            //item.type = itemType;
            //item.worldItem = gameObject;

            //Inventory.pickUpItem(item);

            Destroy(gameObject);
        }
    }
}

public enum InteractType
{
    Pickup,
}

public enum ItemType
{
    Log,
    Campfire,
    Grass,
    SmallRock
}
