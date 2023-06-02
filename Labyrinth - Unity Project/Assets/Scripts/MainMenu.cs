using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class MainMenu : MonoBehaviour
{
    public TMPro.TMP_InputField tb_rows;
    public TMPro.TMP_InputField tb_columns;
    public Button start;

    public GameObject loadingScreen;
    public Slider slider;

    public static int rowsNum;
    public static int columnNum;



    // Start is called before the first frame update
    void Start()
    {
        rowsNum = 10;
        columnNum = 10;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        ManageColumnInput();
    }

    public void ManageRowInput()
    {
        if (tb_rows.text == "")
        {
            return;
        }

        try
        {
            if (Convert.ToInt16(tb_rows.text) > 250)
            {
                tb_rows.text = "250";
                rowsNum = 250;
                return;
            }
        }

        catch
        {

        }

        try
        {
            rowsNum = Convert.ToInt32(tb_rows.text);
        }
        catch
        {
            tb_rows.text = rowsNum.ToString();
        }

        if (tb_rows.text.Length > 3)
        {
            tb_rows.text = tb_rows.text.Substring(0, 3);
        }
    }

    public void ManageColumnInput()
    {
        if (tb_columns.text == "")
        {
            return;
        }

        try
        {
            if (Convert.ToInt16(tb_columns.text) > 250)
            {
                tb_columns.text = "250";
                columnNum = 250;
                return;
            }
        }

        catch
        {

        }

        try
        {
            columnNum = Convert.ToInt32(tb_columns.text);
        }
        catch
        {
            tb_columns.text = columnNum.ToString();
        }

        if (tb_columns.text.Length > 3)
        {
            tb_columns.text = tb_columns.text.Substring(0, 3);
        }
    }

    public void AfterColumnDeselect()
    {
        if (Convert.ToInt16(tb_columns.text) < 10)
        {
            tb_columns.text = "10";
            columnNum = 10;
        }
    }

    public void AfterRowDeselect()
    {
        if (Convert.ToInt16(tb_rows.text) < 10)
        {
            tb_rows.text = "10";
            rowsNum = 10;
        }
    }



    //when start is clicked, the labyrinth gets created
    public void CreateLabyrinth()
    {
        StartCoroutine(LoadLabyrinth());
    }

    //follows the progress of the next loading scene, in this case the labyrinth, and displays it as a loading bar
    IEnumerator LoadLabyrinth()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("LabyrinthScene");

        loadingScreen.SetActive(true);
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            yield return null;
        }
    }

    //quits the application
    public void QuitApplication()
    {
        Application.Quit();
    }
}
