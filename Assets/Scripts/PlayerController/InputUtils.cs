/**
AUTHOR: Dillon Evans

DESCRIPTION:
Utility functions for getting input from the player.

HOW TO USE:
1. At the top of the file: using static InputUtils;
2. Use the functions: IsAxisPressed("Vertical");
or
1. Use the functions: InputUtils.IsAxisPressed("Vertical");

NOTE: This is not meant to be attached to a GameObject.
**/


using UnityEngine;


public class InputUtils
{
    public static float GetInputFromAxis(string axisName, bool useRawInput)
    {
        if (useRawInput)
        {
            return Input.GetAxisRaw(axisName);
        }
        else
        {
            return Input.GetAxis(axisName);
        }
    }

    public static Vector3 ApplyDeadzone(Vector3 input, float deadzone)
    {
        if (input.sqrMagnitude <= deadzone * deadzone)
        {
            return Vector3.zero;
        }
        return input;
    }

    public static bool IsAxisPressed(string axisName)
    {
        return Input.GetAxisRaw(axisName) != 0f;
    }

    public static Vector3 NormalizeIfNeeded(Vector3 vector)
    {
        if (vector.sqrMagnitude > 1f)
        {
            vector.Normalize();
        }
        return vector;
    }
}
