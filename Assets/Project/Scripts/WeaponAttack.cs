using UnityEngine;
using System.Collections.Generic;

public class WeaponAttack : MonoBehaviour, IAttack
{

    private Animation       animations;
    private List<Transform> characDamaged;
    private Equipement equipement;

    [HideInInspector]
    public GenericCharacter user;

    void Awake()
    {
        animations = GetComponent<Animation>();
        characDamaged = new List<Transform>();
    }

    void Start ()
    {
        equipement = GetComponent<Equipement>();
        user = GetComponent < GenericCharacter>();
    }

    public void Attack()
    {
        if (animations.isPlaying)
            return;
        if (characDamaged.Count > 0)
            characDamaged.Clear();

        animations.Play("AttackAnimation");
    }

    public bool IsAttacking()
    {
        return animations.IsPlaying("AttackAnimation");
    }

    public void DamageTargets(Transform other)
    {
        if (!IsAttacking())
            return;
        if (characDamaged.Contains(other))
            return; 
        IDamageable idamage = other.GetComponent<IDamageable>();
        if (idamage != null)
        {
            idamage.takeDamage(equipement.WeaponBaseDamage);
            characDamaged.Add(other);
        }
    }
}