/**
AUTHOR: Dillon Evans

DESCRIPTION:
Animates "Paused..." text.

HOW TO USE:
1. Attach the PauseAnimation component to a TextMeshPro text object.
2. Set the frameTime and frames in the Unity editor.
**/


using System.Collections.Generic;
using TMPro;
using UnityEngine;


[RequireComponent(typeof(TextMeshProUGUI))]
public class PauseAnimation : MonoBehaviour
{
    [Tooltip("How long to wait between frames.")]
    public float frameTime = 0.1f;
    [Tooltip("Frames (strings) to switch between.")]
    public List<string> frames = new()
    {
        "Paused   ",
        "Paused.  ",
        "Paused.. ",
        "Paused...",
        "Paused ..",
        "Paused  ."
    };


    private TextMeshProUGUI text;

    private int currentFrame = 0;
    private float currentFrameTime = 0f;


    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = frames[currentFrame];
    }


    private void Update()
    {
        currentFrameTime += Time.unscaledDeltaTime;
        if (currentFrameTime > frameTime)
        {
            currentFrame = (currentFrame + 1) % frames.Count;
            text.text = frames[currentFrame];
            currentFrameTime = 0f;
        }
    }
}
