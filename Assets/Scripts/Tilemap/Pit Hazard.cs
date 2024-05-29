using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class PitHazard : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out Health health))
        {
            health.Kill();
        }
    }
}
