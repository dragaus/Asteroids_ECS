using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIManager : MonoBehaviour
{
    static UIManager m_instance;
    public static UIManager instance { get => m_instance; }

    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text livesText;
    [SerializeField] TMP_Text instructionText;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            m_instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        scoreText.gameObject.SetActive(false);
        livesText.gameObject.SetActive(false);
        instructionText.gameObject.SetActive(true);
    }

    public void ShowGameUI() 
    {
        scoreText.gameObject.SetActive(true);
        livesText.gameObject.SetActive(true);
        instructionText.gameObject.SetActive(false);
    }

    public void UpddateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void UpdateLives(int lives)
    {
        livesText.text = $"lifes: {lives}";
    }

    public void GameOver()
    {
        scoreText.gameObject.SetActive(false);
        livesText.gameObject.SetActive(false);
        instructionText.gameObject.SetActive(true);
        instructionText.text = "Game over press enter to replay";
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            m_instance = null;
        }
    }
}
