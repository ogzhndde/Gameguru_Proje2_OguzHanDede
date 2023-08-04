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
    [SerializeField] private Transform falling;
    [SerializeField] private Transform stand;

    [SerializeField] private Transform Last;

    public float testValue;

    [ContextMenu("Test")]
    private void Test()
    {
        Divide(Last.position.x - reference.position.x);
    }

    private void Divide(float value)
    {
        bool isFirstFalling = value > 0;

        //Size
        var fallingSize = reference.localScale;
        fallingSize.x = Mathf.Abs(value);
        falling.localScale = fallingSize;

        var standSize = reference.localScale;
        standSize.x = reference.localScale.x - Mathf.Abs(value);
        stand.localScale = standSize;

        //Position
        var fallingPosition = GetPositionEdge(referenceMesh, isFirstFalling ? Direction.Left : Direction.Right);
        var fallingMultiply = (isFirstFalling ? 1 : -1);
        fallingPosition.x += (fallingSize.x / 2) * fallingMultiply;
        falling.position = fallingPosition;

        var standPosition = GetPositionEdge(referenceMesh, !isFirstFalling ? Direction.Left : Direction.Right);
        var standMultiply = (!isFirstFalling ? 1 : -1);
        standPosition.x += (standSize.x / 2) * standMultiply;
        stand.position = standPosition;
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
        throw new NotImplementedException();
    }

    private void OnDivide()
    {
        throw new NotImplementedException();
    }

}
