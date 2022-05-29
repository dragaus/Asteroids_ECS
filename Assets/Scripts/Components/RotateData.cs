using Unity.Entities;

[GenerateAuthoringComponent]
public struct RotateData : IComponentData
{
    public float rotateSpeed;
    public float direction;
}
