using UnityEngine;

public class StartGame : MonoBehaviour
{

    [SerializeField] GameObject StartSequence;
    [SerializeField] GameObject cineCam;
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject flashlightBar;
    [SerializeField] GameObject objectiveUI;
    [SerializeField] OffsetFlashlight flashlight;
    [SerializeField] Player player;

    [SerializeField] Vector3 playerStartPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flashlightBar.SetActive(false);
        flashlight.TurnOffFlashlight();
        player.blockMovement = true;
        Cursor.lockState = CursorLockMode.Locked;
        player.gameObject.GetComponent<InteractWithObject>().enabled = false;
        objectiveUI.SetActive(false);

        player.enabled = false;
        player.footstepsSFX.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGameButton()
    {
        Debug.Log("Start Game!!!");
        StartSequence.SetActive(false);
        cineCam.SetActive(false);
        startMenu.SetActive(false);
        objectiveUI.SetActive(true);

        ObjectiveManager.objectiveManager?.UpdateObjectiveText("Time to go home after a long day at the office.");

        Camera.main.transform.localPosition = new Vector3(0, 0, 0);

        flashlightBar.SetActive(true);
        player.blockMovement = false;
        player.cursorLocked = true;
        Cursor.lockState = CursorLockMode.Locked;

        player.gameObject.transform.position = playerStartPosition;
        player.gameObject.GetComponent<InteractWithObject>().enabled = true;
        
        player.enabled = true;
        player.footstepsSFX.enabled = true;
    }
}
