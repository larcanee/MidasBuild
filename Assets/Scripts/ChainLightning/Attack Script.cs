using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(Stamina))]
public class AttackScript : MonoBehaviour
{
    [Header("Objects and Animation")]
    public GameObject lightning_bolt;
    public GameObject lightning_aoe;
    public Animator midasAnimator;

    [Header("Stamina")]
    public float boltStaminaPerSecond = 40f;
    public float aoeStaminaPerSecond = 40f;

    [Header("Controls")]
    public List<KeyCode> boltKeys = new() { KeyCode.Z, KeyCode.Mouse0 };
    public List<KeyCode> aoeKeys = new() { KeyCode.X, KeyCode.Mouse1 };


    private Stamina stamina = null;
    private float boltTime = -1f;
    private float aoeTime = -1f;


    private void Start()
    {
        stamina = GetComponent<Stamina>();
    }

    private void Update()
    {
        bool isUsingBolt = boltKeys.Any(key => Input.GetKey(key));
        bool isUsingAoe = aoeKeys.Any(key => Input.GetKey(key));

        // Bolt attack.
        if (isUsingBolt && stamina.UseStamina(boltStaminaPerSecond, ref boltTime))
        {
            Attack(true, false);
        }
        // AOE attack.
        else if (isUsingAoe && stamina.UseStamina(aoeStaminaPerSecond, ref aoeTime))
        {
            Attack(false, true);
        }
        // Not attacking or not able to attack.
        else
        {
            // If trying to attack, but not able, delay stamina regeneration.
            if (isUsingBolt || isUsingAoe)
            {
                stamina.DelayRegeneration();
            }
            Attack(false, false);
        }
    }


    private void Attack(bool boltAttack, bool aoeAttack)
    {
        lightning_bolt.SetActive(boltAttack);
        lightning_aoe.SetActive(aoeAttack);
        midasAnimator.SetBool("isAttacking", boltAttack || aoeAttack);
    }
}
