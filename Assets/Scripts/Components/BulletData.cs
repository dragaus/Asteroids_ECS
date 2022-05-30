using Unity.Entities;

[GenerateAuthoringComponent]
public struct BulletData : IComponentData
{
    public bool hitAnAsteroid;
    public AsteroidSizes asteroidSize;
}
