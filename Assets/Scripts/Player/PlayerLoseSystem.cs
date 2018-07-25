using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[UpdateAfter(typeof(PlayerMoveSystem))]
public class PlayerLoseSystem : ComponentSystem
{
    struct PlayerData
    {
        public int Length;
        public ComponentArray<Transform> Tranforms;
        public ComponentArray<Rigidbody2D> Rigidbodies2D;
        public ComponentArray<PlayerState> PlayerStates;
    }

    [Inject] private PlayerData PlayerEntities;

    protected override void OnUpdate()
    {
        var playerTransform = PlayerEntities.Tranforms[0];
        var playerRigidbody = PlayerEntities.Rigidbodies2D[0];
        var playerState = PlayerEntities.PlayerStates[0];
        
        ISettings settings = Bootstrap.Settings;

        if (!playerState.Jumping 
            && Bootstrap.GetSystem<AngleOmeterSpawnSystem>().AngleOmeterIsActive
            && Bootstrap.GetSystem<AngleOmeterRotateSystem>().HasStopped)
        {
            playerState.IsDead = true;
            playerRigidbody.simulated = true;

            //push player away from the wall
            Vector2 direction = Mathf.Approximately(playerTransform.position.x, settings.MaxX)
                ? Vector2.left 
                : Vector2.right;

            playerRigidbody.AddForce(200 * direction);
            playerRigidbody.AddTorque(-10 * direction.x);
        }
    }
}
