using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class ECSManager : MonoBehaviour
{
    public int initialAmountOfAsteroids;
    [SerializeField] GameObject bigAsteroidPrefab;
    [SerializeField] GameObject mediumAsteroidPrefab;
    [SerializeField] GameObject smallAsteroidPrefab;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject bulletPrefab;

    EntityManager entityManager;
    BlobAssetStore blobAssetStore;

    // Start is called before the first frame update
    void Start()
    {
        blobAssetStore = new BlobAssetStore();
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);

        var playerEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(playerPrefab, settings);
        var bigAsteroidEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(bigAsteroidPrefab, settings);
        var mediumAsteroidEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(mediumAsteroidPrefab, settings);
        var smallAsteroidEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(smallAsteroidPrefab, settings);
        var bulletEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, settings);

        GameDataManager.instance.bigAsteroidEntity = bigAsteroidEntity;
        GameDataManager.instance.mediumAsteroidEntity = mediumAsteroidEntity;
        GameDataManager.instance.smallAsteroidEntity = smallAsteroidEntity;
        GameDataManager.instance.bulletEntity = bulletEntity;

        for (int i = 0; i < initialAmountOfAsteroids; i++)
        {
            var asteroidInstance = entityManager.Instantiate(bigAsteroidEntity);
            var position = new float3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-7, 7), 0);
            float rotationZ = UnityEngine.Random.Range(0f, 360f);

            entityManager.SetComponentData(asteroidInstance, new Translation { Value = position });
            entityManager.SetComponentData(asteroidInstance, new Rotation { Value = quaternion.Euler(0, 0, rotationZ) });
            entityManager.SetComponentData(asteroidInstance, new MovementData { movementSpeed = 2, baseMovementSpeed = 2 });
        }

        entityManager.Instantiate(playerEntity);
    }

    private void OnDestroy()
    {
        blobAssetStore.Dispose();
    }
}
