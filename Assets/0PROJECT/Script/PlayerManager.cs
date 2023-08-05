using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManager : InstanceManager<PlayerManager>
{
    public PlayerState playerStateEnum;

    [HideInInspector] public GameManager manager;
    [HideInInspector] public DivideManager divideManager;
    [HideInInspector] public GameData data;
    [HideInInspector] public GameObject ChildPlayer;

    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public PlayerAnimation playerAnimation;

    [Space(5)]
    [Header("Structs")]
    public StructMovement structMovement;
    public StructAnimation structAnimation;

    [Serializable] //DEFINE ALL MOVEMENT VARIABLES IN THIS STRUCT AND CALL THEM WHEN NECESSARY.
    public struct StructMovement
    {
        public GameObject TargetPlatform;
        public float speed;
        public float xSensitivity;
    }

    [Serializable]
    public struct StructAnimation //DEFINE ALL ANIMATION VARIABLES IN THIS STRUCT AND CALL THEM WHEN NECESSARY.
    {
        public Animator anim;
    }


    void Awake()
    {
        //DEFINE ALL CONTROL MANAGERS
        manager = FindObjectOfType<GameManager>();
        divideManager = FindObjectOfType<DivideManager>();
        data = manager.data;
        ChildPlayer = transform.GetChild(0).gameObject;
    }

    //########################################    EVENTS    ###################################################################

    private void OnEnable()
    {
        //DEFINE EVENTS USED IN THIS SCRIPT
        EventManager.AddHandler(GameEvent.OnStart, OnStart);
        EventManager.AddHandler(GameEvent.OnFinish, OnFinish);
        EventManager.AddHandler(GameEvent.OnFail, OnFail);
        EventManager.AddHandler(GameEvent.OnGenerateLevel, OnGenerateLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnStart, OnStart);
        EventManager.RemoveHandler(GameEvent.OnFinish, OnFinish);
        EventManager.RemoveHandler(GameEvent.OnFail, OnFail);
        EventManager.RemoveHandler(GameEvent.OnGenerateLevel, OnGenerateLevel);
    }

    private void OnStart()
    {
        playerStateEnum = PlayerState.RunPhase;
    }

    private void OnFinish()
    {
        playerStateEnum = PlayerState.FinishPhase;
    }

    private void OnFail()
    {
        playerStateEnum = PlayerState.FallingPhase;
        Destroy(gameObject, 1.5f);
    }

    private void OnGenerateLevel()
    {
        playerStateEnum = PlayerState.IdlePhase;
    }

}
