using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PogBehaviour : PoolObject
{
    public Rigidbody rb;
    public Collider col;

    //public bool isAired;
    public bool isForceApplied;
    public bool isGrounded;
    public bool isFellOnTheTable;
    [SerializeField] ParticleSystem psPogParticle;
    public Rigidbody GetRB() => rb;

    int _level = 0;
    public int LEVEL
    {
        get => _level;

        set
        {
            _level = value;
            ShowActivePog();
        }
    }


    public PokemonTypes _pogType;
    public PokemonTypes PogType
    {
        get => _pogType;

        set
        {
            _pogType = value;

            ShowActivePog();
        }
    }

    [SerializeField] GameObject[] pogObjs;

    void ShowActivePog()
    {

        int _id = GetID();

        for (int i = 0; i < pogObjs.Length; i++)
        {
            pogObjs[i].SetActive(i == _id);
        }
    }

    public int GetID() => (int)PogType * 3 + LEVEL;

    public void ParticlePlay()
    {
        psPogParticle.Play();
    }

    public void ParticleStop()
    {
        psPogParticle.Stop();
    }
    #region POOLING FUNCTION
    public override void OnCreated()
    {
        //transform.SetParent(null);
        OnDeactivate();
    }

    public override void OnDeactivate()
    {
        PoolHandler.I.EnqueObject(this);
        //transform.SetParent(null);
        gameObject.SetActive(false);
    }

    public override void OnSpawn()
    {
        gameObject.SetActive(true);
    }

    #endregion

    public void Init(int id)
    {
        int level = id % 3;
        int typeID = (id - level) / 3;

        LEVEL = level;

        PogType = (PokemonTypes)typeID;
    }

    public void AddForce(float force, Vector3 dir, float radius)
    {
        // Vector3 forceV = (dir + Vector3.up * 10f).normalized * force;

        rb.AddExplosionForce(force * 2, dir, radius, 1f, ForceMode.Impulse);

        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
        //isAired = true;
        isForceApplied = true;
        //isGrounded = false;
    }


    private void OnCollisionEnter(Collision col)
    {

        if (col.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (col.collider.CompareTag("Ground") && isForceApplied)
        {
            isFellOnTheTable = true;
        }

    }


    private void OnCollisionStay(Collision collision)
    {
        bool on = true;
        bool isfall = true;
        if (collision.collider.CompareTag("Pog") && on && isForceApplied)
        {

            RaycastHit[] hits = Physics.SphereCastAll(transform.position, 3f, Vector3.up);
            if (hits.Length > 0)
            {

                for (int i = 0; i < hits.Length; i++)
                {
                    PogBehaviour pb = hits[i].collider.gameObject.GetComponent<PogBehaviour>();

                    if (pb != null && pb.isGrounded)
                    {
                        isGrounded = true;
                        isFellOnTheTable = true;
                        on = false;
                    }
                }

            }
        }

        if (collision.collider.CompareTag("Ground") && isfall && isForceApplied)
        {

            RaycastHit[] hits = Physics.SphereCastAll(transform.position, 3f, Vector3.up);
            if (hits.Length > 0)
            {

                for (int i = 0; i < hits.Length; i++)
                {
                    PogBehaviour pb = hits[i].collider.gameObject.GetComponent<PogBehaviour>();

                    if (pb != null && pb.isGrounded)
                    {

                        isFellOnTheTable = true;
                        isfall = false;
                    }
                }

            }
        }
    }

    public Sequence ExtraPogBehaviour(float behaviourTime)
    {


        Sequence s = DOTween.Sequence();
        s.AppendInterval(0.3f);
        s.Append(transform.DORotate(new Vector3(150, 0f, 0f), 0.2f));
        s.Append(transform.DOMove(new Vector3(0f, 5.85f, -3.7f), behaviourTime));
        s.Join(transform.DOLocalRotate(new Vector3(0, 0, 360 * 2), behaviourTime, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuart));
        s.AppendCallback(() => rb.isKinematic = true);
        s.AppendCallback(() => col.enabled = false);
        s.AppendInterval(0.3f);
        //s.Append(transform.DOMove(DeckManager.I.lsDeckPogs[DeckManager.I.lsDeckPogs.Count-1].transform.position,0.5f));
        s.Append(transform.DOMove(new Vector3(0.02f, 0.55f, 0.2f), 0.5f));
        s.Join(transform.DORotate(Vector3.zero, 0.2f));
        s.AppendCallback(() => Land());
        return s;
    }

    public void Land()
    {
        rb.isKinematic = false;
        col.enabled = true;
        //isAired = false;
        isForceApplied = false;
        isGrounded = true;
        DeckManager.I.lsDeckPogs.Add(this);
        SaveLoadManager.AddToDeck(this.GetID());
        //SaveLoadManager.AddToExtraPogs(this.GetID());
        DeckManager.I.ExtraPogLand();
        InputManager.I.Initialize(InputManager.TouchTypes.HandSwipe);
    }


}
