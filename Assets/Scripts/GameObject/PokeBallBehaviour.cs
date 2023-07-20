using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeBallBehaviour : MonoBehaviour
{

    public float throwArc;
    public float throwDuration;
    public float jumpPower = .5f;
    public float jumpDuration;
    public float openAngle;
    public float openDuration;
    public float fallDuration = .6f;
    [SerializeField] ParticleSystem[] OpenedParticle;

    public void ThrowPokeball(Transform targetPos)
    {
        Vector3 jumpPosition = targetPos.position + Vector3.up * 30f;

        Sequence throwSequence = DOTween.Sequence();

        throwSequence.Append(transform.DOJump(targetPos.position, throwArc, 1, throwDuration));
        throwSequence.Join(transform.DORotate(new Vector3(300, 0, 0), throwDuration, RotateMode.FastBeyond360));
        throwSequence.Append(transform.DOJump(jumpPosition, jumpPower, 1, jumpDuration));
        throwSequence.Join(transform.DOLookAt(targetPos.position, jumpDuration));



        throwSequence.Append(transform.GetChild(0).GetChild(0).DOLocalRotate(new Vector3(-openAngle, 0, 0), openDuration).SetEase(Ease.OutBack));
        throwSequence.Join(transform.GetChild(0).GetChild(1).DOLocalRotate(new Vector3(openAngle, 0, 0), openDuration).SetEase(Ease.OutBack));
        throwSequence.AppendCallback(() => {

            foreach (var item in OpenedParticle)
            {
                item.Play();
            }
        });


        throwSequence.Append(transform.GetChild(0).GetChild(0).DOLocalRotate(Vector3.zero, openDuration / 3));
        throwSequence.Join(transform.GetChild(0).GetChild(1).DOLocalRotate(Vector3.zero, openDuration / 3));

        throwSequence.Join(transform.DORotate(new Vector3(0f, 180f, 0f), openDuration / 3).SetEase(Ease.OutBack));

        throwSequence.AppendInterval(.3f);

        throwSequence.AppendCallback(() => gameObject.SetActive(false));


    }


}
