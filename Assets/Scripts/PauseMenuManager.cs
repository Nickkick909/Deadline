using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] Player player;
    [SerializeField] Slider sensitivitySlider;

    private bool isPaused;
    private InputAction pauseAction;

    public Volume globalVolume;
    private DepthOfField depthOfField;
    private Vignette vignette;

    private float defaultVignette;
    private float pauseVignette = 0.2f;

    private float dofStart;
    private float dofEnd;

    public bool canPause = true;

    public GameObject middleText;
    public GameObject objectiveText;

    public bool debug;


    void Awake()
    {
        pauseAction = InputSystem.actions.FindAction("Escape"); // Bind your "Pause" input
        pauseAction.performed += _ => TogglePause();
    }

    void Start()
    {
        pauseMenuUI.SetActive(false);
        sensitivitySlider.value = player.mouseSpeed;
        sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);

        globalVolume.profile.TryGet(out depthOfField);


        globalVolume.profile.TryGet(out vignette);

        dofStart = depthOfField.gaussianStart.value;
        dofEnd = depthOfField.gaussianEnd.value;

    }

    void TogglePause()
    {
        if (!canPause)
        {
            return;
        }
        isPaused = !isPaused;

        if (isPaused)
            PauseGame();
        else if (debug && !isPaused)
            ResumeGame();
    }

    public void PauseGame()
    {

        if (!debug)
        {
            depthOfField.active = true;
            pauseMenuUI.SetActive(true);

            EnableBlur(true);

            middleText.SetActive(false);
            objectiveText.SetActive(false);
        }

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.blockMovement = true;
        AudioListener.pause = true;
    }

    public void ResumeGame()
    {

        if (!debug)
        {
            depthOfField.active = false;
            pauseMenuUI.SetActive(false);

            EnableBlur(false);

            middleText.SetActive(true);
            objectiveText.SetActive(true);
        }

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
        player.blockMovement = false;
        AudioListener.pause = false;
    }

    public void UpdateSensitivity(float newValue)
    {
        player.mouseSpeed = newValue;
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void EnableBlur(bool enable)
    {
        if (depthOfField == null && globalVolume.profile.TryGet(out depthOfField))
            return;




        if (enable)
        {
            depthOfField.active = true;
            depthOfField.mode.value = DepthOfFieldMode.Gaussian;
            depthOfField.gaussianStart.value = 0f;
            depthOfField.gaussianEnd.value = 0f;
            vignette.intensity.value = Mathf.Clamp01(pauseVignette);
        }
        else
        {
            depthOfField.active = false; // keep active if you want subtle DoF
            depthOfField.mode.value = DepthOfFieldMode.Gaussian;
            depthOfField.gaussianStart.value = dofStart;
            depthOfField.gaussianEnd.value = dofEnd;
            vignette.intensity.value = Mathf.Clamp01(defaultVignette);
        }
    }
}
