using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    Vector3 startRot;
    [SerializeField] float speed;

    private void Start()
    {
        startRot = transform.eulerAngles;
    }

    void Update()
    {
        transform.localEulerAngles += transform.up * speed * 5f * Time.deltaTime;
    }



    private void OnDisable()
    {
        transform.eulerAngles = startRot;
    }
}
