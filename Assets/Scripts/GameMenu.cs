using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GameMenu : MonoBehaviour {

    public Text levelText;
	// Use this for initialization
	void Start () {
        levelText.text = "0";
	}


   public void PlayGame() {
        if (Game.startingLevel == 0)
            Game.startingAtLevelZero = true;
        else
            Game.startingAtLevelZero = false; 

        Application.LoadLevel("level");
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
