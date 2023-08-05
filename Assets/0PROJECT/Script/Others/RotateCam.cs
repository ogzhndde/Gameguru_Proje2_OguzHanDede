using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCam : MonoBehaviour
{
    GameObject Player;
    Animator anim;


    void Awake()
    {
        Player = FindObjectOfType<PlayerManager>().gameObject;
        anim = GetComponent<Animator>();
    }

    //####################      EVENTS      ###########################
    void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnFinish, OnFinish);
        EventManager.AddHandler(GameEvent.OnGenerateLevel, OnGenerateLevel);
    }

    void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnFinish, OnFinish);
        EventManager.RemoveHandler(GameEvent.OnGenerateLevel, OnGenerateLevel);

    }

    private void OnGenerateLevel()
    {
        //RESET ROTATE VALUE WHEN GENERATE NEW LEVEL
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private void OnFinish()
    {
        //START ROTATING ON FINISH LINE
        transform.position = Player.transform.position;
        anim.Play("RotateCam", 0, 0);
    }
}
