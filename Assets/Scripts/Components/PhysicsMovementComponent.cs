using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct PhysicsMovementComponent : IComponentData
{
    public float baseMovementSpeed;
    public float movementSpeed;
}
