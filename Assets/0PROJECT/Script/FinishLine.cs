using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private GameObject StablePlatform;
    [SerializeField] private GameObject NextLevelPlatform;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                EventManager.Broadcast(GameEvent.OnFinish);
                break;
        }
    }
}
