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
        //SPAWN COLLECTABLES WITH A DELAY
        SpawnCollectable();
    }

    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        if (_isStop) return;

        //MOVE THE PLATFORM RIGHT TO LEFT IN A LOOP
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

    //###############     MATERIAL METHODS, SET NEW MATERIALS IN A LOOP    ##################
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

    //ACTIONS TO BE TAKEN WHEN SPAWNED
    public void SpawnProcess(Direction comingSide)
    {
        _isStop = false;

        directionEnum = comingSide == Direction.Left ? Direction.Right : Direction.Left;

        SetMaterial();
    }

    //ACTIONS ON STANDING PLATFORM
    public void StandProcess()
    {
        _isStop = true;
    }

    //ACTIONS ON FALLING PLATFORM
    public void FallingProcess()
    {
        _isStop = true;

        Rigidbody rb = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        rb.useGravity = true;

        Destroy(gameObject, 3f);
    }

    //ACTIONS ON PLATFORMS THAT SHOULD BE DISAPPEARED
    public void DestroyProcess()
    {
        Destroy(gameObject);
    }


    //ACTIONS ON PERFECT SHOOT
    public void PerfectShootProcess()
    {
        gameObject.LeanScale(transform.localScale * 1.1f, 0.3f).setEasePunch();

        //SPAWN A PARTICLE ON PERFECT SHOOT
        CreatePerfectParticle();
    }



    //#########################      COLLECTABLES    #########################
    public void SpawnCollectable()
    {
        if (Random.value < 0.8f) return; //%20 PERCENT SPAWN COLLECTABLE
        if (_isStop) return; //IF PLATFORM IS STABLE, DONT SPAWN COLLECTABLE

        var collList = GameManager.Instance.Collectables;

        //SPAWN RANDOM COLLECTABLE
        GameObject collectable = Instantiate(collList[Random.Range(0, collList.Count)], transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        PlatformCollectable = collectable;

        //SET THIS PLATFORM AS COLLECTABLE TARGET
        collectable.GetComponent<Collectable>().SetTarget(transform);
    }


    private void CreatePerfectParticle()
    {
        //FIND BOUND OF THE PLATFORM 
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        Vector3 surfaceNormalWorld = -transform.forward;

        Vector3 surfaceSizeWorld = meshRenderer.bounds.size;

        Vector3 surfaceCenterWorld = meshRenderer.bounds.center;
        Vector3 surfaceSizeLocal = transform.InverseTransformVector(surfaceSizeWorld);

        //DETECT THE EDGE BACK TO THE Z AXIS
        if (Mathf.Abs(surfaceSizeLocal.z) > 0)
            surfaceCenterWorld += surfaceNormalWorld * (surfaceSizeWorld.z * 0.5f);
        else
            surfaceCenterWorld -= surfaceNormalWorld * (surfaceSizeWorld.z * 0.5f);

        //SPAWN PARTICLE
        var perfectParticle = Instantiate(Resources.Load("PerfectParticle") as GameObject, surfaceCenterWorld, Quaternion.identity);
    }


}
