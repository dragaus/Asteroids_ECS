using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[UpdateBefore(typeof(MovementSystem))]
public partial class ShootSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities
            .WithoutBurst()
            .WithName("ShootSystem")
            .WithStructuralChanges()
            .ForEach((ref ShooterData shooter, ref Rotation rotation, ref Translation position) =>
            {
                if (shooter.hasToShoot)
                {
                    var instance = EntityManager.Instantiate(GameDataManager.instance.bulletEntity);
                    EntityManager.SetComponentData(instance, new Translation { Value = position.Value });
                    EntityManager.SetComponentData(instance, new Rotation { Value = rotation.Value });
                    if (!shooter.shootDouble) { return; }
                    var secondBullet = EntityManager.Instantiate(GameDataManager.instance.bulletEntity);
                    EntityManager.SetComponentData(secondBullet, new Translation { Value = position.Value });
                    EntityManager.SetComponentData(secondBullet, new Rotation { Value = math.mul(rotation.Value, quaternion.Euler(0, 0 , 180)) });
                }
            })
            .Run();
    }
}
