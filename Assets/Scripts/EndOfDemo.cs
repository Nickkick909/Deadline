using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EndOfDemo : MonoBehaviour
{

    public GameObject endOfDemoUI;

    public GameObject middleText;
    public GameObject objectiveText;

    public PauseMenuManager pauseMenuManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        endOfDemoUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Player.player.blockMovement = true;
        Player.player.footstepsSFX.enabled = false;

        pauseMenuManager.EnableBlur(true);

        middleText.SetActive(false);
        objectiveText.SetActive(false);
    }
}
