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
    public TextMeshProUGUI TMP_LevelText;
    public Button BTN_NextLevel;
    public Button BTN_Restart;

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        data = manager.data;
    }

    private void Start()
    {


    }

    void Update()
    {

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

        EventManager.AddHandler(GameEvent.OnFinish, OnFinish);
        EventManager.AddHandler(GameEvent.OnFail, OnFail);
        EventManager.AddHandler(GameEvent.OnNextLevel, OnNextLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnFinish, OnFinish);
        EventManager.RemoveHandler(GameEvent.OnFail, OnFail);
        EventManager.RemoveHandler(GameEvent.OnNextLevel, OnNextLevel);
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
        data.LevelCount++;
        OBJ_WinPanel.SetActive(false);
    }

}
