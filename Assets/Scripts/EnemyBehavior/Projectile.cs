using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    public int damage = 1;
    public LayerMask damageLayers;
    public LayerMask wallLayers;


    private void OnTriggerEnter2D(Collider2D collider)
    {
        int layer = collider.gameObject.layer;
        if (IsLayerInMask(damageLayers, layer) && collider.TryGetComponent(out Health health))
        {
            health.Damage(damage);
            Destroy(gameObject);
        }
        else if (IsLayerInMask(wallLayers, layer))
        {
            Destroy(gameObject);
        }
    }


    // https://forum.unity.com/threads/checking-if-a-layer-is-in-a-layer-mask.1190230/
    private bool IsLayerInMask(LayerMask mask, int layer)
    {
        return (mask & (1 << layer)) != 0;
    }
}
