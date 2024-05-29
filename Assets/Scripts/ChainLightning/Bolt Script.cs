using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class BoltScript : MonoBehaviour
{
    public int damage = 3;
    public LayerMask enemies;


    private float timeSinceLastDamaged = -1f;


    private void OnTriggerStay2D(Collider2D collider)
    {
        Debug.Log(name + " Hit: " + collider.name);
        if (collider.gameObject == transform.parent.gameObject)
        {
            return;
        }
        int layer = collider.gameObject.layer;
        // Enemy
        if (IsLayerInMask(enemies, layer) && collider.TryGetComponent(out Health enemy))
        {
            enemy.Damage(damage, ref timeSinceLastDamaged);
        }
        // Pylon
        else if (collider.TryGetComponent(out PylonScript pylon))
        {
            pylon.ActivateEffect(name);
        }
        // Landmine
        else if (collider.TryGetComponent(out Landmine landmine))
        {
            landmine.Explode();
        }
    }


    // https://forum.unity.com/threads/checking-if-a-layer-is-in-a-layer-mask.1190230/
    private bool IsLayerInMask(LayerMask mask, int layer)
    {
        return (mask & (1 << layer)) != 0;
    }
}
