using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;

public class SceneScrollSystem : ComponentSystem
{
    struct PlayerData
    {
        public int Length;
        public ComponentArray<Transform> Transforms;
        [ReadOnly] public ComponentArray<PlayerState> PlayerStates;
    }

    [Inject] private PlayerData PlayerEntities;

    struct ScrollableObjectData
    {
        public int Length;
        public ComponentArray<Transform> Transforms;
        public ComponentArray<ScrollData> ScrollDatas;
    }

    [Inject] private ScrollableObjectData ScrollableEntities;

    private float TimeElapsed;
    private float DistanceToScroll;
    private bool DoLerp;

    protected override void OnUpdate()
    {
        Transform playerTransform = PlayerEntities.Transforms[0];
        PlayerState playerState = PlayerEntities.PlayerStates[0];

        ISettings settings = Bootstrap.Settings;

        if (playerState.IsDead 
            || playerState.Jumping 
            || Mathf.Approximately(playerTransform.position.y, settings.MinY))
            return;
        
        if (DoLerp)
        {
            TimeElapsed += Time.deltaTime;
            LerpScollableObjects();
        }
        else
        {
            DistanceToScroll = Mathf.Abs(playerTransform.position.y - settings.MinY);
            UpdateScollComponents(DistanceToScroll);
            DoLerp = true;
            TimeElapsed = 0;
        }

    }

    private void UpdateScollComponents(float distance)
    {
        for (int i = 0; i < ScrollableEntities.Length; i++)
        {
            Transform entityTransform = ScrollableEntities.Transforms[i];
            ScrollData scrollData = ScrollableEntities.ScrollDatas[i];

            scrollData.StartPosition = entityTransform.position;
            scrollData.EndPosition = scrollData.StartPosition - (new Vector3(0, distance, 0));
        }
    }

    private void LerpScollableObjects()
    {
        float fracCompleted = (TimeElapsed * Bootstrap.Settings.ScrollSpeed) / DistanceToScroll;

        List<GameObject> objToDestroy = new List<GameObject>();

        for (int i = 0; i < ScrollableEntities.Length; i++)
        {
            Transform entityTransform = ScrollableEntities.Transforms[i];
            ScrollData scrollData = ScrollableEntities.ScrollDatas[i];
            
            entityTransform.position = Vector3.Lerp(scrollData.StartPosition, scrollData.EndPosition, fracCompleted);
        }

        //stop once completed
        if (fracCompleted >= 1.0f)
            DoLerp = false;
    }
}
