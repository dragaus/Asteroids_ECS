using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial class MovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        Entities
            .WithName("MovementSystem")
            .ForEach((ref MovementData movementComponent, ref Translation position, ref Rotation rotation) =>
            {
                var axis = math.mul(rotation.Value, new float3(0f, 1f, 0f));
                position.Value += axis * movementComponent.movementSpeed * deltaTime;
            })
            .ScheduleParallel();
    }
}
