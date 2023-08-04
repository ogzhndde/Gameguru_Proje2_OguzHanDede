using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using DG.Tweening;
public class GameManager : InstanceManager<GameManager>
{
    public GameData data;

    public int normalShootCounter;

    [Space(10)]
    [Header("Bools")]
    public bool _isGameStarted = false;
    public bool _canDividePlatform = true;
    public bool _canCreatePlatform = true;


    void Awake()
    {
#if !UNITY_EDITOR
        SaveManager.LoadData(data);
#endif
    }

    void Start()
    {
        EventManager.Broadcast(GameEvent.OnGenerateLevel);
    }

    void Update()
    {

    }

    public void ShootCounter()
    {
        normalShootCounter++;

        if (normalShootCounter == data.lists.LevelPlatformCounts[data.values.LevelCount])
        {
            _canCreatePlatform = false;
        }
    }


    //########################################    EVENTS    ###################################################################

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnStart, OnStart);
        EventManager.AddHandler(GameEvent.OnFinish, OnFinish);
        EventManager.AddHandler(GameEvent.OnMissShoot, OnMissShoot);
        EventManager.AddHandler(GameEvent.OnNormalShoot, OnNormalShoot);
        EventManager.AddHandler(GameEvent.OnPerfectShoot, OnPerfectShoot);
        EventManager.AddHandler(GameEvent.OnGenerateLevel, OnGenerateLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnStart, OnStart);
        EventManager.RemoveHandler(GameEvent.OnFinish, OnFinish);
        EventManager.RemoveHandler(GameEvent.OnMissShoot, OnMissShoot);
        EventManager.RemoveHandler(GameEvent.OnNormalShoot, OnNormalShoot);
        EventManager.RemoveHandler(GameEvent.OnPerfectShoot, OnPerfectShoot);
        EventManager.RemoveHandler(GameEvent.OnGenerateLevel, OnGenerateLevel);
    }

    private void OnStart()
    {
        _isGameStarted = true;
    }
    private void OnFinish()
    {
        _isGameStarted = false;
    }

    private void OnMissShoot()
    {
        _canDividePlatform = false;
    }

    private void OnNormalShoot()
    {

        data.bools._isPerfectShoot = false;
        data.values.perfectShootCounter = 0;

        EventManager.Broadcast(GameEvent.OnPlaySound, "SoundNormalShoot");
    }

    private void OnPerfectShoot()
    {

        data.bools._isPerfectShoot = true;
        data.values.perfectShootCounter++;

        //CHECK LONGEST PERFECT SHOOT RALLY AND SAVE
        int longestRally = data.values.LongestPerfectShoot;
        int currentRally = data.values.perfectShootCounter;
        data.values.LongestPerfectShoot = currentRally > longestRally ? currentRally : longestRally;

        float soundPitchValue = 1f - (data.values.perfectShootCounter * 0.1f);
        EventManager.Broadcast(GameEvent.OnPlaySoundPitch, "SoundPerfectShoot", soundPitchValue < 0.1f ? 0.1f : soundPitchValue);
    }

    private void OnGenerateLevel()
    {
        //SET FINISH LINE POSITION ON NEXT LEVELS
        var beginPlatformPos = DivideManager.Instance.AllPlatforms[0].transform.position;
        var finishLineOffset = data.lists.LevelPlatformCounts[data.values.LevelCount] * (DivideManager.Instance.AllPlatforms[0].transform.localScale.z) + 2.233698f;

        var FinishLine = Instantiate(Resources.Load("Finish") as GameObject, beginPlatformPos + new Vector3(0, 7f, finishLineOffset), Quaternion.identity);

        FinishLine.transform.DOMove(FinishLine.transform.position + new Vector3(0, -6.5f, 0), 2f).SetEase(Ease.OutExpo);
    }
}
