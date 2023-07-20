using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] public bool canStart = false;
    [SerializeField] public bool isRunning = false;

    public void OnStartGame()
    {
        if (isRunning || !canStart) return;

        canStart = false;

        //TODO SEND ANALYTICS EVENT

        UIManager.I.OnGameStarted();
        InputManager.I.OnGameStarted();
        isRunning = true;
    }

    public void OnLevelCompleted()
    {
        isRunning = false;
        canStart = false;

        StopAllCoroutines();

        UIManager.I.OnSuccess();
    }

    public void OnLevelFailed()
    {
        isRunning = false;
        canStart = false;

        StopAllCoroutines();
        UIManager.I.OnFail();
    }

    public void ReloadScene(bool isSuccess)
    {
        //TODO SEND ANALYTICS EVENT

        if (isSuccess)
        {
            SaveLoadManager.IncreaseLevel();
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void DelayLevelComplated()
    {
        StartCoroutine(WaitLevelEndRoutine());
    }

    IEnumerator WaitLevelEndRoutine()
    {
        isRunning = false;
        yield return new WaitForEndOfFrame();
        OnLevelCompleted();

    }
}
