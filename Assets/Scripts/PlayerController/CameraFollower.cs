/**
AUTHOR: Dillon Evans

DESCRIPTION:
Moves the camera to be between the player and what the player is aiming at.

HOW TO USE:
1. Attach the CameraFollower component to the camera.
2. Adjust the settings of the CameraFollower component in the Unity editor.
**/


using UnityEngine;


public class CameraFollower : MonoBehaviour
{
    [Tooltip("The player to follow.")]
    public PlayerAiming player = null;

    [Tooltip(
        "How much to consider the position the player is aiming at.\n" +
        "0 if the position aimed at should be ignored.\n" +
        "0.5 if the camera should be exactly between the player and the position aimed at."
    )]
    [Range(-0.5f, 0.5f)]
    public float aimingWeight;

    [Tooltip("How long, in seconds, the camera takes to move to the target position.")]
    [Min(0f)]
    public float floatiness;


    private Vector3 velocity = Vector3.zero;


    private void FixedUpdate()
    {
        UpdatePosition(Time.fixedDeltaTime);
    }


    private void UpdatePosition(float deltaTime)
    {
        transform.position = Vector3.SmoothDamp(
            transform.position,
            TargetPosition(),
            ref velocity,
            floatiness,
            Mathf.Infinity,
            deltaTime
        );
    }


    private Vector3 TargetPosition()
    {
        return new(
            player.transform.position.x + aimingWeight * player.aimingAt.x,
            player.transform.position.y + aimingWeight * player.aimingAt.y,
            transform.position.z
        );
    }
}
