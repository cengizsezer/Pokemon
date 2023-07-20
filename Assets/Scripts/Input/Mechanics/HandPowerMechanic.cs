using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPowerMechanic : TouchBase
{
    [SerializeField] PowerUI powerUI;
    [SerializeField] Transform hand;
    [SerializeField] ParticleSystem psHandHitDust;
    [SerializeField] IndicatorController indicator;
    public override void OnDown()
    {
        UIManager.I.ShowExtraPogButton(false);

        if (LevelManager.I.IsFirstLevel())
            UIManager.I.ShowTapTutorial(false);

        float power = powerUI.GetPower();
        InputManager.I.Initialize(InputManager.TouchTypes.NONE);
        HitTheTable(power);
    }

    public override void OnDrag()
    {

    }

    public override void OnUp()
    {

    }

    public override void OnInitialized()
    {
        powerUI.SetActive(true);
        UIManager.I.ShowExtraPogButton(false);
        DeckManager.I.TutorialArea.SetActive(false);
        DeckManager.I.TutorialArrow.SetActive(false);
    }

    void HitTheTable(float power)
    {

        StartCoroutine(indicator.ClosedIndicator());
        hand.DOMoveY(.65f, .4f).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            psHandHitDust.Play();
            ApplyForceThePogs(power);
            Vibrator.Haptic(MoreMountains.NiceVibrations.HapticTypes.MediumImpact);

        });

        new DelayedAction(() => HideElements(), 1f).Execute(this);
    }

    void HideElements()
    {
        hand.gameObject.SetActive(false);
        hand.transform.position = new Vector3(-4.382f, 1.89f, -2.51f);
        hand.transform.eulerAngles = new Vector3(-97.289f, 66.346f, -227.011f);
        powerUI.SetActive(false);
    }

    float radius = 3f;
    float maxForce = 1f;

    void ApplyForceThePogs(float pow)
    {
        RaycastHit[] hits = Physics.SphereCastAll(hand.transform.position, radius, Vector3.up);
        List<PogBehaviour> lsAiredPos = new List<PogBehaviour>();

        //float power = pow <= 0.1f ? .3f : pow;
        float power = Mathf.Clamp(pow, 2f, 7f);

        if (LevelManager.I.IsFirstLevel())
        {
            power = 6f;
        }

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.CompareTag("Pog"))
                {
                    PogBehaviour pb = hits[i].collider.gameObject.GetComponent<PogBehaviour>();


                    if (pb != null)
                    {
                        lsAiredPos.Add(pb);
                        float dist = Vector3.Distance(pb.transform.position, hand.position);


                        float totalForce = (dist / 3f) * power * maxForce;
                        Vector3 direction = (pb.transform.position - hand.position).WithY(0);

                        pb.AddForce(totalForce, hand.position, radius);

                    }
                }
            }

            CameraController.I.ShakeCamera(pow / 10f);
            DeckManager.I.WaitForLanding(lsAiredPos);
        }



    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(hand.position, radius);
    //}

}
