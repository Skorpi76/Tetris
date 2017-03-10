using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    float fall = 0;
    public float fallSpeed = 1;
    public bool allowRotation = true;
    public bool limitRotation = false;
    public int individualScore = 100;
    private float individualScoreTime;

    private float continiusVerticalSpeed = 0.05f;
    private float continiousHorizontalSpeed = 0.1f;
    private float verticalTimer = 0;
    private float horizontalTimer = 0;

    private float buttonDownWaitMax = 0.2f;
    private float buttonDownWaitTimer = 0;

    private bool moveImmediatHorizontal = false;
    private bool moveImmediatVertical = false;
    public AudioClip moveSound;
    public AudioClip rotateSound;
    public AudioClip landSound;

    private AudioSource audioSourse;

    void Start()
    {
        audioSourse = GetComponent<AudioSource>();
    }


    void Update()
    {
        CheckUserInput();
        UpdateIndividualScore();
    }

    void UpdateIndividualScore()
    {
        if (individualScoreTime < 1)
        {
            individualScoreTime += Time.deltaTime;
        }
        else
        {
            individualScoreTime = 0;
            individualScore = Mathf.Max(individualScore - 10, 0);
        }

    }

    void CheckUserInput()
    {

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            moveImmediatHorizontal = false;
            moveImmediatVertical = false;
            horizontalTimer = 0;
            verticalTimer = 0;
            buttonDownWaitTimer = 0;
          
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (moveImmediatHorizontal)
            {
                if (buttonDownWaitTimer < buttonDownWaitMax)
                {
                    buttonDownWaitTimer += Time.deltaTime;
                    return;
                }

                if (horizontalTimer < continiousHorizontalSpeed)
                {
                    horizontalTimer += Time.deltaTime;
                    return;
                }
            }
            if (!moveImmediatHorizontal)
            {
                moveImmediatHorizontal = true;
            }
            horizontalTimer = 0;



            transform.position += new Vector3(1f, 0f, 0f);

            if (!CheckIsValidPosition())
            {
                transform.position += new Vector3(-1, 0, 0);
            }
            else
            {
                FindObjectOfType<Game>().UpdateGrid(this);
                PlayMoveAudio();
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (moveImmediatHorizontal)
            {

                if (buttonDownWaitTimer < buttonDownWaitMax)
                {
                    buttonDownWaitTimer += Time.deltaTime;
                    return;
                }
                if (horizontalTimer < continiousHorizontalSpeed)
                {
                    horizontalTimer += Time.deltaTime;
                    return;
                }
            }
            if (!moveImmediatHorizontal)
            {
                moveImmediatHorizontal = true;
            }
            horizontalTimer = 0;



            transform.position += new Vector3(-1f, 0f, 0f);
            if (!CheckIsValidPosition())
            {
                transform.position += new Vector3(1, 0, 0);
            }
            else
            {
                FindObjectOfType<Game>().UpdateGrid(this);
                PlayMoveAudio();
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
                    PlayRotateAudio();
                }

            }
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Time.time - fall >= fallSpeed)
        {
            if (moveImmediatVertical)
            {
                if (buttonDownWaitTimer < buttonDownWaitMax)
                {
                    buttonDownWaitTimer += Time.deltaTime;
                    return;
                }
                if (verticalTimer < continiusVerticalSpeed)
                {
                    verticalTimer += Time.deltaTime;
                    return;
                }
            }
            if (!moveImmediatVertical)
            {
                moveImmediatVertical = true;
            }
            verticalTimer = 0;


            transform.position += new Vector3(0f, -1f, 0f);
            fall = Time.time;
            if (!CheckIsValidPosition())
            {
                transform.position += new Vector3(0, 1, 0);
                FindObjectOfType<Game>().DeleteRow();

                if (FindObjectOfType<Game>().ChechIsAboveGrid(this))
                {
                    FindObjectOfType<Game>().GameOver();
                }

                PlayLandAudio();
                FindObjectOfType<Game>().SpawnNextTetromino();
                Game.currentScore += individualScore;
                enabled = false;
            }
            else
            {
                FindObjectOfType<Game>().UpdateGrid(this);
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    PlayMoveAudio();
                }
            }
        }
    }


    void PlayMoveAudio()
    {
        audioSourse.PlayOneShot(moveSound);
    }
    void PlayRotateAudio()
    {
        audioSourse.PlayOneShot(rotateSound);

    }
    void PlayLandAudio()
    {
        audioSourse.PlayOneShot(landSound);
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

