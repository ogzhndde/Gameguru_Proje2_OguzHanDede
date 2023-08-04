using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : InstanceManager<GameManager>
{
    public GameData data;



    [Space(10)]
    [Header("Bools")]
    public bool _isGameStarted = false;
    public bool _canDividePlatform = true;


    void Awake()
    {
#if !UNITY_EDITOR
        SaveManager.LoadData(data);
#endif
    }

    void Start()
    {
        
    }

    void Update()
    {

    }



    //########################################    EVENTS    ###################################################################

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnStart, OnStart);
        EventManager.AddHandler(GameEvent.OnGameFail, OnGameFail);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnStart, OnStart);
        EventManager.RemoveHandler(GameEvent.OnGameFail, OnGameFail);
    }

    private void OnStart()
    {
       _isGameStarted = true;
    }

    private void OnGameFail()
    {
       _canDividePlatform = false;
    }
}
