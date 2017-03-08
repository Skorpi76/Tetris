using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSystem : MonoBehaviour {

    public void PlayAgain()
    {
        Application.LoadLevel("level");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
