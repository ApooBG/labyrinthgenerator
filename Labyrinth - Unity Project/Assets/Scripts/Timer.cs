using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public bool startGame;
    [SerializeField] TMPro.TextMeshProUGUI winningText;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject labyrinthCamera;


    float timer;

    // Start is called before the first frame update
    void Start()
    {
        startGame = true;
        timer = 0;   
    }

    // Update is called once per frame
    void Update()
    {
        if (startGame)
        {
            timer += Time.deltaTime;
        }

        if (gameObject.transform.position.y < -5) //if the user is below -5F on Y, the game ends and shows how many seconds it took the player to beat the level.
        {
            GameWon();
        }
    }

    void GameWon() 
    {
        startGame = false;
        winningText.text = Math.Round(timer, 2) + "s";
        pauseMenu.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameObject.transform.Find("First Person Camera").gameObject.SetActive(false);
    }
}
