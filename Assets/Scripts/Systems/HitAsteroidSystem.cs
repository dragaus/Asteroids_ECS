using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

[UpdateAfter(typeof(MovementSystem))]
[UpdateAfter(typeof(RotateSystem))]
public partial class HitAsteroidSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities
            .WithName("HitAsteroidSystem")
            .ForEach((Entity entity, ref BulletData bulletData, ref LifeTimeData lifeTimeData, ref Translation position) =>
            {
                if (bulletData.hitAnAsteroid)
                {
                    lifeTimeData.lifeTime = 0;
                }
            }).ScheduleParallel();
    }
}
