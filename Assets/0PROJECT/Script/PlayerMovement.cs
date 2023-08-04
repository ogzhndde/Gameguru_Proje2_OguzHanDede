using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerManager pManager;



    void Awake()
    {
        pManager = GetComponent<PlayerManager>();
    }

    void Start()
    {
        InvokeRepeating(nameof(SetTargetPlatform), 0.5f, 0.5f);
    }


    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (!pManager.manager._isGameStarted) return;

        var TargetPlatform = pManager.structMovement.TargetPlatform;

        float xMovement = Mathf.Lerp(transform.position.x,
                                        TargetPlatform != null ?
                                        TargetPlatform.transform.position.x : transform.position.x,
                                        Time.deltaTime * pManager.structMovement.xSensitivity);
        float zMovement = pManager.structMovement.speed * Time.deltaTime;

        transform.position = new Vector3(xMovement, transform.position.y, transform.position.z + zMovement);
    }

    private void SetTargetPlatform()
    {
        List<GameObject> allPlatforms = pManager.divideManager.AllPlatforms;

        float closestDistance = Mathf.Infinity;
        GameObject selectedPlatform = null;

        for (int i = 0; i < allPlatforms.Count; i++)
        {
            if (allPlatforms[i].transform.position.z > transform.position.z) //EGER PLATFORM BENIM ILERIMDE ISE
            {
                float distance = allPlatforms[i].transform.position.z - transform.position.z;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    selectedPlatform = allPlatforms[i];
                }
            }
        }

        if (selectedPlatform != null)
        {
            pManager.structMovement.TargetPlatform = selectedPlatform;
        }
        else
        {
            pManager.structMovement.TargetPlatform = null;
        }

    }
}
