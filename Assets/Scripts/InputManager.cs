using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool b_Input;
    public bool alt_Input;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void OnEnable()
    {
        if(playerControls == null) 
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.B.performed += i => b_Input = true;
            playerControls.PlayerActions.B.canceled += i => b_Input = false;

            playerControls.PlayerActions.Walking.performed += i => alt_Input = true;
            playerControls.PlayerActions.Walking.canceled += i => alt_Input = false;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleWalkingInput();
        if (playerLocomotion.isWalking)
            return;
        HandleSprintingInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isSprinting, playerLocomotion.isWalking);
    }

    private void HandleWalkingInput()
    {
        if (alt_Input && (!playerLocomotion.isSprinting && moveAmount > 0.1f))
        {
            playerLocomotion.isWalking = true;
            
        }
        else
            playerLocomotion.isWalking = false;
    }

    private void HandleSprintingInput()
    {
        if (b_Input && moveAmount > 0.5f)
            playerLocomotion.isSprinting = true;
        else
            playerLocomotion.isSprinting = false;
    }
}
