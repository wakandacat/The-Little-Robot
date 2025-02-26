using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{

    public GameObject world;

    //public canvas objects
    public GameObject pauseMenu;
    public GameObject currentMenu;
    public GameObject settingsMenu;

    //menu items for event system
    public GameObject pauseFirstButton;
    public GameObject pauseSecondButton;
    public GameObject pauseThirdButton;

    private static GameObject lastPauseButton; //static so all instance of class have the same one

    public GameObject controlsFirstButton;
    public GameObject settingsFirstButton;

    //freelook cam
    public CinemachineFreeLook freeCam;

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
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SwitchActionMap("UI");

        //pause the game
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPaused = true;

        //clear event selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set new default selected
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    public void UnPause()
    {
        //unpause the game
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPaused = false;
    }

    public void backButton()
    {
        //if the main pause menu is not active
        if (pauseMenu.activeSelf == false)
        {
            //return to the menu
            BackToPauseMenu();
        }
    }

    //continue button
    public void ContinueGame()
    {

        //switch schemas
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SwitchActionMap("Gameplay");

        //unpause the game
        pauseMenu.SetActive(false);
        currentMenu.SetActive(false);
        Time.timeScale = 1.0f;
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPaused = false;

    }


    //view the control menu
    public void GoToControls()
    {
        lastPauseButton = pauseThirdButton;

        pauseMenu.SetActive(false);
        currentMenu.SetActive(true);

        //clear event selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set new default selected
        EventSystem.current.SetSelectedGameObject(controlsFirstButton);
    }

    //view the pause menu
    public void BackToPauseMenu()
    {     
        pauseMenu.SetActive(true);
        currentMenu.SetActive(false);
        settingsMenu.SetActive(false);

        //clear event selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set new default selected
        EventSystem.current.SetSelectedGameObject(lastPauseButton);
    }

    //view the main menu
    public void BackToMainMenu()
    {
        Time.timeScale = 1.0f;
        lastPauseButton = null;
        SceneManager.LoadScene("MainMenu");
    }

    //view the settings menu
    public void GoToSettings()
    {
        lastPauseButton = pauseSecondButton;

        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);

        //clear event selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set new default selected
        EventSystem.current.SetSelectedGameObject(settingsFirstButton);
    }

    //exit button
    public void QuitGame()
    {
        Debug.Log("Game Closed");
        Application.Quit();
    }

    //adjust the camera sensitivity
    public void AdjustCamSens(float sens)
    {
        freeCam.m_XAxis.m_MaxSpeed = 300 * sens; //middle is 150
        freeCam.m_YAxis.m_MaxSpeed = 4 * sens; //middle is 2

    }

    //reset the camera sensitivity
    public void ResetCamSens()
    {
        freeCam.m_XAxis.m_MaxSpeed = 150; //middle is 150
        freeCam.m_YAxis.m_MaxSpeed = 2; //middle is 2
        settingsMenu.GetComponentInChildren<Slider>().value = 0.5f;
    }
}
