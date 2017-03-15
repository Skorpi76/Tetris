﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static int gridWidth = 10;
    public static int gridHeight = 20;

    public static Transform[,] grid = new Transform[gridWidth, gridHeight];
    public Text hud_score;
    public Text hud_level;
    public Text hud_lines;

    public int scoreOneLine = 40;
    public int scoreTwoLine = 100;
    public int scoreThreeLine = 300;
    public int scoreFourLine = 1200;

    public static bool startingAtLevelZero;
    public static int startingLevel;

    public int currentLevel = 0;
    private int numLinesCleared = 0; 
    public static float fallSpeed = 1.0f; 

    private int numberOfRowsThisTurn = 0;
    public static int currentScore = 0;
    private AudioSource audioSource;
    public AudioClip clearLineSound;
    public AudioClip doubleKill;
    public AudioClip tripleKill;
    public AudioClip monsterKill;
    private GameObject previewTetromino;
    private GameObject nextTetromino;
    private bool gameStarted = false;
    private Vector2 previwTetrominoPosition = new Vector2(16.5f, 12.5f);


    void Start()
    {
        currentLevel = startingLevel;
        SpawnNextTetromino();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        UpdateScore();
        UpdateUI();
        UpdateLevel();
        UpdateSpeed();
        if (Input.GetKey("escape"))
            Application.Quit();
    }

    void UpdateLevel()
    {
        if ((startingAtLevelZero == true) || (startingAtLevelZero == false && numLinesCleared / 10 > startingLevel)) 
        {
            currentLevel = numLinesCleared / 10;
        }
          
      
    }
    void UpdateSpeed()
    {
        fallSpeed = 1.0f - ((float)currentLevel * 0.1f);
      
    }

    public void UpdateUI()
    {
        hud_score.text = currentScore.ToString();
        hud_level.text = currentLevel.ToString();
        hud_lines.text = numLinesCleared.ToString();         

    }
    public void UpdateScore()
    {
        if (numberOfRowsThisTurn > 0)
        {
            if (numberOfRowsThisTurn == 1)
            {
                ClearedOneLine();

            }
            else if (numberOfRowsThisTurn == 2)
            {
                ClearedTwoLines();
            }
            else if (numberOfRowsThisTurn == 3)
            {
                ClearedThreeLines();
            }
            else if (numberOfRowsThisTurn == 4)
            {
                ClearedFourLines();
            }
            numberOfRowsThisTurn = 0;

        }
    }
    public void ClearedOneLine()
    {
        currentScore += scoreOneLine + (currentLevel * 20);
        PlayLineClearedSound();
        numLinesCleared++;
    }
    public void ClearedTwoLines()
    {
        PlayDoubleKill();
        currentScore += scoreTwoLine + (currentLevel * 25);
        numLinesCleared += 2;

    }
    public void ClearedThreeLines()
    {
        PlayTripleKill();
        currentScore += scoreThreeLine + (currentLevel * 30);
        numLinesCleared += 3;

    }
    public void ClearedFourLines()
    {
        PlayMonsterKill();
        currentScore += scoreFourLine + (currentLevel * 50);
        numLinesCleared += 4;

    }
    void PlayLineClearedSound()
    {
        audioSource.PlayOneShot(clearLineSound);
    }
    void PlayDoubleKill()
    {
        audioSource.PlayOneShot(doubleKill);
    }
    void PlayTripleKill()
    {
        audioSource.PlayOneShot(tripleKill);
    }
    void PlayMonsterKill()
    {
        audioSource.PlayOneShot(monsterKill);
    }

    public bool ChechIsAboveGrid(Tetromino tetromino)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            foreach (Transform mino in tetromino.transform)
            {
                Vector2 pos = Round(mino.position);
                if (pos.y > gridHeight - 1)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool IsFullRowAt(int y)
    {
        // The parameter ym is the row we will iterate over in the grid array to check each x position for a transfor.
        for (int x = 0; x < gridWidth; ++x)
        {
            // If we find a position that returns Null instead of a transform, then we know that the row is not full.
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        // Since we found a full row, we increment the full row variable. 
        numberOfRowsThisTurn++;
        // if we iterated over the entire loop and didnt encounter any null positions, that we return true.
        return true;
    }

    public void DeleteMinoAt(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }
    public void MoveRowDown(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += new Vector3(0, -1, 0);

            }
        }
    }
    public void MoveAllRowsDown(int y)
    {
        for (int i = y; i < gridHeight; ++i)
        {
            MoveRowDown(i);
        }
    }
    public void DeleteRow()
    {
        for (int y = 0; y < gridHeight; ++y)
        {
            if (IsFullRowAt(y))
            {
                DeleteMinoAt(y);
                MoveAllRowsDown(y + 1);
                --y;
            }
        }
    }

    public void UpdateGrid(Tetromino tetromino)
    {
        for (int y = 0; y < gridHeight; ++y)
        {
            for (int x = 0; x < gridWidth; ++x)
            {
                if (grid[x, y] != null)
                {
                    if (grid[x, y].parent == tetromino.transform)
                    {
                        grid[x, y] = null;
                    }
                }
            }
        }
        foreach (Transform mino in tetromino.transform)
        {
            Vector2 pos = Round(mino.position);
            if (pos.y < gridHeight)
            {
                grid[(int)pos.x, (int)pos.y] = mino;
            }
        }
    }

    public Transform GetTransformAtGridPosition(Vector2 pos)
    {
        if (pos.y > gridHeight - 1)
        {
            return null;
        }
        else
        {
            return grid[(int)pos.x, (int)pos.y];
        }
    }
    public bool CheckIsInsideGrid(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0);
    }
    public Vector2 Round(Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    public void SpawnNextTetromino()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            nextTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(),
      typeof(GameObject)), new Vector2(5.0f, 20.0f), Quaternion.identity);

            previewTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(),
       typeof(GameObject)), previwTetrominoPosition, Quaternion.identity);

            previewTetromino.GetComponent<Tetromino>().enabled = false;
        }
        else
        {
            previewTetromino.transform.localPosition = new Vector2(5.0f, 20.0f);
            nextTetromino = previewTetromino;
            nextTetromino.GetComponent<Tetromino>().enabled = true;

            previewTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(),
           typeof(GameObject)), previwTetrominoPosition, Quaternion.identity);

            previewTetromino.GetComponent<Tetromino>().enabled = false;
     
        }

    }
    string GetRandomTetromino()
    {
        int randomTetromino = Random.Range(1, 8);
        string randomTetrominoName = "Prefubs/Tetromino_long";
        switch (randomTetromino)
        {
            case 1:
                randomTetrominoName = "Prefubs/Tetromino_long";
                break;
            case 2:
                randomTetrominoName = "Prefubs/Tetromino_Square";
                break;
            case 3:
                randomTetrominoName = "Prefubs/Tetromino_j";
                break;
            case 4:
                randomTetrominoName = "Prefubs/Tetromino_L";
                break;
            case 5:
                randomTetrominoName = "Prefubs/Tetromino_S";
                break;
            case 6:
                randomTetrominoName = "Prefubs/Tetromino_T";
                break;
            case 7:
                randomTetrominoName = "Prefubs/Tetromino_Z";
                break;
        }
        return randomTetrominoName;
    }

    public void GameOver()
    {       
        SceneManager.LoadScene("GameOver");
    }
}
