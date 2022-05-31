using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial class ShipSystem : SystemBase
{
    protected override void OnUpdate()
    {
        GameDataManager gameDataManager = GameDataManager.instance;
        float deltaTime = Time.DeltaTime;
        Entities
            .WithoutBurst()
            .WithName("ShipSystem")
            .ForEach((ref PlayerData playerData, ref Translation position, ref DestroyNowData destroyNowData) =>
            {
                if (!playerData.canBeHit)
                {
                    playerData.untochableTime -= deltaTime;
                    playerData.canBeHit = playerData.untochableTime <= 0;
                }

                if (playerData.isHitByAsteroid)
                {
                    int lives = gameDataManager.LooseALife();
                    if (lives <= 0)
                    {
                        destroyNowData.shouldBeDestroy = true;
                    }
                    else
                    {
                        position.Value = float3.zero;
                        playerData.canBeHit = false;
                        playerData.isHitByAsteroid = false;
                        playerData.untochableTime = gameDataManager.untouchableInitialTime;
                    }
                }
            }).Run();
    }
}
