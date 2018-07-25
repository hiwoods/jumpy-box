using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[UpdateAfter(typeof(PlayerJumpSystem))]
public class PlayerMoveSystem : ComponentSystem
{
    struct PlayerData
    {
        public Transform Transform;
        public PlayerState PlayerState;
    }

    private float TimeElapsed;
    private float Distance;

    private static float ROTATION_SPEED = 600f;

    protected override void OnUpdate()
    {
        TimeElapsed += Time.deltaTime;

        foreach (var entity in GetEntities<PlayerData>())
        {
            //if player is not jumping, reset all the values
            if (!entity.PlayerState.Jumping)
            {
                TimeElapsed = 0;
                Distance = -1;
                continue;
            }

            Vector3 start = entity.PlayerState.JumpPosition;
            Vector3 des = entity.PlayerState.JumpDestination;

            //find the distance if not set, yet
            if (Distance < 0)
                Distance = Vector3.Distance(start, des);

            ISettings settings = Bootstrap.Settings;
            //this should guarantee the jump is linear and at constant speed.
            float completedDistance = (TimeElapsed * settings.PlayerMoveSpeed)/Distance;

            entity.Transform.position = Vector3.Lerp(start, des, completedDistance);

            //restore rotation once completed
            if (completedDistance >= 1.0f)
            {
                entity.Transform.rotation = Quaternion.identity;
            }
            else
            {
                var direction = start.x > 0 ? Vector3.forward : -Vector3.forward;
                entity.Transform.RotateAround(entity.Transform.position, direction, Time.deltaTime * ROTATION_SPEED);
            }
                
        }
    }
}
