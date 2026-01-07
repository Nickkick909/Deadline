using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
    [SerializeField] GameObject pauseMenuManager;

    [SerializeField] bool startFullGame = true;
    [SerializeField] bool enableRedLights = false;

    public Volume globalVolume;
    private DepthOfField depthOfField;

    public StoryEvent topFloorsStoryManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = -1;
        flashlightBar.SetActive(false);
        flashlight.TurnOffFlashlight();
        
        Cursor.lockState = CursorLockMode.Locked;

        

        

        if (startFullGame)
        {
            StartSequence.SetActive(true);
            cineCam.SetActive(true);
            player.enabled = false;
            player.transform.position = playerStartPosition;
            player.blockMovement = true;
            player.gameObject.GetComponent<InteractWithObject>().enabled = false;
            player.footstepsSFX.enabled = false;
            objectiveUI.SetActive(false);
            pauseMenuManager.SetActive(false);

            if (globalVolume.profile.TryGet(out depthOfField))
            {
                depthOfField.active = true;
            }
        }
        else
        {
            StartSequence.SetActive(false);
            cineCam.SetActive(false);
            Camera.main.transform.localPosition = new Vector3(0, 0, 0);
            player.enabled = true;
            startMenu.SetActive(true);
            player.blockMovement = false;
            player.gameObject.GetComponent<InteractWithObject>().enabled = true;
            player.footstepsSFX.enabled = true;
            objectiveUI.SetActive(true);
            pauseMenuManager.SetActive(true);

            if (enableRedLights)
            {
                GameObject.FindAnyObjectByType<PhonePuzzle>().lightsAnim.SetBool("RedLights", true);
            }


            if (globalVolume.profile.TryGet(out depthOfField))
            {
                depthOfField.active = false;

            }

        }
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
        pauseMenuManager.SetActive(true);

        Camera.main.transform.localPosition = new Vector3(0, 0, 0);
        Camera.main.fieldOfView = 90;

        //flashlightBar.SetActive(true);
        player.blockMovement = false;
        player.cursorLocked = true;
        Cursor.lockState = CursorLockMode.Locked;

        player.gameObject.transform.position = playerStartPosition;
        player.gameObject.GetComponent<InteractWithObject>().enabled = true;
        
        player.enabled = true;
        player.footstepsSFX.enabled = true;

        depthOfField.active = false;

        StartCoroutine(WaitToShowObjective());
    }
    

    IEnumerator WaitToShowObjective()
    {
        yield return new WaitForSeconds(0.25f);

        topFloorsStoryManager.GetComponent<StoryEvent>().Play();

        ObjectiveManager.objectiveManager.UpdateObjectiveText("Time to go home after a long day at the office.");

        yield return new WaitForSeconds(8f);

        ObjectiveManager.objectiveManager.UpdateObjectiveText("Press \"F\" to turn on/off your Flashlight.");

        yield return new WaitForSeconds(10f);

        if (ObjectiveManager.objectiveManager.objectiveText.text == "Press \"F\" to turn on/off your Flashlight.")
        {
            ObjectiveManager.objectiveManager.UpdateObjectiveText("Time to go home after a long day at the office.", false);

        }
    }
}
