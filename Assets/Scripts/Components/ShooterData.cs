using Unity.Entities;

[GenerateAuthoringComponent]
public struct ShooterData : IComponentData
{
    public bool hasToShoot;
    public bool canShoot;
    public float shootSpeed;
    public bool shootDouble;
}
