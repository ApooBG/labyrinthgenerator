using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject player;

    bool pausedGame;

    // Start is called before the first frame update
    void Start()
    {
        pausedGame = false;
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausedGame)
                ResumeGame();
            else
                PauseGame();
        }
    }

    //timer stops and the pause menu appears
    void PauseGame()
    {
        pauseMenu.SetActive(true);
        pausedGame = true;
        Time.timeScale = 0;
        player.GetComponent<Timer>().startGame = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        player.SetActive(false);
    }

    //timer continues and the pause menu gets hidden
    void ResumeGame()
    {
        pauseMenu.SetActive(false);
        pausedGame = false;
        Time.timeScale = 1;
        player.GetComponent<Timer>().startGame = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        player.SetActive(true);
    }

    //restarts the game
    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

  

}
