using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct MovementData : IComponentData
{
    public float baseMovementSpeed;
    public float movementSpeed;
}
