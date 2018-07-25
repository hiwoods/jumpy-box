using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;

[UpdateAfter(typeof(PlayerInputSystem))]
public class PlayerJumpSystem : ComponentSystem
{
    struct PlayerData
    {
        public int Length;
        public ComponentArray<PlayerState> PlayerState;
        [ReadOnly] public ComponentArray<Transform> Transform;
        [ReadOnly] public ComponentArray<PlayerInput> PlayerInput;
    }

    [Inject] private PlayerData PlayerEntities;

    protected override void OnUpdate()
    {
        PlayerState playerState = PlayerEntities.PlayerState[0];
        PlayerInput playerInput = PlayerEntities.PlayerInput[0];
        Transform playerTransform = PlayerEntities.Transform[0];

        if (playerInput.Jump
            && Bootstrap.GetSystem<AngleOmeterSpawnSystem>().AngleOmeterIsActive)
        {
            MakePlayerJump(null);
        }
        else if (playerState.Jumping && playerTransform.position == playerState.JumpDestination)
        {
            playerState.Jumping = false;
        }
    }

    public void MakePlayerJump(float? angle)
    {
        for (int i = 0; i < PlayerEntities.Length; i++)
        {
            PlayerState playerState = PlayerEntities.PlayerState[i];
            Transform transform = PlayerEntities.Transform[i];

            if (!playerState.Jumping)
            {
                //if angle is not set, get it from RotateSystem
                if (!angle.HasValue)
                {
                    angle = Bootstrap.GetSystem<AngleOmeterRotateSystem>().Angle;
                }

                Vector3 destination = CalculateDestination(transform.position, angle.Value);

                playerState.JumpPosition = transform.position;
                playerState.JumpDestination = destination;

                playerState.Jumping = true;
            }
        }
    }

    private Vector3 CalculateDestination(Vector3 startPosition, float angle)
    {
        ISettings settings = Bootstrap.Settings;

        float distance = Mathf.Abs(startPosition.x) + settings.MaxX;
        float x;
        if (angle < 90)
        {
            x = settings.MaxX;
        }
        else
        {
            x = -settings.MaxX;
            angle = 180 - angle;
        }

        float y = startPosition.y + distance * Mathf.Tan(angle * Mathf.Deg2Rad);

        return new Vector3(x, y, 0);
    }
}
