using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private bool isMobile = false;
    private bool isSwiping = false;
    private Vector2 swipeDelta;
    private Vector2 keyboardInput;

    public float swipeSensitivity = 2.0f;
    public float movementSpeed = 5.0f;

    private Rigidbody rb;
    private Animator animator;

    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Speed = Animator.StringToHash("Speed");

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Check if the current device is a mobile platform
        isMobile = Application.isMobilePlatform;
    }

    private void OnSwipe(InputValue inputValue)
    {
        // Process swipes only on mobile devices
        if (isMobile)
        {
            Vector2 swipeValue = inputValue.Get<Vector2>();
            swipeDelta = swipeValue * swipeSensitivity;

            if (swipeDelta.magnitude > 1)
            {
                swipeDelta.Normalize();
                isSwiping = true;
            }
            else
            {
                isSwiping = false;
            }
        }
    }

    private void OnMove(InputValue inputValue)
    {
        // Process movement only on the keyboard
        if (!isMobile)
        {
            keyboardInput = inputValue.Get<Vector2>();
        }
    }

    public void Update()
    {
        if (isMobile)
        {
            animator.SetFloat(Horizontal, swipeDelta.x);
            animator.SetFloat(Vertical, swipeDelta.y);
            animator.SetFloat(Speed, swipeDelta.sqrMagnitude);
        }
        else
        {
            animator.SetFloat(Horizontal, keyboardInput.x);
            animator.SetFloat(Vertical, keyboardInput.y);
            animator.SetFloat(Speed, keyboardInput.sqrMagnitude);
        }
    }

    private void FixedUpdate()
    {
        if (isMobile && isSwiping)
        {
            rb.MovePosition(rb.position + new Vector3(swipeDelta.x, 0, 0) * (movementSpeed * Time.fixedDeltaTime));
            isSwiping = false;
        }
        else if (!isMobile)
        {
            rb.MovePosition(rb.position + new Vector3(keyboardInput.x, 0, keyboardInput.y) * (movementSpeed * Time.fixedDeltaTime));
        }
    }

    void MoveCharacter()
    {
        // Additional actions if necessary
    }
}