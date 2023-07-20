using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceHandController : MonoBehaviour
{
    public Vector3 startPos, endPos;
    public bool repeatable = false;
    public float speed = 1.0f;
    public float duration = 3.0f;
    public float StartWaitTime = 3;
    public bool isChoose = false;
    public PogBehaviour crntPog;
    [SerializeField] PogBehaviour prewtPog;
    [SerializeField] LayerMask PogLayer;
   
    public IEnumerator Init()
    {

        yield return new WaitForSeconds(StartWaitTime);

        while (repeatable == true)
        {
            yield return RepeatLerp(startPos, endPos, duration);
            yield return RepeatLerp(endPos, startPos, duration);
        }
    }

    public void Rest()
    {
        gameObject.SetActive(false);
        transform.DOMoveX(-5f, 0.2f);

    }

    public void StopRoutine()
    {
        StopAllCoroutines();
    }

    public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.position = Vector3.Lerp(a, b, i);
            yield return null;
        }
    }


    private void Update()
    {
        if (isChoose)
            ChoosePog();
    }

    
    void ChoosePog()
    {

        RaycastHit hit = default;



        if (Physics.Raycast(transform.position, transform.forward, out hit, 1000f, PogLayer))
        {
            if (hit.transform != null)
            {
                //if(crntPog!=null)


                crntPog = hit.transform.GetComponent<PogBehaviour>();
                crntPog.transform.DOMoveY(2f, 0.2f);
                prewtPog = crntPog;
            }

            if (prewtPog != null)
                prewtPog.transform.DOMoveY(1.5f, 0.2f);

        }
    }


    public void ClosedChooseHand()
    {
        transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => gameObject.SetActive(false));
    }
}
