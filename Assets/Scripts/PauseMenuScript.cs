using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenuScript : MonoBehaviour
{

    //public canvas objects
    public GameObject pauseMenu;
    public GameObject controlMenu;

    //menu items for event system
    public GameObject pauseFirstButton;
    public GameObject controlsFirstButton;


    void Awake()
    {
        //clear event selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set new default selected
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    //pause the game
    public void PauseGame()
    {
        if (GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPaused == false)
        {
            //pause the game
            pauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPaused = true;

            //clear event selected object
            EventSystem.current.SetSelectedGameObject(null);
            //set new default selected
            EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        }
        else
        {
            //unpause the game
            pauseMenu.SetActive(false);
            Time.timeScale = 1.0f;
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPaused = false;
        }
    }

    //continue button
    public void ContinueGame()
    {
        //unpause the game
        pauseMenu.SetActive(false);
        controlMenu.SetActive(false);
        Time.timeScale = 1.0f;
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPaused = false;
    }


    //view the control menu
    public void GoToControls()
    {
        pauseMenu.SetActive(false);
        controlMenu.SetActive(true);

        //clear event selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set new default selected
        EventSystem.current.SetSelectedGameObject(controlsFirstButton);
    }

    //view the pause menu
    public void BackToPauseMenu()
    {
        pauseMenu.SetActive(true);
        controlMenu.SetActive(false);

        //clear event selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set new default selected
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    //view the main menu
    public void BackToMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    //exit button
    public void QuitGame()
    {
        Debug.Log("Game Closed");
        Application.Quit();
    }
}
