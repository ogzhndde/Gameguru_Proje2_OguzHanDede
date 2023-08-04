using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    public PlayerState playerStateEnum;

    [HideInInspector] public GameManager manager;
    [HideInInspector] public DivideManager divideManager;
    [HideInInspector] public GameData data;

    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public PlayerAnimation playerAnimation;


    [Space(5)]
    [Header("Structs")]
    public StructMovement structMovement;
    public StructAnimation structAnimation;


    [Serializable]
    public struct StructMovement
    {
        public GameObject TargetPlatform;
        public float speed;
        public float xSensitivity;
    }

    [Serializable]
    public struct StructAnimation
    {
        public Animator anim;
    }


    void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        divideManager = FindObjectOfType<DivideManager>();
        data = manager.data;
    }


    //########################################    EVENTS    ###################################################################

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnStart, OnStart);
        EventManager.AddHandler(GameEvent.OnFinish, OnFinish);
        EventManager.AddHandler(GameEvent.OnGenerateLevel, OnGenerateLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnStart, OnStart);
        EventManager.RemoveHandler(GameEvent.OnFinish, OnFinish);
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

    
    private void OnGenerateLevel()
    {
        playerStateEnum = PlayerState.IdlePhase;
    }

}
