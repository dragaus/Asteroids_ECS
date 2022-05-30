using UnityEngine;
using Unity.Entities;

public class GameDataManager : MonoBehaviour
{
    static GameDataManager m_instance;
    public static GameDataManager instance { get => m_instance; }

    public Entity bigAsteroidEntity;
    public Entity mediumAsteroidEntity;
    public Entity smallAsteroidEntity;
    public Entity destroyAsteroidEntity;
    public Entity bulletEntity;

    public float xLimit;
    public float yLimit;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            m_instance = this;
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
            m_instance = null;
        }
    }
}
