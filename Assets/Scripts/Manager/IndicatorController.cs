using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorController : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Transform hand;
    [SerializeField] float xOffset, zOffset, yOffset;
    public void IndicatorRoutine()
    {
        StartCoroutine(ClosedIndicator());
    }

    private void Update()
    {
        transform.position = new Vector3
            (

            hand.transform.position.x + xOffset,
            hand.transform.position.y + yOffset,
            hand.transform.position.z + zOffset

            );
    }

    public IEnumerator ClosedIndicator()
    {
        yield return sr.DOFade(0f, .4f).SetEase(Ease.InQuad).WaitForCompletion();
        gameObject.SetActive(false);

    }


    public void OnReset()
    {
        Color color = sr.color;
        sr.color = new Color(color.r, color.g, color.b, 0.3960784f);
        gameObject.SetActive(true);


    }
}
