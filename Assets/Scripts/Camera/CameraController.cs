using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    private const int ACTIVE_VALUE = 10;
    private const int PASSIVE_VALUE = 1;

    public enum CameraStatus
    {
        StartCamera,
        FightCamera,
        None
    }

    [System.Serializable]
    public class VirtualCamera
    {
        public CameraStatus CameraStatus;
        public CinemachineVirtualCamera Camera;
    }

    public CinemachineVirtualCamera currentCamera;
    public VirtualCamera[] VirtualCameras;

    [HideInInspector] public CinemachineBrain Brain;

    public CameraStatus CurrentStatus { get; private set; }

    public Camera MainCamera { get; private set; }

    void Start()
    {
        CurrentStatus = CameraStatus.StartCamera;
        SetCameraStatus(CurrentStatus);

        MainCamera = Camera.main;
        Brain = MainCamera.GetComponent<CinemachineBrain>();
        EventManager.Add<OnStartGame>(OnGameStarted);
        EventManager.Add<OnFight>(OnGameFighted);

    }

    private void OnDisable()
    {
        EventManager.Remove<OnStartGame>(OnGameStarted);
        EventManager.Remove<OnFight>(OnGameFighted);
    }

    public void OnGameStarted(OnStartGame startGameEvent)
    {
        SetCameraStatus(CameraStatus.StartCamera);
    }

    public void OnGameFighted(OnFight fightEvent)
    {
        SetCameraStatus(CameraStatus.FightCamera);
    }

    public void OnWarBoard()
    {
        SetCameraStatus(CameraStatus.FightCamera);
    }

    public void SetCameraStatus(CameraStatus status)
    {
        foreach (var virtualCamera in VirtualCameras)
        {
            if (virtualCamera.CameraStatus == status)
            {
                virtualCamera.Camera.Priority = ACTIVE_VALUE;
                currentCamera = virtualCamera.Camera;
            }
            else
            {
                virtualCamera.Camera.Priority = PASSIVE_VALUE;
            }
        }

        CurrentStatus = status;

    }

    public void ShakeCamera(float Str)
    {
        transform.DOComplete();
        transform.DOShakePosition(.3f, Str);
    }
}
