using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] Level testLevel;
    [SerializeField] Transform pool;
    [SerializeField] Level[] allLevels;

    Level crntLevel;

    public Level GetLevel() => crntLevel;

    private void Start()
    {
        CreateLevel();
        InputManager.I.Initialize(InputManager.TouchTypes.NONE, isButtonDerived: false, isStart: true);

    }

    public bool IsFirstLevel()
    {
        if (SaveLoadManager.GetLevel() == 0)
        {
            return true;
        }


        return false;
    }

    public void CreateLevel()
    {
        if (testLevel == null && allLevels.Length == 0) return;

        int levelID = allLevels.Length >= 1 ? SaveLoadManager.GetLevel() % allLevels.Length : 0;

        crntLevel = Instantiate(testLevel != null ? testLevel : allLevels[levelID], pool);
        //UIManager.I.UpdateCoinText();
        GameManager.I.canStart = true;
    }
}
