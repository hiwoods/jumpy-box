using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;

[UpdateAfter(typeof(AngleOmeterSpawnSystem))]
public class AngleOmeterRotateSystem : ComponentSystem
{
    public float Angle => GetCurrentAngle();
    public bool HasStopped { get; set; }

    struct AngleData
    {
        [ReadOnly] public AngleOmeterData Data;
        public Transform Transform;
    }

    protected override void OnUpdate()
    {
        float dt = Time.deltaTime;

        foreach (var entity in GetEntities<AngleData>())
        {
            Vector3 euler = entity.Transform.rotation.eulerAngles;
            float addValue = entity.Data.RotationSpeed * dt;
            addValue = entity.Data.FacingRight ? addValue : -addValue;

            euler.z += addValue;
            
            // if facing right, start angle > end angle and vice versa
            if (entity.Data.FacingRight)
            {
                euler.z = Mathf.Clamp(euler.z, entity.Data.StartAngle, entity.Data.EndAngle);
            }
            else
            {
                euler.z = Mathf.Clamp(euler.z, entity.Data.EndAngle, entity.Data.StartAngle);
            }

            HasStopped = Mathf.Approximately(euler.z, entity.Data.EndAngle);
            entity.Transform.eulerAngles = euler;
        }
    }

    private float GetCurrentAngle()
    {
        var ometerArray = GetEntities<AngleData>();

        if (ometerArray.Length == 0)
        {
            throw new UnityException("No ometer entity exists.");
        }

        return ometerArray[0].Transform.rotation.eulerAngles.z;
    }
}
