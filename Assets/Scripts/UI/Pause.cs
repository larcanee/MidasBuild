/**
AUTHOR: Dillon Evans

DESCRIPTION:
Pauses or unpauses the game when one of the pause keys is pressed.
Also shows or hides a pause menu when the game is paused.

HOW TO USE:
1. Attach the Pause component to the pause menu (a canvas).
2. Set the pause keys in the Unity editor.
3. Add any components that need to be explicitly paused in the Unity editor.
   Pausing is done by setting Time.timeScale to 0.
   Update() is still called, but anything that relies on time
   (e.g. deltaTime, fixedDeltaTime, Time, etc.) will not occur.
   If something does not rely on time but should be paused,
   add it to the explicitly paused list.
**/


using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(Canvas))]
public class Pause : MonoBehaviour
{
    [Tooltip("Keys that will pause and unpause the game.")]
    public List<KeyCode> pauseKeys = new()
    {
        KeyCode.Escape, KeyCode.P, KeyCode.Pause,
        KeyCode.Joystick1Button6, KeyCode.Joystick1Button7,
        KeyCode.JoystickButton6, KeyCode.JoystickButton7
    };
    [Tooltip("Components that will be disabled when the game is paused.")]
    public List<MonoBehaviour> explicitlyPausedComponents = new();


    private Canvas canvas;
    private float originalTimeScale;


    // Toggles whether the game is paused or not.
    public void TogglePaused()
    {
        // Toggle the time scale.
        if (Time.timeScale == 0f)
        {
            Time.timeScale = originalTimeScale;
        }
        else
        {
            originalTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }
        // Toggle all components that need to be explicitly paused.
        foreach (MonoBehaviour component in explicitlyPausedComponents)
        {
            component.enabled = !component.enabled;
        }
        // Toggle the canvas with the pause menu.
        canvas.enabled = !canvas.enabled;
    }


    private void Start()
    {
        canvas = GetComponent<Canvas>();
    }


    private void Update()
    {
        if (pauseKeys.Any(key => Input.GetKeyDown(key)))
        {
            TogglePaused();
        }
    }
}
