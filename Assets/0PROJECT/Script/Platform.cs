using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Platform : MonoBehaviour
{
    public Direction directionEnum;

    [Space(10)]
    [Header("Lists")]
    public List<Material> AllMaterials = new List<Material>();

    [Space(10)]
    [Header("Int&Floats")]
    [SerializeField] private int MaterialIndex = 0;
    [SerializeField] private float speed;
    [SerializeField] private float limit;

    [Space(10)]
    [Header("Bools")]
    [SerializeField] private bool _isStop = false;
    private bool _isForward;


    void Start()
    {

    }


    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        if (_isStop) return;
        #region Move
        var position = transform.position;
        var direction = _isForward ? 1 : -1;
        var move = speed * Time.deltaTime * direction;

        position.x += move;

        if (position.x < -limit || position.x > limit)
        {
            position.x = Mathf.Clamp(position.x, -limit, limit);
            _isForward = !_isForward;
        }

        transform.position = position;
        #endregion
    }

    void SetMaterial()
    {
        MaterialIndex = MaterialIndex < AllMaterials.Count - 1 ? MaterialIndex + 1 : 0;
        GetComponent<MeshRenderer>().material = AllMaterials[MaterialIndex];
    }

    //#########################      PROCESSES    #########################
    public void SpawnProcess(Direction comingSide)
    {
        _isStop = false;

        directionEnum = comingSide == Direction.Left ? Direction.Right : Direction.Left;

        SetMaterial();
    }

    public void StandProcess()
    {
        _isStop = true;
    }

    public void FallingProcess()
    {
        _isStop = true;

        Rigidbody rb = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        rb.useGravity = true;
        Destroy(gameObject, 3f);

    }

    public void DestroyProcess()
    {
        Destroy(gameObject);
    }
}
