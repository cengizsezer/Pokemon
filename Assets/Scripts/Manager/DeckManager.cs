using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : Singleton<DeckManager>
{
    List<int> lsDeckAsInt = new List<int>();
    List<int> lsExtraPogsAsInt = new List<int>();
    List<PogBehaviour> lsExtraDeckPogs = new List<PogBehaviour>();
    [SerializeField]
    public List<PogBehaviour> lsDeckPogs = new List<PogBehaviour>();
    public Paths[] paths;
    [SerializeField] GameObject goSpawnerObj;
    public GameObject hand;
    [SerializeField] ChoiceHandController ChooseHand;
    [SerializeField] IndicatorController indicator;
    [SerializeField] ParticleSystem psCloud;
    public GameObject TutorialArea;
    public GameObject TutorialArrow;
    private void Start()
    {
        EventManager.Add<OnExtraPog>(ExtraPogEventStarted);
    }

    private void OnDisable()
    {
        EventManager.Remove<OnExtraPog>(ExtraPogEventStarted);
    }


    void ExtraPogEventStarted(OnExtraPog @event)
    {
        InputManager.I.Initialize(InputManager.TouchTypes.NONE);
        ExtraPogsBox.DOLocalMoveX(0f, 0.3f);
        //ChooseHand.gameObject.SetActive(true);

        ChooseHand.startPos = new Vector3(0.5f, 1.6f, -2f);
        ChooseHand.endPos = new Vector3(1.5f, 1.6f, -0.9f);
        ChooseHand.transform.DOMoveX(0.5f, 0.3f).OnComplete(() => ChooseHand.isChoose = true);
        StartCoroutine(ChooseHand.Init());
        hand.transform.DOMoveX(-5f, 0.2f);
        new DelayedAction(() => InputManager.I.Initialize(InputManager.TouchTypes.Choose), .5f).Execute(this);


    }

    public void ExtraPogLand()
    {
        hand.transform.DOMoveX(0f, 0.2f);

    }

    public void ClosedChooseBox()
    {
        ExtraPogsBox.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => ExtraPogsBox.gameObject.SetActive(false));
    }

    public void RepeatDeck()
    {
        GameManager.I.ReloadScene(false);
    }

    [SerializeField] Transform ExtraPogsBox;
    [SerializeField] Transform ExtraPogPos;
    Vector3 newPos = new Vector3(0f, 0f, -0.18f);
    public void GetExtraPogs()
    {
        int[] arr = SaveLoadManager.GetExtraPogs();
        lsExtraPogsAsInt.Clear();
        lsExtraDeckPogs.Clear();
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] >= 0)
                lsExtraPogsAsInt.Add(arr[i]);
        }

        for (int i = 0; i < lsExtraPogsAsInt.Count; i++)
        {
            PogBehaviour pb = PoolHandler.I.GetObject<PogBehaviour>();
            pb.Init(lsExtraPogsAsInt[i]);

            lsExtraDeckPogs.Add(pb);
        }

        for (int i = 0; i < lsExtraDeckPogs.Count; i++)
        {
            lsExtraDeckPogs[i].transform.parent = ExtraPogsBox;
            //Vector3 pos = ExtraPogPos.position;

            newPos.x = 0f;
            newPos.y = 0.125f;
            newPos.z += 0.03f;
            lsExtraDeckPogs[i].transform.localPosition = newPos;

            Vector3 newRot = new Vector3(0, 90f, -90f);
            lsExtraDeckPogs[i].transform.localEulerAngles = newRot;


        }

    }
    public void GetDeck()
    {
        int[] arr = SaveLoadManager.GetDeck();
       
        lsDeckAsInt.Clear();

        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] >= 0)
                lsDeckAsInt.Add(arr[i]);
        }


        SpawnPlayer();
    }

    public void SpawnPlayer()
    {

        lsDeckPogs = new List<PogBehaviour>();
        lsDeckPogs.Clear();
        for (int i = 0; i < lsDeckAsInt.Count; i++)
        {
            PogBehaviour pb = PoolHandler.I.GetObject<PogBehaviour>();
           
            pb.Init(lsDeckAsInt[i]);

            lsDeckPogs.Add(pb);
        }

        StartSpawnRoutine(lsDeckPogs, paths[0], 3f);
    }

    void StartSpawnRoutine(List<PogBehaviour> lsPogs, Paths path, float time)
    {
        StartCoroutine(SpawnRoutine(lsPogs, path, time));

    }
    [SerializeField] Transform HandInGameTransform;
    IEnumerator SpawnRoutine(List<PogBehaviour> lsPogs, Paths path, float time)
    {

        Vector3 startPos = goSpawnerObj.transform.position;
        for (int i = 0; i < lsPogs.Count; i++)
        {
            lsPogs[i].gameObject.SetActive(false);
            lsPogs[i].rb.velocity = Vector3.zero;
            lsPogs[i].transform.localEulerAngles = Vector3.zero;
        }

        goSpawnerObj.SetActive(true);

        yield return goSpawnerObj.transform.DOMove(path.path[0].position, .5f).WaitForCompletion();

        Vector3[] _path = new Vector3[path.path.Length];

        for (int i = 0; i < path.path.Length; i++)
        {
            _path[i] = path.path[i].position;
        }

        float spawnInterval = time / lsPogs.Count;

        goSpawnerObj.transform.DOPath(_path, time).SetEase(Ease.Linear);

        for (int i = 0; i < lsPogs.Count; i++)
        {
            yield return new WaitForSeconds(spawnInterval);

            lsPogs[i].transform.position = goSpawnerObj.transform.position + Vector3.down * .3f;
            lsPogs[i].GetRB().isKinematic = false;
            lsPogs[i].gameObject.SetActive(true);
            psCloud.Play();
        }

        yield return new WaitForSeconds(.4f);

        yield return goSpawnerObj.transform.DOMove(startPos, .5f).OnComplete(() => goSpawnerObj.SetActive(false)).WaitForCompletion();

        yield return hand.transform.DOLocalMove(HandInGameTransform.position, .3f).OnComplete(() => hand.transform.eulerAngles = hand.transform.eulerAngles).WaitForCompletion();

        InputManager.I.Initialize(InputManager.TouchTypes.HandSwipe);


        UIManager.I.ShowExtraPogButton(true);

        if (LevelManager.I.IsFirstLevel())
        {
            TutorialArea.SetActive(true);
            TutorialArrow.SetActive(true);
            UIManager.I.ShowSwipeTutorial(true);
        }

    }


    public void HidePogs()
    {
        foreach (var pog in lsDeckPogs)
        {
            pog.gameObject.SetActive(false);
            pog.transform.eulerAngles = Vector3.zero;

        }
    }

    public void CheckPlayerPogs()
    {
        List<PogBehaviour> lsUpsideDowns = new List<PogBehaviour>();

        for (int i = 0; i < lsDeckPogs.Count; i++)
        {
            float f = Vector3.Dot(lsDeckPogs[i].transform.up, Vector3.up);

            //Debug.Log(lsDeckPogs[i].gameObject.name + " -- > " + f);

            if (f < 0)
            {
                lsUpsideDowns.Add(lsDeckPogs[i]);

            }
        }

        if (lsUpsideDowns.Count == 0)
        {
            //HidePogs();
            RepeatDeck();
            return;
        }

       
        MobManager.I.SpawnPlayerMobs(lsUpsideDowns);

    }

    [SerializeField] List<PogBehaviour> lsAired = new List<PogBehaviour>();
    public void WaitForLanding(List<PogBehaviour> lsAireds)
    {

        lsAired.Clear();
        lsAired = lsAireds;
        StartCoroutine(WaitLandingRoutine());
    }
    IEnumerator WaitLandingRoutine()
    {
        yield return new WaitUntil(HasAllLanding);

        //yield return new WaitForSeconds(1f);

        CheckPlayerPogs();


    }
    bool HasAllLanding()
    {
        foreach (var pog in lsAired)
        {
            if (pog.rb.velocity.magnitude >= 0.3f || !pog.isGrounded || !pog.isFellOnTheTable || !pog.isForceApplied)
            {
                return false;
            }
        }
        return true;
    }

    [System.Serializable]
    public class Paths
    {
        public Transform[] path;
    }


}
