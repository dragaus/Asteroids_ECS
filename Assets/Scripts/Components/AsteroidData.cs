using Unity.Entities;

[GenerateAuthoringComponent]
public struct AsteroidData : IComponentData
{
    public AsteroidSizes asteroidSize;
    public bool shouldBeDestroy;
}
