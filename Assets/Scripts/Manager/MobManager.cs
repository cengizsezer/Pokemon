using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MobManager : Singleton<MobManager>
{

    [SerializeField] private PlayerMob playerMobPrefab;
    [SerializeField] private EnemyMob enemyMobPrefab;
    [SerializeField] private PokeBallBehaviour pokeBall;
    [SerializeField] private Cell[] playerPlaces;
    [SerializeField] private Transform[] enemyPlaces;
    [SerializeField] private Vector3 EnemyPokeballPosition;
    private List<PlayerMob> lsBorns = new List<PlayerMob>();

    [HideInInspector] public MobBase tutorialPokemon = null;

    public PlayerMobController PlayerMobController = null;
    public EnemyMobController EnemyMobController = null;

    private MobManager()
    {
        PlayerMobController = new PlayerMobController();
        EnemyMobController = new EnemyMobController();
    }

    public Transform GetEnemyPosition(int id) => enemyPlaces[id];

    private void Start()
    {
       
        EventManager.Add<OnEnemyState>(EnemyState);
        EventManager.Add<OnTutorial>(TutorialPokemon);
    }

    private void OnDisable()
    {
        EventManager.Remove<OnEnemyState>(EnemyState);
        EventManager.Remove<OnTutorial>(TutorialPokemon);
        PlayerMobController = null;
        EnemyMobController = null;
    }

    private void EnemyState(OnEnemyState newEvent)
    {
        SpawnEnemies();
        if (LevelManager.I.IsFirstLevel())
            EventManager.Send(OnTutorial.Create());
    }

    #region SPAWN PLAYER POKEMON
    public void SpawnPlayerMobs(List<PogBehaviour> lsPogs)
    {
        PlayerMobController.lsMobs.Clear();
        lsBorns.Clear();

        
        if (lsPogs.Count <= playerPlaces.Length)
        {
            for (int i = 0; i < lsPogs.Count; i++)
            {

                SpawnSequence(lsPogs[i], playerPlaces[i]);
            }

        }
        else
        {
            for (int d = 0; d < playerPlaces.Length; d++)
            {
                SpawnSequence(lsPogs[d], playerPlaces[d]);
            }

        }
       
        new DelayedAction(() =>
        {

            for (int i = 0; i < DeckManager.I.lsDeckPogs.Count; i++)
            {
                DeckManager.I.lsDeckPogs[i].gameObject.SetActive(false);
            }

        }, 1f).Execute(this);

       
        //TODO AI HITS TABLE
        new DelayedAction(() =>
        {
            InputManager.I.Initialize(InputManager.TouchTypes.Merge);

            for (int i = 0; i < DeckManager.I.lsDeckPogs.Count; i++)
            {
                DeckManager.I.lsDeckPogs[i].gameObject.SetActive(false);
            }

        }, 3f).Execute(this);
       
        
        WaitForLanding();

    }
    public void WaitForLanding()
    {
        StartCoroutine(WaitLandingRoutine());
    }

    IEnumerator WaitLandingRoutine()
    {
        while (lsBorns.Count==0)
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitUntil(HasAllLanding);
        yield return new WaitForSeconds(.3f);
        EventManager.Send(OnEnemyState.Create());

    }

    bool HasAllLanding()
    {
        foreach (var pm in lsBorns)
        {
            if (!pm.settled)
            {
                return false;
            }
        }
        return true;
    }
    private Sequence SpawnSequence(PogBehaviour pog, Cell c)
    {
        Sequence s = DOTween.Sequence();
        s.AppendCallback(() => pog.ParticlePlay());
        s.AppendInterval(1f);
        s.AppendCallback(() => InstantiatePogemon(pog, c));
        s.AppendCallback(() => pog.gameObject.SetActive(false));

        return s;
    }
    private void InstantiatePogemon(PogBehaviour pog, Cell c)
    {
        PlayerMob pm = Instantiate(playerMobPrefab);
        lsBorns.Add(pm);
       
        pm.transform.position = pog.transform.position;
        pm.MOBTYPE = pog.PogType;
        pm.LEVEL = pog.LEVEL;
        pm.Borned(c);

        PlayerMobController.lsMobs.Add(pm);
       
    }
    private void TutorialPokemon(OnTutorial tutorial)
    {
        if (PlayerMobController.CheckTutorialPokemon())
        {
            new DelayedAction(() => UIManager.I.ShowChooseTutorial(true), .4f).Execute(this);
            return;
        }

        PlayerMob pm = Instantiate(playerMobPrefab);
        tutorialPokemon = pm;
        lsBorns.Add(pm);
        pm.transform.position = playerPlaces[4].transform.position;


        pm.MOBTYPE = PlayerMobController.lsMobs[0].MOBTYPE;
        pm.LEVEL = PlayerMobController.lsMobs[0].LEVEL;
        pm.TutorialPokemonBehaviour(playerPlaces[4]);
        pm.BornSequence();
        PlayerMobController.lsMobs.Add(pm);
        new DelayedAction(() => UIManager.I.ShowChooseTutorial(true), .4f).Execute(this);

    }
    #endregion

    #region REMOVE MOBS

    public void RemoveEnemyMobs(EnemyMob em)
    {
        if (!GameManager.I.isRunning) return;

        EnemyMobController.RemoveMobList(em);

        if (EnemyMobController.lsMobs.Count == 0)
        {
            
            PlayerMobController.ResetFightPlayerMobs();
            GameManager.I.DelayLevelComplated();

        }
    }
    public void RemovePlayerMobs(PlayerMob pm)
    {
        if (!GameManager.I.isRunning) return;

        PlayerMobController.RemoveMobList(pm);

        if (PlayerMobController.lsMobs.Count == 0)
        {
           
            EnemyMobController.ResetFightEnemies();
            GameManager.I.OnLevelFailed();
        }

    }
    #endregion

    #region SPAWN ENEMY POKEMON
    public void SpawnEnemies()
    {
        Level.LevelEnemies[] arrEnemies = LevelManager.I.GetLevel().lvlEnemies;

        for (int i = 0; i < arrEnemies.Length; i++)
        {
            EnemyMob em = Instantiate(enemyMobPrefab);
            EnemyMobController.lsMobs.Add(em);
            PokeBallBehaviour poke = Instantiate(pokeBall);
            poke.transform.position = EnemyPokeballPosition;
            em.SetProps(arrEnemies[i]);
            poke.ThrowPokeball(em.transform);
            em.BornSequence();

        }
    }

    #endregion

}
