using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class ECSManager : MonoBehaviour
{
    public int initialAmountOfAsteroids;
    [SerializeField] GameObject asteroidPrefab;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject bulletPrefab;

    EntityManager entityManager;

    // Start is called before the first frame update
    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);

        var playerEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(playerPrefab, settings);
        var asteroidEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(asteroidPrefab, settings);
        var bulletEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, settings);

        for (int i = 0; i < initialAmountOfAsteroids; i++)
        {
            var asteroidInstance = entityManager.Instantiate(asteroidEntity);
            var position = new float3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-7, 7), 0);
            entityManager.SetComponentData(asteroidInstance, new Translation { Value = position });
            entityManager.SetComponentData(asteroidInstance, new Rotation { Value = new quaternion(0, 0, 0, 0) });

        }

        entityManager.Instantiate(playerEntity);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
