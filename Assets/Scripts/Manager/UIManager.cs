using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Panels")]
    [SerializeField] Panels pnl;
    [Header("Images")]
    [SerializeField] Images img;
    [Header("Buttons")]
    [SerializeField] Buttons btn;
    [Header("Texts")]
    [SerializeField] Texts txt;
    [Header("Texts")]
    [SerializeField] Objects obj;

   
    //public Transform coinPoolParent;
    //public GameObject _canvas;
    //public GameObject ChooseTutorial;
    //public GameObject TapTutorial;
    //public GameObject SwipeTutorial;
    //public RectTransform CoinPoint;
   

    private CanvasGroup activePanel = null;
    private void Start()
    {
        UpdateTexts();
        UpdateCoinText();
        UpdateHapticStatus();
        GameManager.I.OnStartGame();
        EventManager.Send(OnStartGame.Create());
        EventManager.Add<OnEnemyState>(ShowFightButton);
        EventManager.Add<OnFight>(ClosedFightButton);
        btn.fightButton.onClick.AddListener(() => EventManager.Send(OnFight.Create()));
        btn.extraPogButton.onClick.AddListener(() =>
        {
            EventManager.Send(OnExtraPog.Create());
            SaveLoadManager.ReduceCoin(100);
            ShowExtraPogButton(false);
        });

    }
    public Panels GetPanel() => pnl;
    public Buttons GetButtons() => btn;

    public Objects GetObject() => obj;

    public void ShowChooseTutorial(bool on)
    {
        GetObject().ChooseTutorial.SetActive(on);
    }

    public void ShowTapTutorial(bool on)
    {
        GetObject().TapTutorial.SetActive(on);
    }

    public void ShowSwipeTutorial(bool on)
    {
        GetObject().SwipeTutorial.SetActive(on);
    }

    bool IsInteractable()
    {
        if (SaveLoadManager.GetCoin() >= 100)
        {
            return true;
        }


        return false;
    }

    private void OnDisable()
    {
        EventManager.Remove<OnEnemyState>(ShowFightButton);
        EventManager.Remove<OnFight>(ClosedFightButton);
        btn.fightButton.onClick.RemoveListener(() => EventManager.Send(OnFight.Create()));

    }


    public void ShowExtraPogButton(bool on)
    {
        if (LevelManager.I.IsFirstLevel()) return;
        btn.extraPogButton.gameObject.SetActive(on);
        btn.extraPogButton.interactable = IsInteractable();
    }



    public void ShowFightButton(OnEnemyState newEvent)
    {
        new DelayedAction(() => btn.fightButton.gameObject.SetActive(newEvent.on), 2.3f).Execute(this);

    }

    public void ClosedFightButton(OnFight fightEvent)
    {
        btn.fightButton.gameObject.SetActive(fightEvent.on);
        ShowChooseTutorial(false);
    }
    public void Initialize(bool isButtonDerived)
    {
        btn.play.gameObject.SetActive(isButtonDerived);
        //img.taptoStart.gameObject.SetActive(!isButtonDerived);
        FadeInAndOutPanels(pnl.mainMenu);
    }


    public void OnGameStarted()
    {
        FadeInAndOutPanels(pnl.gameIn);
    }

    public void OnFail()
    {
        FadeInAndOutPanels(pnl.fail);
    }

    public void OnSuccess()
    {
        btn.nextLevel.gameObject.SetActive(true);
        FadeInAndOutPanels(pnl.success);

    }
    

    public void ReloadScene(bool isSuccess)
    {
        GameManager.I.ReloadScene(isSuccess);
    }

    void FadeInAndOutPanels(CanvasGroup _in)
    {
        CanvasGroup _out = activePanel;
        activePanel = _in;

        if (_out != null)
        {
            _out.interactable = false;
            _out.blocksRaycasts = false;

            _out.DOFade(0f, Configs.UI.FadeOutTime).OnComplete(() =>
            {
                _in.DOFade(1f, Configs.UI.FadeOutTime).OnComplete(() =>
                {
                    _in.interactable = true;
                    _in.blocksRaycasts = true;
                });
            });
        }
        else
        {
            _in.DOFade(1f, Configs.UI.FadeOutTime).OnComplete(() =>
            {
                _in.interactable = true;
                _in.blocksRaycasts = true;
            });
        }


    }

    public void ShowJoystickHighlights(int area)
    {
        for (int i = 0; i < img.joystickHighlights.Length; i++)
        {
            img.joystickHighlights[i].gameObject.SetActive(i == area);
        }
    }

    public void UpdateTexts()
    {
        txt.level.text = "LEVEL " + (SaveLoadManager.GetLevel() + 1).ToString();
    }

    public void UpdateCoinText()
    {
        txt.Coin.text = (SaveLoadManager.GetCoin()).ToString();
    }

    public void UpdateHapticStatus()
    {
        for (int i = 0; i < img.vibrations.Length; i++)
        {
            img.vibrations[i].color = SaveLoadManager.HasVibration() ? img.vibrations[i].color.SetAlpha(1f) : img.vibrations[i].color.SetAlpha(.1f);
        }
    }

    public void ChangeHapticStatus()
    {
        SaveLoadManager.ChangeVibrationStatus();
    }

    [System.Serializable]
    public class Panels
    {
        public CanvasGroup mainMenu, gameIn, success, fail;
    }

    [System.Serializable]
    public class Images
    {
        public Image taptoStart;
        public Image[] joystickHighlights, vibrations;
    }

    [System.Serializable]
    public class Buttons
    {
        public Button play, nextLevel, fightButton, extraPogButton;
    }

    [System.Serializable]
    public class Texts
    {
        public TextMeshProUGUI level;
        public TextMeshProUGUI Coin;
    }

    [System.Serializable]
    public class Objects
    {
        public Transform coinPoolParent;
        public GameObject _canvas;
        public GameObject ChooseTutorial;
        public GameObject TapTutorial;
        public GameObject SwipeTutorial;
        public RectTransform CoinPoint;
    }
}
