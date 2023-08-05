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
    [Header("Definitions")]
    public GameObject Player;
    public GameObject CurrentFinishLine;

    [Space(10)]
    [Header("Bools")]
    public bool _isGameStarted = false;
    public bool _isGameFail = false;
    public bool _canDividePlatform = true;
    public bool _canCreatePlatform = true;

    [Space(10)]
    [Header("Lists")]
    public List<GameObject> Collectables = new List<GameObject>();

    void Awake()
    {
        //LOAD DATA FOR MOBILE DEVICES
#if !UNITY_EDITOR
        SaveManager.LoadData(data);
#endif
        //MAKE FPS STABLE
        Application.targetFrameRate = 60;

        data.values.perfectShootCounter = 0;

        Player = FindObjectOfType<PlayerManager>().gameObject;
    }

    void Start()
    {
        //GENERATE LEVEL IN BEGINNING OF THE GAME
        EventManager.Broadcast(GameEvent.OnGenerateLevel);

        InvokeRepeating(nameof(CheckFail), 0.2f, 0.2f);
        InvokeRepeating(nameof(SaveData), 0.2f, 0.2f);
    }


    void CheckFail()
    {
        if (_isGameFail) return;

        //IF PLAYER FALLS, GAME FAIL
        if (Player.transform.position.y < -0.2f)
        {
            _isGameFail = true;
            EventManager.Broadcast(GameEvent.OnFail);
        }
    }

    public void ShootCounter()
    {
        //COUNT PLATFORM NUMBER IN EVERY DIVIDE
        normalShootCounter++;

        //IF REACH LEVEL PLATFORM COUNT, STOP CREATE NEW PLATFORMS
        if (normalShootCounter == data.lists.LevelPlatformCounts[data.LevelCount])
            _canCreatePlatform = false;

        //CHECK HIGH SCORE AND SAVE
        int highScore = data.values.HighScore;
        int currentScore = normalShootCounter;
        data.values.HighScore = currentScore > highScore ? currentScore : highScore;
    }

    private void ClearPreviousLevelValues()
    {
        //WHEN YOU GET NEXT LEVEL, RESET VALUES
        _isGameStarted = false;
        _canCreatePlatform = true;

        normalShootCounter = 0;
    }

    public void SaveData()
    {
        SaveManager.SaveData(data);
    }


    //########################################    EVENTS    ###################################################################

    private void OnEnable()
    {
        //DEFINE EVENTS USED IN THIS SCRIPT
        EventManager.AddHandler(GameEvent.OnStart, OnStart);
        EventManager.AddHandler(GameEvent.OnFinish, OnFinish);
        EventManager.AddHandler(GameEvent.OnFail, OnFail);
        EventManager.AddHandler(GameEvent.OnMissShoot, OnMissShoot);
        EventManager.AddHandler(GameEvent.OnNormalShoot, OnNormalShoot);
        EventManager.AddHandler(GameEvent.OnPerfectShoot, OnPerfectShoot);
        EventManager.AddHandler(GameEvent.OnGenerateLevel, OnGenerateLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnStart, OnStart);
        EventManager.RemoveHandler(GameEvent.OnFinish, OnFinish);
        EventManager.RemoveHandler(GameEvent.OnFail, OnFail);
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
        data.values.perfectShootCounter = 0;
        EventManager.Broadcast(GameEvent.OnPlaySound, "SoundFinish");
    }

    private void OnFail()
    {
        EventManager.Broadcast(GameEvent.OnPlaySound, "SoundFail");
    }

    private void OnMissShoot()
    {
        //WHEN MISS THE PLATFORM, STOP DIVIDING
        _canDividePlatform = false;
        EventManager.Broadcast(GameEvent.OnPlaySound, "SoundMiss");
    }

    private void OnNormalShoot()
    {
        //LOSE PERFECT SHOOT COMBO
        data.bools._isPerfectShoot = false;
        data.values.perfectShootCounter = 0;

        EventManager.Broadcast(GameEvent.OnPlaySound, "SoundNormal");
    }

    private void OnPerfectShoot()
    {
        //KEEP COUNTING PERFECT SHOOT COMBO
        data.bools._isPerfectShoot = true;
        data.values.perfectShootCounter++;

        //CHECK LONGEST PERFECT SHOOT STREAK AND SAVE
        int longestStreak = data.values.LongestPerfectStreak;
        int currentStreak = data.values.perfectShootCounter;
        data.values.LongestPerfectStreak = currentStreak > longestStreak ? currentStreak : longestStreak;

        //PLAY A SOUND THAT GOES FURTHER
        float soundPitchValue = 0.9f + (data.values.perfectShootCounter * 0.1f);
        EventManager.Broadcast(GameEvent.OnPlaySoundPitch, "SoundPerfect", soundPitchValue > 3f ? 3f : soundPitchValue);
    }

    private void OnGenerateLevel()
    {
        //SET FINISH LINE POSITION ON NEXT LEVELS
        var beginPlatformPos = DivideManager.Instance.AllPlatforms[0].transform.position;
        var finishLineOffset = data.lists.LevelPlatformCounts[data.LevelCount] * (DivideManager.Instance.AllPlatforms[0].transform.localScale.z) + 2.233698f;

        //INSTANTIATE NEW FINISH LINE
        var FinishLine = Instantiate(Resources.Load("Finish") as GameObject, beginPlatformPos + new Vector3(0, 7f, finishLineOffset), Quaternion.identity);

        FinishLine.transform.DOMove(FinishLine.transform.position + new Vector3(0, -6.5f, 0), 3f).SetEase(Ease.OutExpo);

        ClearPreviousLevelValues();

        EventManager.Broadcast(GameEvent.OnPlaySound, "SoundJump");
    }
}
