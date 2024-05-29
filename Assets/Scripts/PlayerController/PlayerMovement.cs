/**
AUTHOR: Dillon Evans

DESCRIPTION:
Checks for input from the player and moves the player accordingly.
Does not rotate or animate the player.

HOW TO USE:
1. Make sure the player has a Rigidbody2D component.
2. Attach the PlayerMovement component to the player.
3. Adjust the settings of the PlayerMovement component in the Unity editor.
**/


using UnityEngine;

using static InputUtils;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Normal Speed")]
    [Tooltip("Speed, in units per second.")]
    public float speed = 5f;
    [Tooltip(
        "How long it takes to accelerate, in seconds.\n" +
        "Used as the `smoothTime` parameter for SmoothDamp()."
    )]
    public float accelerationTime = 0.1f;

    [Header("Slow Speed")]
    [Tooltip("Whether or not moving slow is enabled.")]
    public bool canMoveSlow = false;
    [Tooltip("Speed when moving slow, in units per second.")]
    public float slowSpeed = 3f;
    [Tooltip("The name of the input axis for moving slow.")]
    public string slowAxisName = "Fire1";

    [Header("Fast Speed")]
    [Tooltip("Whether or not moving fast is enabled.")]
    public bool canMoveFast = false;
    [Tooltip("Speed when moving fast, in units per second.")]
    public float fastSpeed = 7f;
    [Tooltip("The name of the input axis for moving fast.")]
    public string fastAxisName = "Fire3";

    [Header("Controls")]
    [Tooltip("The name of the input axis for moving in the X direction.")]
    public string xAxisName = "Horizontal";
    [Tooltip("The name of the input axis for moving in the Y direction.")]
    public string yAxisName = "Vertical";
    [Tooltip("Whether to use GetInput() or GetInputRaw().")]
    public bool useRawInput = true;
    [Tooltip(
        "The deadzone of input.\n" +
        "Only used when useRawInput is true."
    )]
    public float deadzone = 0.19f;


    public Vector2 direction { get; private set; } = Vector2.zero;

    public enum SpeedState { Slow, Normal, Fast }
    public SpeedState speedState { get; private set; } = SpeedState.Normal;


    private Rigidbody2D rb2d = null;
    private Vector2 acceleration = Vector2.zero;

    public Animator midasAnimator;


    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        GetComponent<SpriteRenderer>().enabled = false;
        midasAnimator.SetBool("isWalking", false);
    }

    private void Update()
    {
        UpdateDirection();
        UpdateSpeedState();
    }

    private void FixedUpdate()
    {
        Vector2 targetVelocity = GetSpeed() * direction;
        rb2d.velocity = Vector2.SmoothDamp(
            rb2d.velocity,
            targetVelocity,
            ref acceleration,
            accelerationTime
        );

        if (rb2d.velocity != Vector2.zero)
        {
            midasAnimator.SetBool("isWalking", true);
        }
        else
        {
            midasAnimator.SetBool("isWalking", false);
        }
    }


    private void UpdateDirection()
    {
        Vector2 input = new(
            GetInputFromAxis(xAxisName, useRawInput),
            GetInputFromAxis(yAxisName, useRawInput)
        );
        direction = ApplyDeadzone(input, deadzone);
        direction = NormalizeIfNeeded(input);
    }

    private void UpdateSpeedState()
    {
        bool isSlow = canMoveSlow && IsAxisPressed(slowAxisName);
        bool isFast = canMoveFast && IsAxisPressed(fastAxisName);
        if (isSlow == isFast)
        {
            speedState = SpeedState.Normal;
        }
        else if (isSlow)
        {
            speedState = SpeedState.Slow;
        }
        else if (isFast)
        {
            speedState = SpeedState.Fast;
        }
    }

    private float GetSpeed()
    {
        switch (speedState)
        {
            case SpeedState.Slow:
                return slowSpeed;
            case SpeedState.Normal:
                return speed;
            case SpeedState.Fast:
                return fastSpeed;
            default:
                return 0f;
        }
    }
}
