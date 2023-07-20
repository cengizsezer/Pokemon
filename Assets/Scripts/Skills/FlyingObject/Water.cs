using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public SpriteRenderer circleIndicator;
    public ParticleSystem psCore;
    public ParticleSystem psImpact;
    public AttackBase owner;

    public void StartRoutine(Vector3 targetPos)
    {
        StartCoroutine(MeteorHandler(targetPos));
    }

    public IEnumerator MeteorHandler(Vector3 targetPos)
    {
        if (owner.parentMob.GetTarget() == null) yield break;
        //yield return circleIndicator.DOFade(0f, 1f).SetEase(Ease.InQuad).WaitForCompletion();
        psCore.transform.localPosition = Vector3.zero;
        psCore.Play();
        yield return psCore.transform.DOMoveY(-2.5f, 1f).SetEase(Ease.Linear).WaitForCompletion();
        Destroy(psCore.gameObject);
        if (owner.parentMob.GetTarget() == null) yield break;
        ApplyDamage(owner.parentMob.GetTarget().transform.position);
        psImpact.transform.position = circleIndicator.transform.position;
        psImpact.Play();
        yield return new WaitForSeconds(0.5f);
        circleIndicator.gameObject.SetActive(false);
        gameObject.SetActive(false);

    }

    public void ApplyDamage(Vector3 tp)
    {
        if (!GameManager.I.isRunning) return;
        if (owner.parentMob.GetTarget() == null) return;
        RaycastHit[] hits = Physics.SphereCastAll(tp, 0.1f, Vector3.up);
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

                            target.HP -= owner.parentMob.DAMAGE;

                            if (owner.parentMob is PlayerMob)
                            {
                                PlayerMob Player = owner.parentMob as PlayerMob;

                                if (Player != null)
                                {
                                    Player.DamageTextHandler(15, Color.white);
                                    SaveLoadManager.AddCoin(15);
                                    Vibrator.Haptic(MoreMountains.NiceVibrations.HapticTypes.HeavyImpact);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
