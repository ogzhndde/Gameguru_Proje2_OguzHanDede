using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour, IPointerDownHandler
{
    private GameManager manager;
    private GameData data;

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        data = manager.data;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!manager._isGameStarted)
        {
            //IF GAME IS NOT STARTED YET, CALL THIS EVENT
            EventManager.Broadcast(GameEvent.OnStart);
            return;
        }

        //IF GAME STARTED, CALL DIVIDE EVENT ON CLICK
        EventManager.Broadcast(GameEvent.OnDivide);
    }

}
