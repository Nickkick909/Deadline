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

        if (globalVolume.profile.TryGet(out depthOfField))
        {
            depthOfField.active = false;
        }

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
        //else
        //    ResumeGame();
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.blockMovement = true;

        EnableBlur(true);

        middleText.SetActive(false);
        objectiveText.SetActive(false);
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
        player.blockMovement = false;

        EnableBlur(false);

        middleText.SetActive(true);
        objectiveText.SetActive(true);
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
            depthOfField.active = true; // keep active if you want subtle DoF
            depthOfField.mode.value = DepthOfFieldMode.Gaussian;
            depthOfField.gaussianStart.value = dofStart;
            depthOfField.gaussianEnd.value = dofEnd;
            vignette.intensity.value = Mathf.Clamp01(defaultVignette);
        }
    }
}
