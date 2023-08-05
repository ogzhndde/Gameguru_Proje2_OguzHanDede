using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivideManager : InstanceManager<DivideManager>
{
    GameManager manager;

    public List<GameObject> AllPlatforms = new List<GameObject>();

    [Space(10)]
    [Header("Definitions")]
    [SerializeField] private GameObject CurrentMovingPlatform;
    [SerializeField] private float perfectShotTolerance = 0.1f;
    [SerializeField] private Transform reference;
    [SerializeField] private MeshRenderer referenceMesh;

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
    }

    private void Divide(float distance, ref GameObject standPlatform, ref GameObject fallingPlatform)
    {
        Transform stand = standPlatform.transform;
        Transform falling = fallingPlatform.transform;

        bool isFirstFalling = distance > 0;

        //OLCU AYARI
        var fallingSize = reference.localScale;
        fallingSize.x = Mathf.Abs(distance);
        falling.localScale = fallingSize;

        var standSize = reference.localScale;
        standSize.x = reference.localScale.x - Mathf.Abs(distance);
        stand.localScale = standSize;

        //POZISYON AYARI
        var fallingPosition = GetPositionEdge(referenceMesh, isFirstFalling ? Direction.Left : Direction.Right);
        var fallingMultiply = (isFirstFalling ? 1 : -1);
        fallingPosition.x += (fallingSize.x / 2) * fallingMultiply;
        falling.position = fallingPosition;

        var standPosition = GetPositionEdge(referenceMesh, !isFirstFalling ? Direction.Left : Direction.Right);
        var standMultiply = (!isFirstFalling ? 1 : -1);
        standPosition.x += (standSize.x / 2) * standMultiply;
        stand.position = standPosition;

        //LISTE ISLEMLERI
        AllPlatforms.Add(stand.gameObject);

        if (manager._canCreatePlatform)
            CreateNewPlatform(stand.gameObject);
    }

    private Vector3 GetPositionEdge(MeshRenderer mesh, Direction direction)
    {
        var extents = mesh.bounds.extents;
        var position = mesh.transform.position;

        switch (direction)
        {
            case Direction.Left:
                position.x += -extents.x;
                break;
            case Direction.Right:
                position.x += extents.x;
                break;
        }

        return position;
    }

    private void CreateNewPlatform(GameObject nextPlatform)
    {
        var comingSide = nextPlatform.GetComponent<Platform>().directionEnum;
        var xOffset = comingSide == Direction.Left ? -1 : 1;
        var distanceFromNextPlatform = xOffset * 3.5f;

        var newPosition = nextPlatform.transform.position + new Vector3(distanceFromNextPlatform, 0, nextPlatform.transform.localScale.z);
        var newPlatform = Instantiate(nextPlatform, newPosition, Quaternion.identity);

        CurrentMovingPlatform = newPlatform;

        Platform platform = newPlatform.GetComponent<Platform>();
        platform.SpawnProcess(comingSide);
    }




    //########################################    EVENTS    ###################################################################

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnStart, OnStart);
        EventManager.AddHandler(GameEvent.OnDivide, OnDivide);
        EventManager.AddHandler(GameEvent.OnPlatformListReset, OnPlatformListReset);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnStart, OnStart);
        EventManager.RemoveHandler(GameEvent.OnDivide, OnDivide);
        EventManager.RemoveHandler(GameEvent.OnPlatformListReset, OnPlatformListReset);
    }

    private void OnStart()
    {
        CreateNewPlatform(AllPlatforms[^1]);
    }

    private void OnDivide()
    {
        if (!manager._canDividePlatform) return;
        if (!CurrentMovingPlatform) return;
        manager.ShootCounter();

        reference = CurrentMovingPlatform.transform;
        referenceMesh = reference.GetComponent<MeshRenderer>();
        Transform lastPlatform = AllPlatforms[^1].transform;

        var distance = lastPlatform.position.x - reference.position.x;

        #region Miss Shot
        if (Mathf.Abs(distance) > reference.localScale.x)
        {
            CurrentMovingPlatform.GetComponent<Platform>().FallingProcess();
            EventManager.Broadcast(GameEvent.OnMissShoot);
            return;
        }
        #endregion

        #region Perfect Shot
        if (Mathf.Abs(distance) <= perfectShotTolerance)
        {
            distance = 0;
            reference.transform.position = new Vector3(lastPlatform.transform.position.x,
                                                        reference.transform.position.y,
                                                        reference.transform.position.z);

            EventManager.Broadcast(GameEvent.OnPerfectShoot);
        }
        #endregion

        var stand = Instantiate(reference.gameObject, reference.position, Quaternion.identity);
        var falling = Instantiate(reference.gameObject, reference.position, Quaternion.identity);

        Divide(distance, ref stand, ref falling);

        //MAKE PROCESS ON PIECES CREATED
        #region Piece Process 
        stand.GetComponent<Platform>().StandProcess();
        reference.GetComponent<Platform>().DestroyProcess();

        if (distance != 0) //NORMAL SHOOT
        {
            falling.GetComponent<Platform>().FallingProcess();
            EventManager.Broadcast(GameEvent.OnNormalShoot);
        }
        else //PERFECT SHOOT
        {
            falling.GetComponent<Platform>().DestroyProcess();
        }
        #endregion 

    }

    private void OnPlatformListReset(object value)
    {
        var newBeginPlatform = (GameObject)value;

        //DESTROY ALL PREVIOUS PLATFORMS EXCEPT LAST ONE
        for (int i = 0; i < AllPlatforms.Count - 1; i++)
        {
            Destroy(AllPlatforms[i]);
        }

        AllPlatforms.Clear();
        AllPlatforms.Add(newBeginPlatform);
    }

}
