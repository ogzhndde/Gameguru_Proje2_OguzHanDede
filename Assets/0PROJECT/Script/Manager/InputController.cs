using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour, IPointerDownHandler
{
    private GameManager manager;
    private DivideManager divideManager;
    private GameData data;

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        data = manager.data;
    }


    void Update()
    {

    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (!manager._isGameStarted)
        {
            EventManager.Broadcast(GameEvent.OnStart);
            return;
        }

        EventManager.Broadcast(GameEvent.OnDivide);

    }

}
