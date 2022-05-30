using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[UpdateAfter(typeof(MovementSystem))]
[UpdateAfter(typeof(RotateSystem))]
public partial class HitAsteroidSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities
            .WithoutBurst()
            .WithName("HitAsteroidSystem")
            .WithStructuralChanges()
            .ForEach((Entity entity, ref BulletData bulletData, ref LifeTimeData lifeTimeData, ref Translation position) =>
            {
                if (bulletData.hitAnAsteroid)
                {
                    bulletData.hitAnAsteroid = false;
                    lifeTimeData.lifeTime = 0;
                    for (int i = 0; i < 30; i++)
                    {
                        var instance = EntityManager.Instantiate(GameDataManager.instance.destroyAsteroidEntity);
                        EntityManager.SetComponentData(instance, new Rotation
                        {
                            Value = quaternion.Euler(
                            UnityEngine.Random.Range(0f, 360f),
                            UnityEngine.Random.Range(0f, 360f),
                            UnityEngine.Random.Range(0f, 360f))
                        });
                        EntityManager.SetComponentData(instance, new Translation { Value = position.Value });
                    }
                    if (bulletData.asteroidSize != AsteroidSizes.Small)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            var asterdoidEntity = bulletData.asteroidSize == AsteroidSizes.Big ? GameDataManager.instance.mediumAsteroidEntity : GameDataManager.instance.smallAsteroidEntity;
                            var instance = EntityManager.Instantiate(asterdoidEntity);
                            EntityManager.SetComponentData(instance, new Translation { Value = position.Value });
                            EntityManager.SetComponentData(instance, new Rotation
                            {
                                Value = quaternion.Euler(0, 0, UnityEngine.Random.Range(0f, 360f))
                            });
                        }
                    }

                }
            }).Run();
    }
}
