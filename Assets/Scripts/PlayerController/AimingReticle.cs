/**
AUTHOR: Dillon Evans

DESCRIPTION:
Moves the object to the position the player is aiming at.

HOW TO USE:
1. Attach the AimingReticle component to an object that will be the aiming reticle.
**/


using UnityEngine;


public class AimingReticle : MonoBehaviour
{
    [Tooltip("The player that keeps track of what they are aiming at.")]
    public PlayerAiming player = null;
    [Tooltip("Whether or not to make the cursor invisible.")]
    public bool hideCursor = false;
    [Tooltip("Whether or not to keep the cursor in the game window.")]
    public bool confineCursor = false;


    private void Start()
    {
        Cursor.visible = !hideCursor;
        if (confineCursor)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }


    private void LateUpdate()
    {
        transform.position = new(
            player.transform.position.x + player.aimingAt.x,
            player.transform.position.y + player.aimingAt.y,
            transform.position.z
        );
    }
}
