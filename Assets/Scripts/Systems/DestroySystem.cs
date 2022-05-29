using Unity.Entities;

public partial class DestroySystem : SystemBase
{
    EndSimulationEntityCommandBufferSystem buffer;

    protected override void OnCreate()
    {
        buffer = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        EntityCommandBuffer.ParallelWriter ecb = buffer.CreateCommandBuffer().AsParallelWriter();

        Entities
            .WithName("DestroySystem")
            .ForEach((Entity entity, int entityInQueryIndex, ref DestroyNowData destroyNow) =>
            {
                if (destroyNow.shouldBeDestroy)
                {
                    ecb.DestroyEntity(entityInQueryIndex, entity);
                }
            })
            .ScheduleParallel();

        buffer.AddJobHandleForProducer(Dependency);
    }
}
