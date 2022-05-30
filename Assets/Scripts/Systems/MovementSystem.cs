using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial class MovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        float xLimit = GameDataManager.instance.xLimit;
        float yLimit = GameDataManager.instance.yLimit;

        Entities
            .WithName("MovementSystem")
            .ForEach((ref MovementData movementComponent, ref Translation position, ref Rotation rotation) =>
            {
                var axis = math.mul(rotation.Value, new float3(0f, 1f, 0f));
                if ((axis.x > 0 && position.Value.x >= xLimit) || (axis.x < 0 && position.Value.x <= -xLimit ))
                {
                    position.Value.x *= -1;
                }

                if ((axis.y > 0 && position.Value.y >= yLimit) || (axis.y < 0 && position.Value.y <= -yLimit))
                {
                    position.Value.y *= -1;
                }
                position.Value += axis * movementComponent.movementSpeed * deltaTime;
            })
            .ScheduleParallel();
    }
}
