using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIManager : MonoBehaviour
{
    static UIManager m_instance;
    public static UIManager instance { get => m_instance; }

    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text lifesText;
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
        lifesText.gameObject.SetActive(false);
        instructionText.gameObject.SetActive(true);
    }

    public void ShowGameUI() 
    {
        scoreText.gameObject.SetActive(true);
        lifesText.gameObject.SetActive(true);
        instructionText.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            m_instance = null;
        }
    }
}
