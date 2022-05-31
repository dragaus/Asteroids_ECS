using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[UpdateAfter(typeof(MovementSystem))]
[UpdateAfter(typeof(RotateSystem))]
public partial class CreateSpecials : SystemBase
{
    int timeToCreateSpecial = 15;
    float currentTimeToCreateSpecial;
    protected override void OnUpdate()
    {
        if(GameDataManager.instance.isPlaying)
        {
            currentTimeToCreateSpecial -= Time.DeltaTime;
            if (currentTimeToCreateSpecial <= 0)
            {
                currentTimeToCreateSpecial = UnityEngine.Random.Range(5, timeToCreateSpecial);
                int index = UnityEngine.Random.Range(0, 2);

                var entity = GameDataManager.instance.shieldEntity;

                switch (index)
                {
                    case 1:
                        entity = GameDataManager.instance.doubleShootEntity;
                        break;
                }

                var instance = EntityManager.Instantiate(entity);
                EntityManager.SetComponentData(instance, new Translation
                {
                    Value = new float3(
                    UnityEngine.Random.Range(-GameDataManager.instance.xLimit, GameDataManager.instance.xLimit),
                    UnityEngine.Random.Range(-GameDataManager.instance.yLimit, GameDataManager.instance.yLimit),
                    0f)
                });
            }
        }
    }
}
