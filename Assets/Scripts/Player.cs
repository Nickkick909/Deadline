using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    private InputSystem_Actions inputs;

    [SerializeField]
    float playerSpeed = 5.0f;

    [SerializeField]
    float playerDefaultSpeed = 5f;
    [SerializeField]
    float playerSlowSpeed = 2.5f;

    [SerializeField]
    bool isGrounded = true;

    [SerializeField]
    public float mouseSpeed = 5f;

    [SerializeField]
    float jumpHeight = 5f;

    [SerializeField]
    Transform playerCamera;

    [SerializeField]
    float interactRange = 5f;

    Vector3 playerVelocity;

    CharacterController controller;
    float xRotation;

    public bool cursorLocked = false;

    [SerializeField]
    float cameraMaxPitch = 35f;

    [SerializeField]
    float cameraMinPitch = -35f;

    float gravityValue = -9.81f;

    public delegate void PlayerIsWalking(Vector3 move);
    public static PlayerIsWalking playerIsWalking;

    public bool blockMovement = false;

    [SerializeField] private List<GameObject> inventory;

    public static Player player;

    public AudioSource footstepsSFX;
    public InputAction escapeButton;

    Vector2 previousLookInput;
    private Vector2 movementInput;
    private Vector2 lookInput;

    [Header("Head Bob Settings")]
    [SerializeField] private float bobAmplitude = 0.03f;
    [SerializeField] private float bobFrequency = 8f;
    private float bobTimer = 0f;
    private Vector3 cameraStartPos;

    public AudioSource voicelineAs;


    private void Awake()
    {
        player = this;

        inputs = new InputSystem_Actions();
        inputs.Player.Enable();


        //escapeButton = InputSystem.actions.FindAction("Escape");
        
    }

    public void SetPlayerSpeed(bool isSlowed)
    {
        playerSpeed = isSlowed ? playerSlowSpeed : playerDefaultSpeed;
    }
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraStartPos = playerCamera.localPosition;

        //Cursor.lockState = CursorLockMode.Locked;
        //cursorLocked = true;

        //Vector3 savedPlayerPosition = new Vector3(PlayerPrefs.GetFloat("playerXPosition", 0),
        //    PlayerPrefs.GetFloat("playerYPosition", 1), PlayerPrefs.GetFloat("playerXPosition", 0));

        //transform.position = savedPlayerPosition;
    }

    private void Update()
    {
        //if (escapeButton.WasPressedThisFrame()) {
        //    cursorLocked = false;
        //    Cursor.lockState = CursorLockMode.None;
        //}

        //if (!cursorLocked && Mouse.current.leftButton.wasPressedThisFrame && Application.isFocused)
        //{
        //    player.cursorLocked = true;
        //    Cursor.lockState = CursorLockMode.Locked;
        //}

        movementInput = inputs.Player.Move.ReadValue<Vector2>();
        lookInput = inputs.Player.Look.ReadValue<Vector2>();

        if (!blockMovement)
        {
            HandleMovement();

        }
    }
    void LateUpdate()
    {
        if (!blockMovement)
        {
            HandleLook();
        }
    }

    private void OnApplicationQuit()
    {
        //PlayerPrefs.SetFloat("playerXPosition", transform.position.x);
        //PlayerPrefs.SetFloat("playerYPosition", transform.position.y);
        //PlayerPrefs.SetFloat("playerZPosition", transform.position.z);
    }

    public void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        //Vector2 movementInput = inputs.Player.Move.ReadValue<Vector2>();
        float verticalInput = movementInput[1];
        float horizontalInput = movementInput[0];

        Vector3 move = (transform.right * horizontalInput) + (transform.forward * verticalInput);
        controller.Move(playerSpeed * Time.smoothDeltaTime * move.normalized);

        if (!isGrounded)
        {
            playerVelocity.y += gravityValue * Time.smoothDeltaTime;
        }

        controller.Move(playerVelocity * Time.smoothDeltaTime);

        if (movementInput.magnitude >= 0.1)
        {
            footstepsSFX.UnPause();
        } else
        {
            footstepsSFX.Pause();
        }

        playerIsWalking?.Invoke(move);

        if (movementInput.magnitude > 0.1f && isGrounded)
        {
            bobTimer += Time.smoothDeltaTime * bobFrequency * movementInput.magnitude;
            float adjustedAmplitude = bobAmplitude * movementInput.magnitude;
            Vector3 offset = new Vector3(0, Mathf.Sin(bobTimer) * adjustedAmplitude, 0);
            playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, cameraStartPos + offset, Time.smoothDeltaTime * 10f);
        }
        else
        {
            // Smoothly return to default position
            playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, cameraStartPos, Time.smoothDeltaTime * 5f);
            bobTimer = 0f;
        }
    }

    void HandleLook()
    {
        //Vector2 lookInput = inputs.Player.Look.ReadValue<Vector2>();
        lookInput = Vector2.Lerp(previousLookInput, lookInput, 0.5f);
        previousLookInput = lookInput;
        float mouseX = lookInput[0] * mouseSpeed * Time.smoothDeltaTime;
        float mouseY = lookInput[1] * mouseSpeed * Time.smoothDeltaTime;

        transform.Rotate(Vector3.up, mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, cameraMinPitch, cameraMaxPitch);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    public void AddItemToInventory(GameObject newItem)
    {
        inventory.Add(newItem);
    }

    public bool CheckIfPlayerHasItem(GameObject itemToCheck)
    {
        if (inventory.Contains(itemToCheck))
        {
            return true;
        } else
        {
            return false;
        }
    }

    public void PlayVoiceLine (AudioClip voiceClip)
    {
        voicelineAs.PlayOneShot(voiceClip);
    }
}
