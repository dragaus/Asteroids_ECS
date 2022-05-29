using Unity.Entities;

[GenerateAuthoringComponent]
public struct ShooterComponent : IComponentData
{
    public bool hasToShoot;
    public bool canShoot;
    public float shootSpeed;
}
