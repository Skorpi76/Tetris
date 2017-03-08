using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    float fall = 0;
    public float fallSpeed = 1;
    public bool allowRotation = true;
    public bool limitRotation = false;

    void Start()
    {

    }


    void Update()
    {
        CheckUserInput();
    }



    void CheckUserInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            transform.position += new Vector3(1f, 0f, 0f);
            if (!CheckIsValidPosition())
            {
                transform.position += new Vector3(-1, 0, 0);
            }
            else
            {
                FindObjectOfType<Game>().UpdateGrid(this);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1f, 0f, 0f);
            if (!CheckIsValidPosition())
            {
                transform.position += new Vector3(1, 0, 0);
            }
            else
            {
                FindObjectOfType<Game>().UpdateGrid(this);
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (allowRotation)
            {
                if (limitRotation)
                {
                    if (transform.rotation.eulerAngles.z >= 90)
                    {
                        transform.Rotate(0, 0, -90);
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                }
                if (!CheckIsValidPosition())
                {
                    if (limitRotation)
                    {
                        if (transform.rotation.eulerAngles.z >= 90)
                        {
                            transform.Rotate(0, 0, -90);
                        }
                        else
                        {
                            transform.Rotate(0, 0, 90);
                        }
                    }
                    else
                    {
                        transform.Rotate(0, 0, -90);
                    }

                }
                else
                {
                    FindObjectOfType<Game>().UpdateGrid(this);
                }

            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - fall >= fallSpeed)
        {
            transform.position += new Vector3(0f, -1f, 0f);
            fall = Time.time;
            if (!CheckIsValidPosition())
            {
                transform.position += new Vector3(0, 1, 0);
                FindObjectOfType<Game>().DeleteRow();
                enabled = false;
                FindObjectOfType<Game>().SpawnNextTetromino();
            }
            else
            {
                FindObjectOfType<Game>().UpdateGrid(this);
            }
        }
    }
    bool CheckIsValidPosition()
    {
        foreach (Transform mino in transform)
        {
            Vector2 pos = FindObjectOfType<Game>().Round(mino.position);
            if (FindObjectOfType<Game>().CheckIsInsideGrid(pos) == false)
            {
                return false;
            }
            if (FindObjectOfType<Game>().GetTransformAtGridPosition(pos) != null && FindObjectOfType<Game>().GetTransformAtGridPosition(pos).parent != transform)
            {
                return false;
            }

        }
        return true;

    }
}
