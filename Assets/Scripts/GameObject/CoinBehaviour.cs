using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : PoolObject
{
    [SerializeField]private RectTransform rect;
    private void Rest()
    {
        Vector3 targetPos = new Vector3(Random.Range(-90f, 90), Random.Range(-80, 100f), 0f);// + midPOs
        rect.localPosition = targetPos;
        rect.localScale = Vector3.zero;
        gameObject.SetActive(true);
    }
    private Sequence SpawnSequence()
    {
        float time = Random.Range(0.12f, 0.25f);
        float delay = Random.Range(0.1f, 0.2f);



        Sequence s = DOTween.Sequence();
        s.AppendCallback(() => gameObject.transform.SetParent(UIManager.I.GetObject()._canvas.transform, false));
        s.AppendCallback(() => Rest());
        s.Append(rect.DOScale(1f, time));
        s.AppendInterval(.3f);
        s.Append(rect.DOMove(UIManager.I.GetObject().coinPoolParent.position, time * 2f));
        s.AppendCallback(() =>
        {
            gameObject.transform.SetParent(UIManager.I.GetObject().coinPoolParent);

        });
        s.Join(rect.DOLocalMove(Vector3.zero, time).OnComplete(() => {
            OnDeactivate();

        }));



        return s;
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
        SpawnSequence();
    }

    #endregion
}
