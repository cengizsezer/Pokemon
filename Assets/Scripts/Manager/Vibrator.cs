using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibrator : MonoBehaviour
{
    public static Vibrator I;

    private void Awake()
    {
        if (I == null)
        {
            Init();
        }
        else Destroy(gameObject);
    }

    void Init()
    {
        I = this;
        DontDestroyOnLoad(gameObject);
        MMVibrationManager.SetHapticsActive(SaveLoadManager.HasVibration());
    }

    public static void ChangeVibrationStatus()
    {
        SaveLoadManager.ChangeVibrationStatus();
        MMVibrationManager.SetHapticsActive(SaveLoadManager.HasVibration());
    }

    public static void EnableVibration(bool isEnabled)
    {
        SaveLoadManager.SetVibrationStatus(isEnabled);
        MMVibrationManager.SetHapticsActive(SaveLoadManager.HasVibration());
    }

    public static void Haptic(HapticTypes type = HapticTypes.MediumImpact)
    {
        if (MMVibrationManager.HapticsSupported())
            MMVibrationManager.Haptic(type);
    }


}
