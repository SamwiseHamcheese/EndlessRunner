using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuBehaviour : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    public GameObject controlPanel; //12th chapters

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
    public void ResetScore()
    {
        PlayerPrefs.SetInt("score", 0);
        GetAndDisplayScore();
    }
    private void Start()
    {
        // check for a high score and set it to our TMProUGUI
        GetAndDisplayScore();

    }
    private void GetAndDisplayScore()
    {
        highScoreText.text = "High Score : " + PlayerPrefs.GetInt("score").ToString();
    }
}
