using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    GameManager manager;
    DivideManager divideManager;
    GameData data;

    [Header("Definitions")]
    [SerializeField] private GameObject TargetPlatform;
    [SerializeField] private float speed;
    [SerializeField] private float xSensitivity;


    void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        divideManager = FindObjectOfType<DivideManager>();
        data = manager.data;

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
        if (!manager._isGameStarted) return;

        float xMovement = Mathf.Lerp(transform.position.x, TargetPlatform != null ? TargetPlatform.transform.position.x : transform.position.x, Time.deltaTime * xSensitivity);
        float zMovement = speed * Time.deltaTime;

        transform.position = new Vector3(xMovement, transform.position.y, transform.position.z + zMovement);
    }

    private void SetTargetPlatform()
    {
        List<GameObject> allPlatforms = divideManager.AllPlatforms;

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
            TargetPlatform = selectedPlatform;
        }
        else
        {
            TargetPlatform = null;
        }

    }
}
