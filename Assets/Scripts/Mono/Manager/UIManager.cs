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
        ShowInitialText();
    }

    public void ShowInitialText()
    {
        scoreText.gameObject.SetActive(false);
        livesText.gameObject.SetActive(false);
        instructionText.gameObject.SetActive(true);
        instructionText.text = "Press Enter to play";
    }

    public void ShowGameUI() 
    {
        UpddateScore(0);
        UpdateLives(GameDataManager.instance.lives);
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
        instructionText.text = $"Game over you win {scoreText.text} points";
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            m_instance = null;
        }
    }
}
