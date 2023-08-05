using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    // ALL DIVIDE PROCESSES
    private void Divide(float distance, ref GameObject standPlatform, ref GameObject fallingPlatform)
    {
        Transform stand = standPlatform.transform;
        Transform falling = fallingPlatform.transform;

        bool isFirstFalling = distance > 0;

        // SCALE
        #region Scale
        Vector3 referenceScale = reference.localScale;
        float fallingScaleX = Mathf.Abs(distance);
        float standScaleX = referenceScale.x - fallingScaleX;
        falling.localScale = new Vector3(fallingScaleX, referenceScale.y, referenceScale.z);
        stand.localScale = new Vector3(standScaleX, referenceScale.y, referenceScale.z);
        #endregion

        // POSITION 
        #region Position
        Vector3 fallingPosition = GetPositionEdge(referenceMesh, isFirstFalling ? Direction.Left : Direction.Right);
        float fallingOffset = fallingScaleX / 2f;
        fallingPosition.x += isFirstFalling ? fallingOffset : -fallingOffset;
        falling.position = fallingPosition;

        Vector3 standPosition = GetPositionEdge(referenceMesh, !isFirstFalling ? Direction.Left : Direction.Right);
        float standOffset = standScaleX / 2f;
        standPosition.x += !isFirstFalling ? standOffset : -standOffset;
        stand.position = standPosition;
        #endregion

        // PLATFORM LIST OPERATIONS
        AllPlatforms.Add(stand.gameObject);

        if (manager._canCreatePlatform)
            CreateNewPlatform(stand.gameObject);
    }

    //DETECT THE LIMITS OF THE MESH
    private Vector3 GetPositionEdge(MeshRenderer mesh, Direction direction)
    {
        var extents = mesh.bounds.extents;
        var position = mesh.transform.position;

        position.x += direction switch { Direction.Left => -extents.x, Direction.Right => extents.x, _ => 0 };
        return position;
    }

    private void CreateNewPlatform(GameObject nextPlatform)
    {
        var comingSide = nextPlatform.GetComponent<Platform>().directionEnum;
        var xOffset = comingSide == Direction.Left ? -1 : 1;
        var distanceFromNextPlatform = xOffset * 3.5f;

        var newPosition = nextPlatform.transform.position + new Vector3(distanceFromNextPlatform, 0, nextPlatform.transform.localScale.z);
        var newPlatform = Instantiate(nextPlatform, newPosition, Quaternion.identity);

        //UPDATE CURRENT MOVING PLATFORM
        CurrentMovingPlatform = newPlatform;

        //NECESSARY PROCEDURES ARE TAKING ON THE SPAWNED PLATFORM
        Platform platform = newPlatform.GetComponent<Platform>();
        platform.SpawnProcess(comingSide);
    }

    //########################################    EVENTS    ###################################################################

    private void OnEnable()
    {
        //DEFINE EVENTS USED IN THIS SCRIPT
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
        if (!manager._canDividePlatform || !CurrentMovingPlatform) return;

        //COUNT ON EVERY DIVIDE
        manager.ShootCounter();

        //DEFINE REFERENCE
        reference = CurrentMovingPlatform.transform;
        referenceMesh = reference.GetComponent<MeshRenderer>();
        Transform lastPlatform = AllPlatforms[^1].transform;

        //CHECK DISTANCE BETWEEN REFERENCE AND LAST STABLE PLATFORM
        var distance = lastPlatform.position.x - reference.position.x;

        #region Miss Shot
        //IF DISTANCE IS LARGER THAN PLATFORM, YOU MISS SHOOT
        if (Mathf.Abs(distance) > reference.localScale.x)
        {
            CurrentMovingPlatform.GetComponent<Platform>().FallingProcess();
            EventManager.Broadcast(GameEvent.OnMissShoot);
            return;
        }
        #endregion

        #region Perfect Shot
        //IF DISTANCE IS SMALLER THAN TOLERANCE VALUE, IT IS PERFECT SHOOT
        if (Mathf.Abs(distance) <= perfectShotTolerance)
        {
            distance = 0;
            reference.transform.position = new Vector3(lastPlatform.transform.position.x,
                                                        reference.transform.position.y,
                                                        reference.transform.position.z);

            EventManager.Broadcast(GameEvent.OnPerfectShoot);
        }
        #endregion

        //SPAWN TWO NEW PLATFORMS, ONE IS STANDING PLATFORM, OTHER IS FALLING ONE
        var stand = Instantiate(reference.gameObject, reference.position, Quaternion.identity);
        var falling = Instantiate(reference.gameObject, reference.position, Quaternion.identity);

        //DIVIDING PROCESS  
        Divide(distance, ref stand, ref falling);

        //ALL PROCESSES ON NEW PLATFORMS
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
            stand.GetComponent<Platform>().PerfectShootProcess();
            falling.GetComponent<Platform>().DestroyProcess();
        }
        #endregion 

    }

    //RESET ALL PLATFORM LIST WHEN PASS THE NEXT LEVEL
    private void OnPlatformListReset(object value)
    {
        var newBeginPlatform = (GameObject)value;

        //DESTROY ALL PREVIOUS PLATFORMS WITH A DELAY
        for (int i = 0; i < AllPlatforms.Count; i++)
        {
            Destroy(AllPlatforms[i], i < AllPlatforms.Count - 2 ? 1f : 10f);
        }

        AllPlatforms.Clear();

        //ADD NEW LEVEL BEGINNING PLATFORM TO LIST
        AllPlatforms.Add(newBeginPlatform);
    }

}
