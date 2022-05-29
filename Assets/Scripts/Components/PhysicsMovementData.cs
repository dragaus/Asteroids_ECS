using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct PhysicsMovementData : IComponentData
{
    public float baseMovementSpeed;
    public float movementSpeed;
}
