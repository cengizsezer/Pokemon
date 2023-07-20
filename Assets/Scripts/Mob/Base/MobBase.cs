using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public enum AnimStates
{
    isIdle,
    isRun,
    isDie,
    isFall,
    isAttack,
    isTakeDamage
}

public enum PokemonTypes
{
    None=-1,
    Water1 = 0,
    Water2 = 1,
    Fire1 = 2,
    Fire2 = 3,
    Grass1 = 4,
    Grass2 = 5,
    Physic1 = 6,
    Physic2 = 7
}
public abstract class MobBase : PoolObject
{
    public delegate void OnUpdate();
    public OnUpdate onUpdate;

    public MobBase targetMob;
    public bool settled;


    public AttackBase attackBase;
    public ParticleHandler psParticle;
    public Transform pokeBall;
    public Material ShaderMaterial;

    AnimStates _animState = AnimStates.isIdle;
   
    public AnimStates AnimState
    {
        get => _animState;

        set
        {
            if (_animState != value)
            {
                _animState = value;
                if (crntAnim != null)
                {
                    crntAnim.SetTrigger(_animState.ToString());
                }

            }
        }
    }

    PokemonTypes mobType;
    public PokemonTypes MOBTYPE
    {
        get => mobType;

        set
        {
            mobType = value;
            ShowActiveMob();
        }
    }

    int Level = 0;
    public int LEVEL
    {
        get => Level;
        set
        {
            Level = value;
            ShowActiveMob();
        }
    }

    [SerializeField] float hp;
    public float HP
    {
        get => hp;

        set
        {
            hp = value;

            if (hp <= 0)
            {
                hp = 0f;
                if (isAlive)
                {
                    OnDeath();
                }
            }
        }
    }


    public Rigidbody rb;
    public CapsuleCollider col;

    [HideInInspector] public Animator crntAnim;
    [SerializeField] GameObject crntMesh;
    [SerializeField] Animator[] allMobs;
    public Renderer[] allRenderers;
    public Material[] allMaterials;
    public bool isAlive = true;
    public float DAMAGE;

    [SerializeField] float attackTimer, attackInterval;
    [SerializeField] public bool isFight = false;

    #region GETTER
    public int GetID() => (int)MOBTYPE * 3 + Level;
    public MobBase GetTarget() => targetMob;
    #endregion

    #region SETTER

    #endregion

    private void Start()
    {

        OnStart();

    }

    private void OnDisable()
    {
        _Disable();

    }

    public void OnResetFight()
    {
        targetMob = null;
        ResetAnimationStates();
        AnimState = AnimStates.isIdle;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        col.enabled = false;
        //transform.DOScale(Vector3.zero, 0.2f);
    }

    public virtual void OnStart()
    {
        EventManager.Add<OnFight>(Fight);
    }

    public virtual void _Disable()
    {
        EventManager.Remove<OnFight>(Fight);
    }

    public virtual void Fight(OnFight fightEvent)
    {
        isFight = true;
        rb.isKinematic = false;

        if (attackBase.range > 10f)
        {
            col.isTrigger = true;
        }

        StartFight();
        InputManager.I.Initialize(InputManager.TouchTypes.NONE);
    }

    public void ShowActiveMob()
    {

        int _id = GetID();

        HP = Configs.Player.HPs[_id];
        DAMAGE = Configs.Player.damages[_id];
        attackInterval = Configs.Player.attackInterval[_id];
        isAlive = true;

        crntAnim = allMobs[_id];
        //ResetAnimationStates();
        //AnimState = AnimStates.isIdle;

        for (int i = 0; i < allMobs.Length; i++)
        {
            allMobs[i].gameObject.SetActive(i == _id);
            crntMesh = allMobs[i].gameObject;
        }
        SetAttackBase();
        MobShake();

    }
    void MobShake()
    {
        allMobs[GetID()].transform.localPosition = Vector3.Lerp(Vector3.down * -1, new Vector3(0, 0, 0), 1f);
        allMobs[GetID()].transform.DOComplete();
        allMobs[GetID()].transform.DOShakeScale(.3f, .5f, 10, 90, true);
    }

    void SetAttackBase()
    {
        attackBase = allMobs[GetID()].GetComponent<AttackBase>();
    }

    public virtual void OnDeath()
    {
        if (!isAlive) return;
        col.enabled = false;
        isAlive = false;
        AnimState = AnimStates.isDie;
        DieSequence();
    }

    public virtual Sequence DieSequence()
    {
        return null;
    }

    public virtual MobBase GetClosestEnemy()
    {

        return null;
    }

    private void ResetAllTriggers()
    {
        if (crntAnim.Equals(null)) return;

        foreach (var parametre in crntAnim.parameters)
        {
            if (parametre.type == AnimatorControllerParameterType.Trigger)
            {
                crntAnim.ResetTrigger(parametre.name);
            }
        }
    }
    public void ResetAnimationStates()
    {
        ResetAllTriggers();
    }
    public virtual void RemoveMeFromList() { }
    public void StopMovement()
    {
        ResetAnimationStates();
        AnimState = AnimStates.isIdle;
        rb.isKinematic = true;
    }
    public virtual void StartFight()
    {
        onUpdate = null;

        ResetAnimationStates();

        MobBase mb = GetClosestEnemy();

        if (!isAlive) return;

        if (!GameManager.I.isRunning)
        {
            StopMovement();
            return;
        }

        if (mb == null)
        {
            StartFight();


            return;
        }

        if (!mb.isAlive)
        {

            mb.RemoveMeFromList();
            StopMovement();
            StartFight();
            return;
        }


        targetMob = mb;
        rb.isKinematic = false;
        onUpdate = FollowTarget;
    }
    private void Update()
    {
        _OnUpdate();
    }
    public virtual void _OnUpdate()
    {
        if (GameManager.I.isRunning && isFight)
        {

            onUpdate?.Invoke();
        }
    }
    void AttackRoutine()
    {
        if (!GameManager.I.isRunning)
        {
            StopMovement();
            onUpdate = null;
            return;
            //isrunning
        }


        if (targetMob == null)
        {

            onUpdate = null;
            StartFight();
            return;
        }
        if (!isAlive)
        {

            onUpdate = null;
            return;
            //hepsi canlı
        }
        if (!targetMob.isAlive)
        {

            onUpdate = null;
            targetMob = null;
            StartFight();
            return;

        }

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackInterval)
        {

            attackTimer = 0f;
            attackBase.Cast();
        }

    }
    void FollowTarget()
    {
        if (!GameManager.I.isRunning)
        {
            StopMovement();
            onUpdate = null;
            return;
        }

        if (!targetMob.isAlive)
        {
            targetMob = null;
            onUpdate = null;
            StartFight();
            return;
        }

        if (targetMob == null)
        {
            onUpdate = null;
            StartFight();
            return;
        }

        if (!isAlive)
        {
            onUpdate = null;
            return;
        }



        if (IsInRange())
        {

            StopMovement();
            attackTimer = attackInterval / 1.2f;
            onUpdate = AttackRoutine;
            return;
        }

        transform.SlowLookAt(targetMob.transform.position, 8f);

        //rb.velocity = transform.forward * Configs.Player.speed;
        rb.velocity = transform.forward * 1f;
        AnimState = AnimStates.isRun;

    }
    public bool IsInRange()
    {
        return attackBase.IsInRange(targetMob.transform);
    }
}
