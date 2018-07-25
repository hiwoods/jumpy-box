using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSettings : MonoBehaviour, ISettings
{
    public GameObject OmeterPrefab;
    public GameObject PlayerPrefab;
    public GameObject BasePrefab;

    public float MaxX;
    public float MinY;

    public float ScrollSpeed;
    public float PlayerMoveSpeed;
    public float Deadline;

    public float MinAngle;
    public float MaxAngle;
    public float RotationSpeed;
    public float MaxRotationSpeed;

    GameObject ISettings.OmeterPrefab => OmeterPrefab;
    GameObject ISettings.PlayerPrefab => PlayerPrefab;
    GameObject ISettings.BasePrefab => BasePrefab;

    float ISettings.MaxX => MaxX;
    float ISettings.MinY => MinY;
    float ISettings.ScrollSpeed => ScrollSpeed;
    float ISettings.PlayerMoveSpeed => PlayerMoveSpeed;
    float ISettings.Deadline => Deadline;

    float ISettings.MinAngle => MinAngle;
    float ISettings.MaxAngle => MaxAngle;
    float ISettings.RotationSpeed => RotationSpeed;
    float ISettings.MaxRotationSpeed => MaxRotationSpeed;
}
