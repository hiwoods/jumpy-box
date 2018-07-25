using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Entities;

public class UIEventHandlers : MonoBehaviour, IPointerClickHandler {

    private float ScreenMidX;
    private PlayerState PlayerState;

    public void Start()
    {
        ScreenMidX = Screen.width/2f;
        PlayerState = GameObject.Find("Player").GetComponent<PlayerState>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PlayerState == null)
        {
            OnPlayerReset();
        }
        else
        {
            OnPlayerStart(eventData);
        }
    }
    
    private void OnPlayerStart(PointerEventData eventData)
    {
        Vector2 position = eventData.position;

        bool right = position.x > ScreenMidX;

        float angle;
        if (right)
            angle = 40f;
        else
            angle = 140f;

        Bootstrap.GetSystem<PlayerJumpSystem>().MakePlayerJump(angle);
        Bootstrap.GetSystem<UpdateHUDSystem>().UpdateStart();
        enabled = false;
    }

    private void OnPlayerReset()
    {
        Bootstrap.NewGame();
        PlayerState = GameObject.Find("Player").GetComponent<PlayerState>();
    }
}
