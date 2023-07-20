using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSpellAttack : AttackBase
{
    [SerializeField] ParticleSystem SnakeParticle;
    [SerializeField] Poison PoisonPrefab;
    [SerializeField] float radius;
    public override void Cast()
    {

        parentMob.crntAnim.SetTrigger("isAttack");

    }


    public void Attack()
    {
        MobBase target = parentMob.GetTarget();
        if (target == null) return;
        Vector3 targetPos = target.transform.position;
        //ParticleSystem go = Instantiate(SnakeParticle);
        Poison poison = Instantiate(PoisonPrefab);
        poison.transform.position = transform.position + transform.forward * .2f;
        poison.PsPoisonBlast.Play();
        poison.PsPoisonBlast.transform.DOMove(targetPos, 1f).OnComplete(() =>
        {
            poison.PsPoisonBlast.Stop();
            Destroy(poison.PsPoisonBlast.gameObject);
            poison.psPoisonLand.transform.position = poison.PsPoisonBlast.transform.position;
            poison.psPoisonLand.Play();
            Destroy(poison.psPoisonLand.gameObject, 2f);
            ApplyDamage(targetPos);
        });
    }

    public void ApplyDamage(Vector3 tp)
    {
        RaycastHit[] hits = Physics.SphereCastAll(tp, radius, Vector3.up);
        MobBase target;
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.TryGetComponent<MobBase>(out target))
                {
                    target = hits[i].collider.gameObject.GetComponent<MobBase>();
                    if (target)
                    {
                        if (target.isAlive)
                        {

                            //target.AnimState = AnimStates.isTakeDamage;
                            target.HP -= parentMob.DAMAGE;
                        }
                    }

                }
            }
        }
    }
}
