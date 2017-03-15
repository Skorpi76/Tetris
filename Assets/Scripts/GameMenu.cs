using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class GameMenu : MonoBehaviour {

    public Text levelText;

    
	void Start () {
       // if (Application.loadedLevelName == "GameMenu")
       if(SceneManager.GetActiveScene().name == "GameMenu")
        levelText.text = "0";
	}


   public void PlayGame() {
        if (Game.startingLevel == 0)
            Game.startingAtLevelZero = true;
        else
            Game.startingAtLevelZero = false; 

        SceneManager.LoadScene("level");
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void ChangedValue(float value)
    {
        Game.startingLevel = (int)value;
        levelText.text = value.ToString();
      
    }
}
