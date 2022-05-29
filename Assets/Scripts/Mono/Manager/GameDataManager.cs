using UnityEngine;
using Unity.Entities;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;

    public Entity bigAsteroidEntity;
    public Entity mediumAsteroidEntity;
    public Entity smallAsteroidEntity;
    public Entity bulletEntity;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this) 
        {
            instance = null;
        }
    }
}
