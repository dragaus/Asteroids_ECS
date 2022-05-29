using Unity.Entities;
using Unity.Transforms;

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
                }
            })
            .Run();
    }
}
