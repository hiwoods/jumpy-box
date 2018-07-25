using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [HideInInspector] public bool IsDead;
    [HideInInspector] public bool Jumping;
    [HideInInspector] public Vector3 JumpPosition;
    [HideInInspector] public Vector3 JumpDestination;
}
