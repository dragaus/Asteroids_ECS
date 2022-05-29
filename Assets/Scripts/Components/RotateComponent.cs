using Unity.Entities;

[GenerateAuthoringComponent]
public struct RotateComponent : IComponentData
{
    public float rotateSpeed;
    public float direction;
}
