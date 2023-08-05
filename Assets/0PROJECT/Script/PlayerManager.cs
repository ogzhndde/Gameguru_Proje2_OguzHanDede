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
     public GameObject ChildPlayer;

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
        ChildPlayer = transform.GetChild(0).gameObject;

    }

  


    //########################################    EVENTS    ###################################################################

    private void OnEnable()
    {
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
        Destroy(gameObject, 1.5f);
    }


    private void OnGenerateLevel()
    {
        playerStateEnum = PlayerState.IdlePhase;

        //RESET CHILD ROTATION TO PREVENT THE CHILD FROM TRACKING
        ChildPlayer.transform.eulerAngles = Vector3.zero;
        Debug.Log("rotayon sifirladim");
        {
            
        }
    }

}
