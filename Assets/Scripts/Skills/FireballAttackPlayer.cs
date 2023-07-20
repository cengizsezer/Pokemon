using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAttackPlayer : AttackBase
{
    [SerializeField] ParticleSystem psPrefab;
    [SerializeField] ParticleSystem SwrilPrefab;
    [SerializeField] float radius;
    [SerializeField] float OffSet;
    [SerializeField] ParticleSystem Impact;

    public override void Cast()
    {
        parentMob.crntAnim.SetTrigger("isAttack");
        SwrilPrefab.transform.localPosition = this.transform.localPosition + Vector3.up * OffSet;
        SwrilPrefab.Play();

    }
    public void Attack()
    {
        MobBase target = parentMob.GetTarget();
        if (target == null) return;
        Vector3 targetPos = target.transform.position;

        ParticleSystem ps = Instantiate(psPrefab);

        //parentMob.crntAnim.SetTrigger("isAttack");

        Vector3 mid = transform.GetMiddlePosition(targetPos) + Vector3.up * 5f;
        Vector3[] path = transform.GetQuadraticPath(mid, targetPos, 20);

        ps.transform.position = transform.position + transform.forward * .2f;

        ps.transform.DOPath(path, 1f).OnComplete(() =>
        {
            if (Impact != null)
            {
                ParticleSystem ımpactObj = Instantiate(Impact);
                ımpactObj.transform.position = ps.transform.position;
                ımpactObj.Play();
            }
            ApplyDamage(targetPos);
            ps.Stop();
            Destroy(ps.gameObject, 1f);
        });


    }

    public void ApplyDamage(Vector3 tp)
    {
        if (!GameManager.I.isRunning) return;
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
                            if (parentMob is PlayerMob)
                            {
                                PlayerMob Player = parentMob as PlayerMob;

                                if (Player != null)
                                {
                                    Player.DamageTextHandler(15, Color.white);
                                    SaveLoadManager.AddCoin(15);
                                    Vibrator.Haptic(MoreMountains.NiceVibrations.HapticTypes.HeavyImpact);
                                }
                            }
                            //target.AnimState = AnimStates.isTakeDamage;
                            target.HP -= parentMob.DAMAGE;
                        }
                    }
                }
            }
        }
    }
}
