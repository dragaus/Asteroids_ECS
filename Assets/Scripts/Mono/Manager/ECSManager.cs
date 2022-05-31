using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using Unity.Scenes;
using Unity.Mathematics;

public class ECSManager : MonoBehaviour
{
    InputAction enterAction;

    public int initialAmountOfAsteroids;
    [SerializeField] GameObject bigAsteroidPrefab;
    [SerializeField] GameObject mediumAsteroidPrefab;
    [SerializeField] GameObject smallAsteroidPrefab;

    [SerializeField] GameObject shieldPowerUpPrefab;
    [SerializeField] GameObject doubleShootPowerUpPrefab;

    [SerializeField] GameObject destroyAsteroidParticle;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject bulletPrefab;

    EntityManager entityManager;
    BlobAssetStore blobAssetStore;

    bool shouldInitialize = true;

    public static Vector2 GetScreenSize()
    {
        var camera = Camera.main;
        if (camera == null)
        {
            Debug.LogWarning("You need a cemera to perform this action");
            return Vector2.zero;
        }

        return new Vector2(camera.orthographicSize * Screen.width / Screen.height, camera.orthographicSize) * (camera.transform.position.z / -10);
    }

    // Start is called before the first frame update
    void Start()
    {
        enterAction = new InputAction();
        enterAction.AddCompositeBinding("Axis")
            .With("Positive", "<Keyboard>/Enter");
        enterAction.Enable();
        enterAction.performed += (context) =>
        {
            if (shouldInitialize)
            {
                Initialize();
                shouldInitialize = false;
                return;
            }

            if (GameDataManager.instance.lives <= 0)
            {

                return;
            }
        };
    }

    void Initialize()
    {
        blobAssetStore = new BlobAssetStore();
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);

        var playerEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(playerPrefab, settings);
        var bigAsteroidEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(bigAsteroidPrefab, settings);
        var mediumAsteroidEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(mediumAsteroidPrefab, settings);
        var smallAsteroidEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(smallAsteroidPrefab, settings);
        var bulletEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, settings);
        var shieldEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(shieldPowerUpPrefab, settings);
        var doubleShootEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(doubleShootPowerUpPrefab, settings);
        var destroyAsteroidEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(destroyAsteroidParticle, settings);

        GameDataManager.instance.bigAsteroidEntity = bigAsteroidEntity;
        GameDataManager.instance.mediumAsteroidEntity = mediumAsteroidEntity;
        GameDataManager.instance.smallAsteroidEntity = smallAsteroidEntity;
        GameDataManager.instance.bulletEntity = bulletEntity;
        GameDataManager.instance.destroyAsteroidEntity = destroyAsteroidEntity;
        GameDataManager.instance.shieldEntity = shieldEntity;
        GameDataManager.instance.doubleShootEntity = doubleShootEntity;

        entityManager.Instantiate(doubleShootEntity);

        var cameraSize = GetScreenSize();
        GameDataManager.instance.xLimit = cameraSize.x;
        GameDataManager.instance.yLimit = cameraSize.y;

        for (int i = 0; i < initialAmountOfAsteroids; i++)
        {
            var asteroidInstance = entityManager.Instantiate(bigAsteroidEntity);
            var position = new float3(UnityEngine.Random.Range(-cameraSize.x, cameraSize.x), UnityEngine.Random.Range(-cameraSize.y, cameraSize.y), 0);
            float rotationZ = UnityEngine.Random.Range(0f, 360f);

            entityManager.SetComponentData(asteroidInstance, new Translation { Value = position });
            entityManager.SetComponentData(asteroidInstance, new Rotation { Value = quaternion.Euler(0, 0, rotationZ) });
            entityManager.SetComponentData(asteroidInstance, new MovementData { movementSpeed = 2, baseMovementSpeed = 2 });
        }

        var playerInstance = entityManager.Instantiate(playerEntity);
        entityManager.SetComponentData(playerInstance, new PlayerData { canBeHit = false, powerUpTime = GameDataManager.instance.untouchableInitialTime, powerUp = KindOfPowerUp.Shield });

        UIManager.instance.ShowGameUI();
    }

    private void OnDestroy()
    {
        if (blobAssetStore != null)
        { 
            blobAssetStore.Dispose();
        }
    }
}
