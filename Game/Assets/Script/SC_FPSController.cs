using UnityEngine;
using SimpleInputNamespace; // Make sure to add this namespace

[RequireComponent(typeof(CharacterController))]
public class SC_FPSController : MonoBehaviour
{
    public FixedJoystick joyStick;
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void Update()
    {
        // We are grounded, so recalculate move direction based on joystick axes or keyboard input
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        bool isRunning = joyStick && joyStick.Direction.magnitude > 0.5f && Input.GetKey(KeyCode.LeftShift);
        float speedMultiplier = isRunning ? runningSpeed : walkingSpeed;  // Adjust speed based on running state

        float curSpeedX = canMove ? speedMultiplier * (joyStick ? joyStick.Vertical : SimpleInput.GetAxis("Vertical")) : 0;
        float curSpeedY = canMove ? speedMultiplier * (joyStick ? joyStick.Horizontal : SimpleInput.GetAxis("Horizontal")) : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player rotation with mouse or joystick
        if (canMove)
        {
            rotationX += -SimpleInput.GetAxis("LookY") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, SimpleInput.GetAxis("LookX") * lookSpeed, 0);
        }

    }
}
