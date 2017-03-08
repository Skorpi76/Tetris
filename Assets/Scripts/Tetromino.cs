using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour {
    float fall = 0;
    public float fallSpeed = 1;

	void Start () {
		
	}
	

	void Update () {
        CheckUserInput();
	}



    void CheckUserInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1f, 0f, 0f);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1f, 0f, 0f);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, 90); 
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - fall >= fallSpeed)
        {                                                                    
            transform.position += new Vector3(0f, -1f, 0f);
            fall = Time.time;
        }
    }
}
