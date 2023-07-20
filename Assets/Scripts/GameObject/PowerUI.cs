using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUI : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] Transform pin;

    float crnt_rotation;
    [SerializeField] float rotSpeed;
    [SerializeField] float rotLimit;

    float _dir = 1f;

    [SerializeField]
    bool isActive;

    private void Update()
    {
        if (isActive)
        {
            RotatePin();
        }
    }

    void RotatePin()
    {
        crnt_rotation += Time.deltaTime * rotSpeed * _dir;

        if (Mathf.Abs(crnt_rotation) >= rotLimit)
        {
            crnt_rotation = rotLimit * _dir;
            _dir *= -1f;
        }

        var rot = Quaternion.Euler(Vector3.forward * crnt_rotation);
        pin.rotation = rot;
    }



    public void SetActive(bool isActive)
    {
        obj.SetActive(isActive);
        if (isActive)
            crnt_rotation = Random.Range(-40f, 40f);

        this.isActive = isActive;
    }

    public float GetPower()
    {
        isActive = false;

        float f = (rotLimit - Mathf.Abs(crnt_rotation)) / rotLimit;
        return f * 10f;
    }
}
