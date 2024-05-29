/**
AUTHOR: Dillon Evans

DESCRIPTION:
Checks for input from the player and aims accordingly.
Rotates the player to look at where they are aiming.

HOW TO USE:
1. Attach the PlayerAiming component to the player.
2. Adjust the settings of the PlayerAiming component in the Unity editor.
**/


using UnityEngine;

using static InputUtils;


public class PlayerAiming : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The direction the player is facing when rotation is 0.")]
    public Vector2 initialForward = Vector2.up;

    [Header("Mouse Controls")]
    [Tooltip("Whether or not aiming with the mouse is enabled.")]
    public bool canAimWithMouse = true;

    [Header("Axes Controls")]
    [Tooltip("Whether or not aiming with axes (keyboard or controller) is enabled.")]
    public bool canAimWithAxes = true;
    [Tooltip("The name of the input axis for aiming in the X direction.")]
    public string xAxisName = "Aim Horizontal";
    [Tooltip("The name of the input axis for aiming in the Y direction.")]
    public string yAxisName = "Aim Vertical";
    [Tooltip("Whether to use GetInput() or GetInputRaw().")]
    public bool useRawInput = true;
    [Tooltip(
        "The deadzone of input.\n" +
        "Only used when useRawInput is true."
    )]
    public float deadzone = 0.19f;


    public Vector2 aimingAt { get; private set; } = Vector2.zero;


    private bool isAimingWithMouse = true;


    private void Start()
    {
        isAimingWithMouse = canAimWithMouse;
        UpdateAimingAt();
    }

    private void Update()
    {
        UpdateAimingInput();
        UpdateAimingAt();
        Rotate();
    }


    private void UpdateAimingInput()
    {
        if (canAimWithMouse && IsMouseMoving())
        {
            isAimingWithMouse = true;
        }
        else if (canAimWithAxes && AreAxesMoving())
        {
            isAimingWithMouse = false;
        }
    }

    private void UpdateAimingAt()
    {
        if (canAimWithMouse && isAimingWithMouse)
        {
            aimingAt = GetMousePosition();
        }
        else if (canAimWithAxes && !isAimingWithMouse)
        {
            aimingAt = GetAxesPosition();
        }
        else
        {
            aimingAt = Vector2.zero;
        }
    }

    private void Rotate()
    {
        transform.localRotation = Quaternion.FromToRotation(initialForward, aimingAt);
    }


    private bool IsMouseMoving()
    {
        return Input.mousePositionDelta != Vector3.zero;
    }

    private bool AreAxesMoving()
    {
        return GetInputFromAxes() != Vector2.zero;
    }


    private Vector2 GetMousePosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        Rect pixelRect = Camera.main.pixelRect;
        mousePosition.x = Mathf.Clamp(mousePosition.x, pixelRect.xMin, pixelRect.xMax);
        mousePosition.y = Mathf.Clamp(mousePosition.y, pixelRect.yMin, pixelRect.yMax);
        return Camera.main.ScreenToWorldPoint(mousePosition) - transform.position;
    }

    private Vector2 GetAxesPosition()
    {
        Vector2 input = GetInputFromAxes();
        input += Vector2.one;
        input *= 0.5f;
        return Camera.main.ViewportToWorldPoint(input) - transform.position;
    }


    private Vector2 GetInputFromAxes()
    {
        Vector3 input = new(
            GetInputFromAxis(xAxisName, useRawInput),
            GetInputFromAxis(yAxisName, useRawInput)
        );
        input = ApplyDeadzone(input, deadzone);
        return input;
    }
}
