using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawAttack : AttackBase
{
    [SerializeField] ParticleSystem psAttack;
    public override void Cast()
    {
        parentMob.crntAnim.SetTrigger("isAttack");
    }


    public void Attack()
    {

        MobBase target = parentMob.GetTarget();
        if (target == null) return;
        if (!parentMob.IsInRange()) return;
        if (!GameManager.I.isRunning) return;

        if (psAttack)
            psAttack.Play();

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
        target.HP -= parentMob.DAMAGE;

    }


}
