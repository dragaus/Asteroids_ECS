using Unity.Entities;

[UpdateAfter(typeof(DestroySystem))]
[UpdateAfter(typeof(HitAsteroidSystem))]
public partial class LifeTimeSystem : SystemBase
{
    EndSimulationEntityCommandBufferSystem buffer;

    protected override void OnCreate()
    {
        buffer = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        EntityCommandBuffer.ParallelWriter ecb = buffer.CreateCommandBuffer().AsParallelWriter();
        float deltaTime = Time.DeltaTime;

        Entities
            .WithName("LifeTimeSystem")
            .ForEach((Entity entity, int entityInQueryIndex, ref LifeTimeData lifeTimeData) => 
            {
                lifeTimeData.lifeTime -= deltaTime;
                if (lifeTimeData.lifeTime <= 0)
                {
                    ecb.DestroyEntity(entityInQueryIndex, entity);
                }
            })
            .ScheduleParallel();

        buffer.AddJobHandleForProducer(this.Dependency);
    }
}
