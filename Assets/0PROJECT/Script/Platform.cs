using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using Random = UnityEngine.Random;

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

    
    [Space(10)]
    [Header("Others")]
    public GameObject PlatformCollectable = null;


    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);
        SpawnCollectable();
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

    //#########################      MATERIALS    #########################
    public void SetRandomMaterial()
    {
        MaterialIndex = Random.Range(0, AllMaterials.Count);
        GetComponent<MeshRenderer>().material = AllMaterials[MaterialIndex];
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

    public void PerfectShootProcess()
    {
        gameObject.LeanScale(transform.localScale * 1.1f, 0.3f).setEasePunch();
    }



    //#########################      COLLECTABLES    #########################

    public void SpawnCollectable()
    {
        if (Random.value < 0.8f) return; //%20 PERCENT SPAWN COLLECTABLE
        if(_isStop) return;

        var collList = GameManager.Instance.Collectables;

        GameObject collectable = Instantiate(collList[Random.Range(0, collList.Count)], transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        PlatformCollectable = collectable;

        collectable.GetComponent<Collectable>().SetTarget(transform);
    }
}
