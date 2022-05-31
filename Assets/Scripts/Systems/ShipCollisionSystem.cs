using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Transforms;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(EndFramePhysicsSystem))]
public partial class ShipCollisionSystem : SystemBase
{
    static EntityManager entityManager;
    StepPhysicsWorld stepPhysicsWorld;

    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    [BurstCompile]
    struct TriggerEventJob : ITriggerEventsJob
    {
        public ComponentDataFromEntity<PlayerData> shipsGroup;
        [ReadOnly] public ComponentDataFromEntity<AsteroidData> AsteroidsGroup;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            bool isBodyAShip = shipsGroup.HasComponent(entityA);
            bool isBodyBShip = shipsGroup.HasComponent(entityB);

            if (isBodyAShip && isBodyBShip) return;

            bool isBodyAAsteriod = AsteroidsGroup.HasComponent(entityA);
            bool isBodyBAsteroid = AsteroidsGroup.HasComponent(entityB);

            if ((isBodyAShip && !isBodyBAsteroid) || (isBodyBShip && !isBodyAAsteriod)) return;

            var ship = isBodyAShip ? entityA : entityB;

            var playerData = shipsGroup[ship];
            if (playerData.canBeHit)
            { 
                playerData.isHitByAsteroid = true;
                shipsGroup[ship] = playerData;
            }
        }
    }

    protected override void OnUpdate()
    {
        Dependency = new TriggerEventJob
        {
            shipsGroup = GetComponentDataFromEntity<PlayerData>(),
            AsteroidsGroup = GetComponentDataFromEntity<AsteroidData>()
        }.Schedule(stepPhysicsWorld.Simulation, Dependency);

        Dependency.Complete();
    }

    protected override void OnDestroy()
    {
        stepPhysicsWorld.FinalSimulationJobHandle.Complete();
    }
}
