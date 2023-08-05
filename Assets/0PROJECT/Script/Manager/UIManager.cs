using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : InstanceManager<UIManager>
{
    GameManager manager;
    GameData data;

    [Header("Definitions")]
    public GameObject OBJ_WinPanel;
    public GameObject OBJ_FailPanel;
    public GameObject OBJ_MainPanel;
    public GameObject OBJ_TapToStart;

    public TextMeshProUGUI TMP_LevelText;
    public TextMeshProUGUI TMP_CoinText;
    public TextMeshProUGUI TMP_LongestShootText;

    public Button BTN_NextLevel;
    public Button BTN_Restart;


    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        data = manager.data;

        InvokeRepeating(nameof(TextCheck), 0.1f, 0.1f);
    }

    private void TextCheck()
    {
        TMP_LevelText.text = "Level " + data.UILevelCount;
        TMP_CoinText.text = data.TotalCoin.ToString();
        TMP_LongestShootText.text = "Perfect Record: " + data.values.perfectShootCounter;
    }

    //######################################################### BUTTONS ##############################################################

    void NextLevelButton()
    {
        EventManager.Broadcast(GameEvent.OnNextLevel);
    }

    void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //########################################    EVENTS    ###################################################################

    private void OnEnable()
    {
        BTN_NextLevel.onClick.AddListener(NextLevelButton);
        BTN_Restart.onClick.AddListener(RestartButton);

        EventManager.AddHandler(GameEvent.OnStart, OnStart);
        EventManager.AddHandler(GameEvent.OnFinish, OnFinish);
        EventManager.AddHandler(GameEvent.OnFail, OnFail);
        EventManager.AddHandler(GameEvent.OnNextLevel, OnNextLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnStart, OnStart);
        EventManager.RemoveHandler(GameEvent.OnFinish, OnFinish);
        EventManager.RemoveHandler(GameEvent.OnFail, OnFail);
        EventManager.RemoveHandler(GameEvent.OnNextLevel, OnNextLevel);
    }

    private void OnStart()
    {
        OBJ_TapToStart.SetActive(false);
    }

    private void OnFinish()
    {
        OBJ_WinPanel.SetActive(true);
    }

    private void OnFail()
    {
        OBJ_FailPanel.SetActive(true);
    }

    private void OnNextLevel()
    {
        //IF REACH THE LAST LEVEL, KEEP PLAYING LAST LEVEL
        data.LevelCount += data.LevelCount < data.lists.LevelPlatformCounts.Count - 1 ? 1 : 0;
        
        //UPDATE UI LEVEL COUNT
        data.UILevelCount++;

        OBJ_TapToStart.SetActive(true);
        OBJ_WinPanel.SetActive(false);
    }

}
