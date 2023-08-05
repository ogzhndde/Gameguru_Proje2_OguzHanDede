using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FinishLine : MonoBehaviour
{
    GameManager manager;

    [SerializeField] private GameObject StablePlatform;
    [SerializeField] private GameObject NextLevelPlatform;

    void Awake()
    {
        manager = FindObjectOfType<GameManager>();
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                EventManager.Broadcast(GameEvent.OnFinish);
                manager.CurrentFinishLine = gameObject;
                break;
        }
    }

    void NextLevelPreperation()
    {
        StablePlatform.SetActive(true);
        NextLevelPlatform.SetActive(true);
        NextLevelPlatform.GetComponent<Platform>().SetRandomMaterial();

        float jumpX = StablePlatform.transform.position.x;
        float jumpY = manager.Player.transform.position.y;
        float jumpZ = StablePlatform.transform.position.z;

        manager.Player.transform.DOJump(new Vector3(jumpX, jumpY, jumpZ), 1f, 1, 0.5f).OnComplete(() =>
        {
            EventManager.Broadcast(GameEvent.OnGenerateLevel);
        });

        EventManager.Broadcast(GameEvent.OnPlatformListReset, NextLevelPlatform);


    }



    //####################      EVENTS      ###########################
    void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnNextLevel, OnNextLevel);
    }

    void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel, OnNextLevel);

    }

    private void OnNextLevel()
    {
        if (manager.CurrentFinishLine == gameObject)
        {
            NextLevelPreperation();
        }
    }
}
