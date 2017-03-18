using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class GameMenu : MonoBehaviour {

    public Text levelText;
    public Text highScoreText;
    public Text highScoreText2;
    public Text highScoreText3;
    public Text playerScore;


    void Start () {

        if (SceneManager.GetActiveScene().name == "GameMenu")
        {
            levelText.text = "0";
            //PlayerPrefs.SetInt("highscore", 20);           
        }
        if (SceneManager.GetActiveScene().name == "GameOver")
            playerScore.text = (Game.currentScore - 100 ).ToString();



        highScoreText.text = PlayerPrefs.GetInt("highscore").ToString();
        highScoreText2.text = PlayerPrefs.GetInt("highscore2").ToString();
        highScoreText3.text = PlayerPrefs.GetInt("highscore3").ToString();
    }


   public void PlayGame() {
        if (Game.startingLevel == 0)
            Game.startingAtLevelZero = true;
        else
            Game.startingAtLevelZero = false; 

        SceneManager.LoadScene("level");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("GameMenu");
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void ResetScore()
    {
        PlayerPrefs.SetInt("highscore", 0);
        PlayerPrefs.SetInt("highscore2", 0);
        PlayerPrefs.SetInt("highscore3", 0);

        highScoreText.text = PlayerPrefs.GetInt("highscore").ToString();
        highScoreText2.text = PlayerPrefs.GetInt("highscore2").ToString();
        highScoreText3.text = PlayerPrefs.GetInt("highscore3").ToString();
    }

    public void ChangedValue(float value)
    {
        Game.startingLevel = (int)value;
        levelText.text = value.ToString();
      
    }
}
