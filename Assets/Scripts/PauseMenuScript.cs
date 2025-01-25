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

    //public canvas objects
    public GameObject pauseMenu;
    public GameObject controlMenu;
    public GameObject settingsMenu;

    //menu items for event system
    public GameObject pauseFirstButton;
    public GameObject controlsFirstButton;
    public GameObject settingsFirstButton;

    //input system
    public InputActionAsset inputActions;
    private InputActionMap playerControls;
    private InputActionMap menuControls;

    //freelook cam
    public CinemachineFreeLook freeCam;

    void Awake()
    {
        //clear event selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set new default selected
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    private void FindSchemas()
    {
        //find the input schemas
        playerControls = inputActions.FindActionMap("Gameplay");
        menuControls = inputActions.FindActionMap("UI");
    }

    //pause the game
    public void PauseGame()
    {

        FindSchemas();

        //if we are currently unpaused
        if (GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPaused == false)
        {

            //switch input systems so they don't overlap
            playerControls.Disable();
            menuControls.Enable();

            //pause the game
            pauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPaused = true;

            //clear event selected object
            EventSystem.current.SetSelectedGameObject(null);
            //set new default selected
            EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        }
        else //if we are currently paused
        {

            //switch schemas
            playerControls.Enable();
            menuControls.Disable();

            //unpause the game
            pauseMenu.SetActive(false);
            Time.timeScale = 1.0f;
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPaused = false;
        }
    }

    //continue button
    public void ContinueGame()
    {
        FindSchemas();
        
        //switch schemas
        playerControls.Enable();
        menuControls.Disable();

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

    //view the settings menu
    public void GoToSettings()
    {
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
