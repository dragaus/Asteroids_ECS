using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Jobs;
using UnityEngine;

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
        public ComponentDataFromEntity<BulletData> bulletGroup;
        public ComponentDataFromEntity<AsteroidData> asteroidGroup;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            bool isTargetA = asteroidGroup.HasComponent(entityA);
            bool isTargetB = asteroidGroup.HasComponent(entityB);

            bool isBulletB = bulletGroup.HasComponent(entityB);
            bool isBulletA = bulletGroup.HasComponent(entityA);

            if (isBulletA && isTargetB)
            {
                SetCollisionData(ref entityB, ref entityA);
            }

            if (isTargetA && isBulletB)
            {
                SetCollisionData(ref entityA, ref entityB);
            }
        }
        void SetCollisionData(ref Entity entityA, ref Entity entityB)
        {
            var asteroidData = asteroidGroup[entityA];
            asteroidData.shouldBeDestroy = true;
            asteroidGroup[entityA] = asteroidData;
            var bulletData = bulletGroup[entityB];
            bulletData.hitAnAsteroid = true;
            bulletData.asteroidSize = asteroidData.asteroidSize;
            bulletGroup[entityB] = bulletData;
        }
    }


    protected override void OnUpdate()
    {
        Dependency = new ColllisonEventJob
        {
            bulletGroup = GetComponentDataFromEntity<BulletData>(),
            asteroidGroup = GetComponentDataFromEntity<AsteroidData>()
        }.Schedule(stepPhysicsWorld.Simulation, Dependency);

        Dependency.Complete();
    }
}
