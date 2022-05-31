using Unity.Entities;

public partial class PowerUpSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities
            .WithName("RotateSystem")
            .ForEach((ref PowerUpData powerUpData, ref LifeTimeData lifeTimeData) =>
            {
                if (powerUpData.collected)
                { 
                    lifeTimeData.lifeTime = 0;
                }
            })
            .ScheduleParallel();
    }
}
