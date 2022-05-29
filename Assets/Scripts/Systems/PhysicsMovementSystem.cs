using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;

public partial class PhysicsMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        Entities
            .WithName("PhysicsMovementSystem")
            .ForEach((ref PhysicsMovementComponent movementComponent, ref PhysicsVelocity physics ,ref Translation position, ref Rotation rotation) =>
            {
                var axis = math.mul(rotation.Value, new float3(0f, 1f, 0f));
                physics.Linear += movementComponent.movementSpeed * deltaTime * axis;
            })
            .ScheduleParallel();
    }
}


