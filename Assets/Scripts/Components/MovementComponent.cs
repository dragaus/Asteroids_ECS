using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct MovementComponent : IComponentData
{
    public float baseMovementSpeed;
    public float movementSpeed;
}
