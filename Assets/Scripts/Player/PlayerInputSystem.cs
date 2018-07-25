using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class PlayerInputSystem : ComponentSystem
{
    struct PlayerData
    {
        public PlayerInput PlayerInput;
    }

    protected override void OnUpdate()
    {
        foreach (var entity in GetEntities<PlayerData>())
        {
            PlayerInput input = entity.PlayerInput;
            input.Jump = Input.GetButtonDown("Jump0");
        }
    }
}
