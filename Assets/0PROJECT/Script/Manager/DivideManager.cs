using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivideManager : InstanceManager<DivideManager>
{
    GameManager manager;

    [Header("Definitions")]
    [SerializeField] private GameObject CurrentMovingPlatform;

    [SerializeField] private List<GameObject> AllPlatforms = new List<GameObject>();

    [SerializeField] private Transform reference;
    [SerializeField] private MeshRenderer referenceMesh;



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
        var newPlatform = Instantiate(nextPlatform, nextPlatform.transform.position + new Vector3(0, 0, nextPlatform.transform.localScale.z), Quaternion.identity);

        CurrentMovingPlatform = newPlatform;

        //PLATFORM SAG SOL ISLEMLERI MATERYAL FALAN BURAYA YAZILACAK
    }




    //########################################    EVENTS    ###################################################################

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnStart, OnStart);
        EventManager.AddHandler(GameEvent.OnDivide, OnDivide);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnStart, OnStart);
        EventManager.RemoveHandler(GameEvent.OnDivide, OnDivide);
    }

    private void OnStart()
    {
        CreateNewPlatform(AllPlatforms[^1]);
    }

    private void OnDivide()
    {
        reference = CurrentMovingPlatform.transform;
        referenceMesh = reference.GetComponent<MeshRenderer>();

        var stand = Instantiate(reference.gameObject, reference.position, Quaternion.identity);
        var falling = Instantiate(reference.gameObject, reference.position, Quaternion.identity);

        Transform lastPlatform = AllPlatforms[^1].transform;

        var distance = lastPlatform.position.x - reference.position.x;
        Debug.Log(distance);

        Divide(distance, ref stand, ref falling);



        stand.GetComponent<Platform>().StandProcess();
        falling.GetComponent<Platform>().FallingProcess();
        reference.GetComponent<Platform>().DestroyProcess();

    }

}
