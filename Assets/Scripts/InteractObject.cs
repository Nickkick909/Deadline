using System.Collections;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    [SerializeField] Outline outline;
    [SerializeField] bool isTargeted = false;
    [SerializeField] InteractType interactType;
    [SerializeField] ItemType itemType;

    public KeyCode interactKey;

    public GameObject requiredObject;
    public bool requiresObject = false;

    private Color regularOutlineColor = Color.white;
    private Color declinedOutlineColor = Color.red;

    [SerializeField] string interactString = "";
    [SerializeField] string cannotInteractString = "";


    public BreakerPuzzle breakerPuzzle;
    public PhonePuzzle phonePuzzle;
    public int itemIndex;

    public AudioSource audioSource;

    void Start()
    {
        //outline = GetComponent<Outline>();
        if (outline != null) 
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

        if (requiresObject && !Player.player.CheckIfPlayerHasItem(requiredObject))
        {
            outline.OutlineColor = declinedOutlineColor;
            MiddleScreenText.updateMiddleScreenText?.Invoke(cannotInteractString);
        } else
        {
            outline.OutlineColor = regularOutlineColor;
            MiddleScreenText.updateMiddleScreenText?.Invoke(interactString);
        }
        
    }

    public void RemoveHighLight()
    {
        isTargeted = false;
        if(outline != null) 
            outline.enabled = false;

        MiddleScreenText.updateMiddleScreenText?.Invoke("");
    }

    public virtual void HandleInteract()
    {
        if (requiresObject && !Player.player.CheckIfPlayerHasItem(requiredObject))
        {
            return;
        }

        if (interactType == InteractType.Pickup)
        {
            //InventoryObject item = new InventoryObject();
            //item.quantity = 1;
            //item.type = itemType;
            //item.worldItem = gameObject;

            //Inventory.pickUpItem(item);
            if (audioSource != null)
            {
                audioSource.PlayOneShot(audioSource.clip);
            }

            Player.player.AddItemToInventory(gameObject);

            StartCoroutine(WaitForAudio());
        }

        if (interactType == InteractType.Door)
        {
            Debug.Log("Interact with door....");
            gameObject.GetComponent<Door>().ActionDoor();

            if (audioSource != null)
            {
                audioSource.PlayOneShot(audioSource.clip);
            }
        }

        if (interactType == InteractType.Breaker)
        {
            breakerPuzzle.ToggleBreaker(itemIndex);
        }

        if (interactType == InteractType.BreakerFinal)
        {
            StartCoroutine(breakerPuzzle.MoveMainBreaker());
            this.enabled = false;
        }

        if (interactType == InteractType.Phone)
        {
            phonePuzzle.AnswerPhone();
        }
    }

    IEnumerator WaitForAudio()
    {
        while (audioSource.isPlaying)
        {
            yield return null; // Wait for the next frame
        }

        Destroy(gameObject);

    }
}

public enum InteractType
{
    Pickup,
    Door,
    Breaker,
    BreakerFinal,
    Phone
}

public enum ItemType
{
    Log,
    Campfire,
    Grass,
    SmallRock
}
