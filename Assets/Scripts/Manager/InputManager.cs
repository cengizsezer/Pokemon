using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] TouchBase[] touchMechanics;

    [SerializeField]TouchBase activeTouch = null;

    public enum TouchTypes
    {
        NONE = -1,
        HandSwipe = 0,
        PowerBar = 1,
        Merge = 2,
        Choose = 3
        //.....so on
    }

    private bool isDragging = false;
    private bool canPlay = false;

    private Vector3 fp, lp, dif;

    public bool IsActive() => GameManager.I.isRunning && canPlay && activeTouch != null;
    public void Enable(bool isActive) => canPlay = isActive;

    private void Update()
    {
        if (IsActive())
            HandleTouch();
    }

    public void OnGameStarted()
    {
        Enable(true);
    }

    void HandleTouch()
    {
        activeTouch.OnUpdate();

        if (!isDragging)
        {
            if (Input.GetMouseButtonDown(0))
            {
                activeTouch.OnDown();
                isDragging = true;
            }
        }
        else
        {
            activeTouch.OnDrag();

            if (Input.GetMouseButtonUp(0))
            {
                activeTouch.OnUp();
                isDragging = false;
            }
        }
    }

    public void Initialize(TouchTypes tt, bool isButtonDerived = true, bool isStart = false)
    {
        isDragging = false;


        if (activeTouch != null)
        {
            activeTouch.OnDeInitialized();
        }

        activeTouch = tt == TouchTypes.NONE ? null : touchMechanics[(int)tt];

        if (activeTouch != null)
        {
            activeTouch.OnInitialized();
        }

        if (isStart)
            UIManager.I.Initialize(isButtonDerived);
    }
}
