using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public delegate void DamageTagetsDel(Transform target);
    public event DamageTagetsDel damageTargetsDel;

    private WeaponAttack attackComponent;
    [HideInInspector] public float    range;

    void Start()
    {
        attackComponent = transform.root.GetComponentInChildren<WeaponAttack>();
        damageTargetsDel += attackComponent.DamageTargets;
        range = attackComponent.user.GetComponent<Equipement>().WeaponBaseRange;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other as CapsuleCollider)
        {
            if (attackComponent != null)
            {
                if (attackComponent.IsAttacking())
                {
                    damageTargetsDel(other.transform);
                }
            }
        }
    }
}
