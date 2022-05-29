using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial class RotateSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        Entities
            .WithName("RotateSystem")
            .ForEach((Entity entity, ref RotateComponent rotateComponent, ref Rotation rotation) =>
            {
                var rotateZ = quaternion.RotateZ(math.radians(rotateComponent.direction * rotateComponent.rotateSpeed * deltaTime));
                rotation.Value = math.mul(rotation.Value, rotateZ);
            })
            .ScheduleParallel();
    }
}
