using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class Mobile_input : MonoBehaviour
{
    public FixedJoystick movementJoystick;
    public float walkingSpeed = 5.0f;
    public float runningSpeed = 10.0f;
    public float gravity = 9.8f; // Adjust the gravity as needed
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public Camera playerCamera;

    private CharacterController characterController;
    private float rotationX = 0;
    private bool isRunning = false; // Flag to check if the player is running

    public Button speedIncreaseButton; // Reference to your UI button

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Attach the button click and release events using EventTrigger
        EventTrigger trigger = speedIncreaseButton.gameObject.AddComponent<EventTrigger>();

        // Add a pointer down event
        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((data) => { OnButtonPressed(); });
        trigger.triggers.Add(pointerDownEntry);

        // Add a pointer up event
        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
        pointerUpEntry.eventID = EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener((data) => { OnButtonReleased(); });
        trigger.triggers.Add(pointerUpEntry);
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        float horizontalInput = movementJoystick.Horizontal;
        float verticalInput = movementJoystick.Vertical;

        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);
        Vector3 movementDirection = Camera.main.transform.TransformDirection(movement);
        movementDirection.y = 0; // Keep the movement in the horizontal plane

        float speed = isRunning ? runningSpeed : walkingSpeed;

        // Apply gravity
        if (!characterController.isGrounded)
        {
            movementDirection.y -= gravity * Time.deltaTime;
        }

        // Move the character using SimpleMove (handles both movement and gravity)
        characterController.SimpleMove(movementDirection * speed);
    }

    void HandleRotation()
    {
        // Player rotation with mouse or joystick
        rotationX += -SimpleInput.GetAxis("LookY") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, SimpleInput.GetAxis("LookX") * lookSpeed, 0);
    }

    void OnButtonPressed()
    {
        // Set the flag to true when the button is pressed
        isRunning = true;
    }

    void OnButtonReleased()
    {
        // Set the flag to false when the button is released
        isRunning = false;
    }
}
