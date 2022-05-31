using Unity.Entities;

[GenerateAuthoringComponent]
public struct PowerUpData : IComponentData
{
    public KindOfPowerUp kindOfPowerUp;
    public bool collected;
}
