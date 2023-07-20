using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void LateUpdate()
    {
        //transform.LookAt(currentCam.transform);
        transform.LookAt(CameraController.I.currentCamera.transform);
    }
}
