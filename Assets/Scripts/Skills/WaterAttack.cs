using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAttack : AttackBase
{

    [SerializeField] Water WaterPrefab;
    [SerializeField] ParticleSystem SwrilPrefab;
    public override void Cast()
    {
        parentMob.crntAnim.SetTrigger("isAttack");
        SwrilPrefab.Play();

    }

    public void Attack()
    {
        MobBase target = parentMob.GetTarget();
        if (target == null) return;
        Vector3 targetPos = target.transform.position;
        //ParticleSystem go = Instantiate(SnakeParticle);
        Water water = Instantiate(WaterPrefab);

        water.transform.position = targetPos.WithY(5f);
        water.owner = this;
        water.StartRoutine(targetPos);
    }
}
