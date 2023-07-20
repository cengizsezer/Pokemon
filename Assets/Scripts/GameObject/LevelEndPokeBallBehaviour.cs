using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndPokeBallBehaviour : MonoBehaviour
{
    public float openAngle;
    public float openDuration;
    [SerializeField] Transform TargetPos;
    [SerializeField] ParticleSystem[] OpenedParticle;
    [SerializeField] Transform Pivot1;
    [SerializeField] Transform Pivot2;
    bool isEnd;

    public void ThrowPokeball(Vector3[] path)
    {

        Sequence throwSequence = DOTween.Sequence();

        throwSequence.Append(transform.DOPath(path, 1f));
        throwSequence.Join(transform.DORotate(new Vector3(0f, 0f, 0f), openDuration / 3).SetEase(Ease.OutBack));
        throwSequence.Append(transform.DOShakeScale(1f));
        throwSequence.Append(Pivot1.DOLocalRotate(new Vector3(0, 0, -openAngle), openDuration).SetEase(Ease.OutBack));
        throwSequence.AppendCallback(() => {

            foreach (var item in OpenedParticle)
            {
                item.Play();
            }
        });
      
        throwSequence.AppendInterval(1f);
        throwSequence.AppendCallback(() => isEnd = true);

    }

    public bool HasLanding()
    {
        return isEnd ? true : false;
    }
}
