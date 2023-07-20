using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArrowController : MonoBehaviour
{
    [SerializeField] Vector3 StartPos;
    [SerializeField] Vector3 EndPos;
    public bool Repeatable;
    [SerializeField] float speed;
    [SerializeField] float duration;
    [SerializeField] WaitForSeconds waitForSeconds;

    private IEnumerator Start()
    {
        StartPos = transform.position;
        yield return waitForSeconds;

        while (Repeatable)
        {
            yield return RepeatLerp(StartPos, EndPos, duration);
            yield return RepeatLerp(EndPos, StartPos, duration);
        }
    }

    public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.position = Vector3.Lerp(a, b, i);
            transform.eulerAngles = Vector3.up * i * 100 * 2f;
            yield return null;
        }
    }
}
