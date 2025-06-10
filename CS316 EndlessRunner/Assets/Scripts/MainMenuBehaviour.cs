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
        /* Unpause the game if needed */
        Time.timeScale = 1;

        // check for a high score and set it to our TMProUGUI
        GetAndDisplayScore();

        // slide in the Panel with all the goodies
        SlideMenuIn(controlPanel);
    }
    public void SlideMenuIn(GameObject obj)
    {
        obj.SetActive(true);

        var rt = obj.GetComponent<RectTransform>();

        if (rt)
        {
            var pos = rt.position;
            pos.x = -Screen.width / 2;
            rt.position = pos;

            var tween = LeanTween.moveX(rt, 0, 1.5f).setEase(LeanTweenType.easeInOutExpo);
            tween.setIgnoreTimeScale(true);
        }
    }
    public void SlideMenuOut(GameObject obj)
    {
        var rt = obj.GetComponent<RectTransform>();
        if (rt)
        {
            var tween = LeanTween.moveX(rt, Screen.width / 2, 0.5f);
            tween.setEase(LeanTweenType.easeOutQuad);
            tween.setIgnoreTimeScale(false);
            tween.setOnComplete(() =>
            {
                obj.SetActive(false);
            });
        }
    }
    private void GetAndDisplayScore()
    {
        highScoreText.text = "High Score : " + PlayerPrefs.GetInt("score").ToString();
    }
}
