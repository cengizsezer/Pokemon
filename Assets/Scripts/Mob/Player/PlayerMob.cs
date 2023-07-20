using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMob : MobBase
{
    [SerializeField] Renderer rd_indicator;
    [SerializeField] Transform pool;
    [SerializeField] Material OriginalMaterial;
    [SerializeField] Cell crntCell;
    [SerializeField] Sprite MeleeSprite;
    [SerializeField] Sprite RangeSprite;
    [SerializeField] SpriteRenderer MobClassSprite;
    [SerializeField] TMPro.TMP_Text txtDamage;

    public void DamageTextHandler(float f, Color color)
    {
        if (!txtDamage) return;
        txtDamage.transform.parent.localPosition = Vector3.zero;
        txtDamage.text = "+" + "$" + " " + (f).ToString("0");
        txtDamage.color = color;

        if (DOTween.IsTweening(txtDamage)) DOTween.Complete(txtDamage);
        txtDamage.DOFade(1f, .1f);
        txtDamage.transform.DOShakePosition(.4f);
        txtDamage.transform.parent.DOLocalMoveY(2, .1f).OnComplete(() => txtDamage.DOFade(0, 1f).SetEase(Ease.InQuad));
    }

    public override void Fight(OnFight fightEvent)
    {
        base.Fight(fightEvent);
        ClosedMobClasesSprite();
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
        s.AppendCallback(() => pokeBall.gameObject.SetActive(false));

        return s;
    }
    public void ShowMobClassesSprite()
    {

        MobClassSprite.transform.parent.gameObject.SetActive(true);

        if (!MobClassSprite.gameObject.activeInHierarchy) return;

        if (attackBase.range >= 100)
        {
            MobClassSprite.sprite = RangeSprite;
        }

        if (attackBase.range <= 100)
        {
            MobClassSprite.sprite = MeleeSprite;
        }
    }
    void ClosedMobClasesSprite()
    {
        MobClassSprite.transform.parent.gameObject.SetActive(false);
    }
    public void Goto(Cell cell, float duration = 1.2f, System.Action onComplete = null)
    {
        AnimState = AnimStates.isRun;
        rb.isKinematic = true;
        cell.mobInside = this;
        crntCell = cell;
        transform.DOMove(cell.transform.position.WithY(0.506f), duration).SetEase(Ease.Linear).OnUpdate(() =>
        {
            transform.SlowLookAt(cell.transform.position.WithY(0.506f), 8f);
        }).OnComplete(() =>
        {
            settled = true;
            AnimState = AnimStates.isIdle;
            onComplete?.Invoke();
            transform.rotation = Quaternion.identity;
            ShowMobClassesSprite();
           
        });
    }
    public void TutorialPokemonBehaviour(Cell cell)
    {
        AnimState = AnimStates.isIdle;
        rb.isKinematic = true;
        cell.mobInside = this;
        crntCell = cell;
        transform.DOMove(cell.transform.position.WithY(0.506f), 0f).SetEase(Ease.Linear).OnUpdate(() =>
        {
            transform.SlowLookAt(cell.transform.position.WithY(0.506f), 8f);
        }).OnComplete(() =>
        {
            settled = true;
            AnimState = AnimStates.isIdle;
            //onComplete?.Invoke();
            transform.rotation = Quaternion.identity;
            ShowMobClassesSprite();
            //MobManager.I.lsBorns.Add(this);
        });
    }
    public void Borned(Cell c)
    {
        Sequence s = DOTween.Sequence();
        s.AppendCallback(() => BornSequence());
        s.AppendCallback(() => Goto(c));
    }
    public Sequence BornSequence()
    {
        Sequence s = DOTween.Sequence();
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
    public void OnHold()
    {
        AnimState = AnimStates.isFall;
        pool.position += Vector3.up * 1.5f;
        rd_indicator.gameObject.SetActive(true);
        SetIndicatorColor(true);
    }
    public void SetIndicatorColor(bool canPlace)
    {
        rd_indicator.material.color = canPlace ? Color.green : Color.red;
    }
    public void DropMeToCell(Cell c)
    {
        crntCell.mobInside = null;
        crntCell = c;
        crntCell.mobInside = this;
        GoBackToPlace();
    }
    public void GoBackToPlace()
    {
        AnimState = AnimStates.isIdle;
        pool.transform.localPosition = Vector3.zero;
        transform.position = crntCell.transform.position.WithY(transform.position.y);
        rd_indicator.gameObject.SetActive(false);
    }
    public void OnMerged()
    {
        LEVEL++;
        psParticle.MergeParticlePlay();
        AnimState = AnimStates.isIdle;
        allRenderers[GetID()].material = allMaterials[GetID()];
        if (UIManager.I.GetObject().ChooseTutorial.activeInHierarchy)
        {
            UIManager.I.ShowChooseTutorial(false);
        }

    }
    public override void OnDeath()
    {
        base.OnDeath();
        RemoveMeFromList();
    }
    public override void RemoveMeFromList()
    {
        MobManager.I.RemovePlayerMobs(this);
    }
    public void RemoveMeForMerge()
    {
        crntCell.mobInside = null;
        crntCell = null;
        RemoveMeFromList();
        gameObject.SetActive(false);
    }
    public override MobBase GetClosestEnemy()
    {
        base.GetClosestEnemy();
        var lsEnemyMobs = MobManager.I.EnemyMobController.lsMobs;
        if (lsEnemyMobs.Count == 0) return null;

        EnemyMob em = null;
        float minDist = float.MaxValue;
        for (int i = 0; i < lsEnemyMobs.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, lsEnemyMobs[i].transform.position);
            if (dist < minDist)
            {
                if (lsEnemyMobs[i] != null && lsEnemyMobs[i].isAlive)
                {
                    minDist = dist;
                    em = lsEnemyMobs[i];
                }
            }

        }

        return em;
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
