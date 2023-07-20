using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMob : MobBase
{

    public override MobBase GetClosestEnemy()
    {
        base.GetClosestEnemy();
        var lsPlayerMobs = MobManager.I.PlayerMobController.lsMobs;

        if (lsPlayerMobs.Count == 0) return null;

        PlayerMob pm = null;
        float minDist = float.MaxValue;
        for (int i = 0; i < lsPlayerMobs.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, lsPlayerMobs[i].transform.position);
            if (dist < minDist)
            {
                if (lsPlayerMobs[i] != null && lsPlayerMobs[i].isAlive)
                {
                    minDist = dist;
                    pm = lsPlayerMobs[i];
                }
            }

        }

        return pm;
    }
    public override Sequence DieSequence()
    {
        Sequence s = DOTween.Sequence();
        s.AppendCallback(() => allRenderers[GetID()].material = ShaderMaterial);
        s.AppendCallback(() => ShaderMaterial.SetFloat("_Dissolve", .5f));
        s.AppendCallback(() => allRenderers[GetID()].material.DOFloat(1f, "_Dissolve", 1f));
        s.AppendCallback(() => pokeBall.gameObject.SetActive(true));
        s.AppendCallback(() => pokeBall.SetParent(null));
        s.AppendCallback(() => pokeBall.transform.position = this.transform.position);
        //s.Append(pokeBall.DOMove(MobManager.I.EnemyPokeballPosition.position, 1f));
        s.AppendCallback(() => pokeBall.gameObject.SetActive(false));

        return s;
    }

    public void SetProps(Level.LevelEnemies infos)
    {
        LEVEL = infos.LEVEL;
        MOBTYPE = infos.MOBTYPE;
        rb.isKinematic = true;
        transform.rotation = Quaternion.Euler(Vector3.up * 180f);
        //CONFIG HP
        //CONFIG DMG
        transform.localScale *= infos.scale;
        DAMAGE = DAMAGE * infos.dmgMult;
        HP = HP * infos.hpMult;
        transform.position = MobManager.I.GetEnemyPosition(infos.posID).position;
        DeActiveted();
    }

    public override void OnDeath()
    {
        base.OnDeath();
        MobManager.I.RemoveEnemyMobs(this);
    }

    void DeActiveted()
    {
        gameObject.SetActive(false);
    }

    void Activeted()
    {
        gameObject.SetActive(true);
    }

    public Sequence BornSequence()
    {
        Sequence s = DOTween.Sequence();

        s.AppendInterval(1f);
        s.AppendCallback(() => Activeted());
        s.AppendCallback(() => rb.isKinematic = true);
        s.AppendCallback(() => SetDissolveValue(GetID()));
        s.AppendInterval(1f);
        s.AppendCallback(() => SetOriginalMaterial(GetID()));
        return s;
    }


    void SetDissolveValue(int id)
    {
        allRenderers[id].material.DOFloat(0.2f, "_Dissolve", 3f);

    }

    void SetOriginalMaterial(int id)
    {
        allRenderers[id].material = allMaterials[id];
    }

    public override void OnDeactivate()
    {
        throw new System.NotImplementedException();
    }

    public override void OnSpawn()
    {
        throw new System.NotImplementedException();
    }

    public override void OnCreated()
    {
        throw new System.NotImplementedException();
    }
}
