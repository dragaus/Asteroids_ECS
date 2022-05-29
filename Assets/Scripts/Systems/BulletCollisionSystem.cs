using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Jobs;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(BuildPhysicsWorld))]
[UpdateBefore(typeof(StepPhysicsWorld))]
public partial class BulletCollisionSystem : SystemBase
{
    StepPhysicsWorld stepPhysicsWorld;

    protected override void OnCreate()
    {
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    [BurstCompile]
    struct ColllisonEventJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<BulletData> bulletGroup;
        public ComponentDataFromEntity<DestroyNowData> destroyables;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            bool isTargetA = destroyables.HasComponent(entityA);
            bool isTargetB = destroyables.HasComponent(entityB);

            bool isBulletA = destroyables.HasComponent(entityA);
            bool isBulletB = destroyables.HasComponent(entityB);

            if (isBulletA && isTargetB)
            {
                var destroyData = destroyables[entityB];
                destroyData.shouldBeDestroy = true;
                destroyables[entityB] = destroyData;
            }

            if (isTargetA && isBulletB)
            {
                var destroyData = destroyables[entityA];
                destroyData.shouldBeDestroy = true;
                destroyables[entityA] = destroyData;
            }
        }
    }

    protected override void OnUpdate()
    {
        Dependency = new ColllisonEventJob
        {
            bulletGroup = GetComponentDataFromEntity<BulletData>(),
            destroyables = GetComponentDataFromEntity<DestroyNowData>()
        }.Schedule(stepPhysicsWorld.Simulation, Dependency);

        Dependency.Complete();
    }
}