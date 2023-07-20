using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PokemonAttackType
{
    None,
    Range,
    Melee
}
public abstract class AttackBase : MonoBehaviour
{
    public MobBase parentMob;
    public PokemonAttackType pokemonAttackType;
    public float range;

    public bool IsInRange(Transform target)
    {
        if (target == null) return false;

        float d = Vector3.Distance(parentMob.transform.position, target.position);

        return (d <= range);
    }

    public abstract void Cast();
}
