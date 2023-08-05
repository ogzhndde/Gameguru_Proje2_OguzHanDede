using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerManager pManager;

    void Awake()
    {
        pManager = GetComponent<PlayerManager>();

        //CHECK CLOSEST PLATFORM TO PLAYER TO MOVE TO IT
        InvokeRepeating(nameof(SetTargetPlatform), 0.5f, 0.5f);
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (!pManager.manager._isGameStarted) return;

        //CHECK TARGET PLATFORM FROM PLAYER MANAGER
        var TargetPlatform = pManager.structMovement.TargetPlatform;

        //SET X AND Z VALUES TO MOVE SMOOTHLY
        float xMovement = Mathf.Lerp(transform.position.x,
                                        TargetPlatform != null ?TargetPlatform.transform.position.x : transform.position.x,
                                        Time.deltaTime * pManager.structMovement.xSensitivity);
        float zMovement = pManager.structMovement.speed * Time.deltaTime;

        //SET POSITION
        transform.position = new Vector3(xMovement, transform.position.y, transform.position.z + zMovement);
    }

    private void SetTargetPlatform()
    {
        //RELIST ALL PLATFORMS FROM DIVIDE MANAGER
        List<GameObject> allPlatforms = pManager.divideManager.AllPlatforms;

        float closestDistance = Mathf.Infinity;
        GameObject selectedPlatform = null;

        //CHECK LIST TO FIND CLOSEST PLATFORM
        for (int i = 0; i < allPlatforms.Count; i++)
        {
            if (allPlatforms[i].transform.position.z - 1f > transform.position.z)
            {
                float distance = allPlatforms[i].transform.position.z - transform.position.z;

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    selectedPlatform = allPlatforms[i];
                }
            }
        }

        //FIND CLOSEST ONE AND SET IT AS TARGET, IF THERE IS NO TARGET, SET IT NULL
        pManager.structMovement.TargetPlatform = selectedPlatform ? selectedPlatform : null;
    }
}
