using Unity.Entities;

[GenerateAuthoringComponent()]
public struct PlayerData : IComponentData
{
    public bool isHitByAsteroid;
    public bool canBeHit;
    public float powerUpTime;
    public KindOfPowerUp powerUp;
}
