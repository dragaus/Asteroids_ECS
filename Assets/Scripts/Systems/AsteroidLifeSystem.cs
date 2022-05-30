using Unity.Entities;
using Unity.Transforms;

[UpdateAfter(typeof(MovementSystem))]
[UpdateAfter(typeof(RotateSystem))]
public partial class AsteroidLifeSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities
            .WithName("HitAsteroidSystem")
            .ForEach((Entity entity, ref AsteroidData asteroidData, ref DestroyNowData destroyNowData, ref Translation position) =>
            {
                if (asteroidData.shouldBeDestroy)
                {
                    destroyNowData.shouldBeDestroy = true;
                }
            }).ScheduleParallel();
    }
}
