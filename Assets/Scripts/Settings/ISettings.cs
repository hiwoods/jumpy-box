using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISettings
{
    GameObject OmeterPrefab { get; }
    GameObject PlayerPrefab { get; }
    GameObject BasePrefab { get; }
    float MaxX { get; }
    float MinY { get; }
    float Deadline { get; }
    float ScrollSpeed { get; }
    float PlayerMoveSpeed { get; }
    float MinAngle { get; }
    float MaxAngle { get; }
    float RotationSpeed { get; }
    float MaxRotationSpeed { get; }
}
