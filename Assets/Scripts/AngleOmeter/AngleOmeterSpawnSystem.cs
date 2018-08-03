using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;


public class AngleOmeterSpawnSystem : ComponentSystem
{
    private struct AngleOmeterSpawnData
    {
        public Vector3 Position;
        public float RateOfChangeMultiplier;
    }

    struct PlayerData
    {
        public int Length;
        [ReadOnly] public ComponentArray<Transform> Transforms;
        [ReadOnly] public ComponentArray<PlayerState> PlayerStates;
    }

    [Inject] private PlayerData PlayerEntities;

    private GameObject SpawnedOmeter;

    protected override void OnUpdate()
    {
        Transform playerTransform = PlayerEntities.Transforms[0];
        PlayerState playerState = PlayerEntities.PlayerStates[0];
        ISettings settings = Bootstrap.Settings;

        if (playerTransform.position.x == 0f 
            || playerState.Jumping
            || playerState.IsDead
            || !Mathf.Approximately(playerTransform.position.y, settings.MinY))
        {
            HideAngleOmeter();
        }
        else if (!AngleOmeterIsActive)
        {
            SpawnOmeter(new AngleOmeterSpawnData()
            {
                Position = playerTransform.position,
                RateOfChangeMultiplier = 1.1f,
            });
        }
    }

    public bool AngleOmeterIsActive => SpawnedOmeter != null && SpawnedOmeter.activeSelf;

    public void ResetOmeter()
    {
        if (SpawnedOmeter == null)
            return;

        ISettings settings = Bootstrap.Settings;
        AngleOmeterData data = SpawnedOmeter.GetComponent<AngleOmeterData>();

        data.StartAngle = settings.MinAngle;
        data.EndAngle = settings.MaxAngle;
        data.RotationSpeed = settings.RotationSpeed;
    }

    private void SpawnOmeter(AngleOmeterSpawnData data)
    {
        if (SpawnedOmeter == null)
        {
            SpawnedOmeter = InstantiateOmeter();
        }

        UpdateOmeter(SpawnedOmeter, data);
        SpawnedOmeter.SetActive(true);
    }

    private GameObject InstantiateOmeter()
    {
        ISettings settings = Bootstrap.Settings;
        GameObject ometerPrefab = settings.OmeterPrefab;

        if (!ometerPrefab)
        {
            throw new UnityException("Failed to instantiate ometer.");
        }

        //create object and set angles
        GameObject meterObject =  Object.Instantiate(ometerPrefab);

        //initialize data values
        ResetOmeter();

        return meterObject;
    }


    private static void UpdateOmeter(GameObject ometer, AngleOmeterSpawnData data)
    {
        AngleOmeterData ometerData = ometer.GetComponent<AngleOmeterData>();
        ISettings settings = Bootstrap.Settings;

        ometerData.FacingRight = data.Position.x < 0;
        ometerData.StartAngle = ometerData.FacingRight ? settings.MinAngle : 180 - settings.MinAngle;

        Quaternion rotation = Quaternion.AngleAxis(ometerData.StartAngle, Vector3.forward);

        ometer.transform.rotation = rotation;
        ometer.transform.position = data.Position;

        //rotation speed
        ometerData.RotationSpeed *= data.RateOfChangeMultiplier;
        ometerData.RotationSpeed = Mathf.Clamp(ometerData.RotationSpeed, ometerData.RotationSpeed, settings.MaxRotationSpeed);

        //flip end angle if needed
        if (!Mathf.Approximately(settings.MaxAngle, 90f))
        {
            float diff = Mathf.Abs(settings.MaxAngle - 90);
            ometerData.EndAngle = ometerData.FacingRight ? 90 - diff : 90 + diff;
        }
    }

    private void HideAngleOmeter()
    {
        if (SpawnedOmeter != null)
            SpawnedOmeter.SetActive(false);
    }
}
