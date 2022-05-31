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
public partial class PowerUpCollisionSystem : SystemBase
{
    StepPhysicsWorld stepPhysicsWorld;

    protected override void OnCreate()
    {
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    [BurstCompile]
    struct TriggerEventJob : ITriggerEventsJob
    {
        public ComponentDataFromEntity<PlayerData> shipsGroup;
        public ComponentDataFromEntity<PowerUpData> powerUpGroup;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            bool isBodyAShip = shipsGroup.HasComponent(entityA);
            bool isBodyBShip = shipsGroup.HasComponent(entityB);

            if (isBodyAShip && isBodyBShip) return;

            Entity ship = isBodyAShip ? entityA : entityB;

            bool isBodyAPowerUp = powerUpGroup.HasComponent(entityA);
            bool isBodyBPowerUp = powerUpGroup.HasComponent(entityB);
            if ((isBodyAShip && !isBodyBPowerUp) || (isBodyBShip && !isBodyAPowerUp)) { return; }

            Entity powerUp = isBodyAPowerUp ? entityA : entityB;
            
            PowerUpData powerUpData = powerUpGroup[powerUp];
            powerUpData.collected = true;
            powerUpGroup[powerUp] = powerUpData;

            PlayerData playerData = shipsGroup[ship];
            playerData.powerUp = powerUpData.kindOfPowerUp;
            switch (powerUpData.kindOfPowerUp)
            {
                case KindOfPowerUp.Shield:
                    playerData.powerUpTime = 6f;
                    break;
                case KindOfPowerUp.DoubleShoot:
                    playerData.powerUpTime = 20;
                    break;
            }
            shipsGroup[ship] = playerData;
        }
    }

    protected override void OnUpdate()
    {
        Dependency = new TriggerEventJob
        {
            shipsGroup = GetComponentDataFromEntity<PlayerData>(),
            powerUpGroup = GetComponentDataFromEntity<PowerUpData>(),
        }.Schedule(stepPhysicsWorld.Simulation, Dependency);

        Dependency.Complete();
    }

    protected override void OnDestroy()
    {
        stepPhysicsWorld.FinalSimulationJobHandle.Complete();
    }
}