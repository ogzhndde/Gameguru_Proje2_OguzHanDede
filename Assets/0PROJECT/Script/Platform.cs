using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Direction directionEnum;

    [SerializeField] private bool _isStop = false;


    [SerializeField][Range(1, 5)] private float speed;
    [SerializeField][Range(1, 2)] private float limit;

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

    }

    public void StandProcess()
    {
        _isStop = true;
    }

    public void FallingProcess()
    {
        _isStop = true;

    }

    public void DestroyProcess()
    {
        Destroy(gameObject);
    }
}
