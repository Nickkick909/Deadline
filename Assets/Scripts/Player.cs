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
    float mouseSpeed = 5f;

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


    private void Awake()
    {
        player = this;

        inputs = new InputSystem_Actions();
        inputs.Player.Enable();


        escapeButton = InputSystem.actions.FindAction("Escape");
        
    }

    public void SetPlayerSpeed(bool isSlowed)
    {
        playerSpeed = isSlowed ? playerSlowSpeed : playerDefaultSpeed;
    }
    void Start()
    {
        controller = GetComponentInChildren<CharacterController>();

        Cursor.lockState = CursorLockMode.Confined;
        cursorLocked = false;

        //Vector3 savedPlayerPosition = new Vector3(PlayerPrefs.GetFloat("playerXPosition", 0),
        //    PlayerPrefs.GetFloat("playerYPosition", 1), PlayerPrefs.GetFloat("playerXPosition", 0));

        //transform.position = savedPlayerPosition;
    }

    private void Update()
    {
        if (escapeButton.WasPressedThisFrame()) {
            cursorLocked = false;
            Cursor.lockState = CursorLockMode.None;
        }

        if (!cursorLocked && Mouse.current.leftButton.wasPressedThisFrame && Application.isFocused)
        {
            player.cursorLocked = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    void LateUpdate()
    {
        if (!blockMovement)
        {
            HandleLook();
        }
    }

    private void FixedUpdate()
    {
        if (!blockMovement)
        {
            HandleMovement();
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

        Vector2 movementInput = inputs.Player.Move.ReadValue<Vector2>();
        float verticalInput = movementInput[1];
        float horizontalInput = movementInput[0];

        Vector3 move = (transform.right * horizontalInput) + (transform.forward * verticalInput);
        controller.Move(playerSpeed * Time.deltaTime * move.normalized);

        if (!isGrounded)
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
        }

        controller.Move(playerVelocity * Time.deltaTime);

        if (movementInput.magnitude >= 0.1)
        {
            footstepsSFX.UnPause();
        } else
        {
            footstepsSFX.Pause();
        }

        playerIsWalking?.Invoke(move);
    }

    void HandleLook()
    {
        Vector2 lookInput = inputs.Player.Look.ReadValue<Vector2>();
        float mouseX = lookInput[0] * mouseSpeed * Time.deltaTime;
        float mouseY = lookInput[1] * mouseSpeed * Time.deltaTime;

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
}
