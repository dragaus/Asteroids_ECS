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
            .ForEach((ref PlayerData playerData, ref Translation position,ref ShooterData shooterData, ref DestroyNowData destroyNowData) =>
            {
                if (playerData.powerUp != KindOfPowerUp.None)
                {
                    playerData.powerUpTime -= deltaTime;

                    switch (playerData.powerUp)
                    {
                        case KindOfPowerUp.Shield:
                            shooterData.shootDouble = false;
                            playerData.canBeHit = playerData.powerUpTime <= 0;
                            break;
                        case KindOfPowerUp.DoubleShoot:
                            playerData.canBeHit = true;
                            shooterData.shootDouble = playerData.powerUpTime > 0;
                            break;
                    }

                    if (playerData.powerUpTime <= 0)
                    {
                        playerData.powerUp = KindOfPowerUp.None;
                    }
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
                        playerData.powerUpTime = gameDataManager.untouchableInitialTime;
                    }
                }
            }).Run();
    }
}
