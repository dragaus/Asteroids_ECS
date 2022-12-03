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
    public Entity shieldEntity;
    public Entity doubleShootEntity;

    public float xLimit;
    public float yLimit;

    public float untouchableInitialTime;

    public int lives;
    public int score;
    public int destroyCount;

    public bool isPlaying;

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

    public void DestroyAsteroid(AsteroidSizes sizes) 
    {
        int scoreToAdd = 0;
        switch (sizes)
        {
            case AsteroidSizes.Big:
                scoreToAdd = 20;
                break;            
            case AsteroidSizes.Medium:
                scoreToAdd = 50;
                break;            
            case AsteroidSizes.Small:
                scoreToAdd = 100;
                destroyCount++;
                break;
            case AsteroidSizes.Alien:
                scoreToAdd = 100;
                break;
        }
        score += scoreToAdd;
        UIManager.instance.UpddateScore(score);
    }

    public bool ShouldCreateAnotherAsteroid() 
    { 
        return destroyCount % 4 == 0 && destroyCount > 0;
    }

    public int LooseALife()
    {
        lives--;
        UIManager.instance.UpdateLives(lives);
        if (lives <= 0)
        {
            UIManager.instance.GameOver();
        }
        return lives;
    }

    private void OnDestroy()
    {
        if (instance == this) 
        {
            m_instance = null;
        }
    }
}
