using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MainMenuScript : MonoBehaviour
{
    //main menu references: https://www.youtube.com/watch?v=zc8ac_qUXQY and https://www.youtube.com/watch?v=SXBgBmUcTe0&t=1s

    //input system
    public PlayerControls pc;

    //public canvas objects
    public GameObject mainMenu;
    public GameObject controlMenu;
    public GameObject settingsMenu;

    //menu items for event system
    public GameObject mainFirstButton;
    public GameObject mainSecondButton;
    public GameObject mainThirdButton;

    private static GameObject lastMainButton; //static


    public GameObject controlsFirstButton;
    public GameObject settingsFirstButton;

    //game setting values
    public GameObject gameSettings;

    void Awake()
    {
        DisableMouse();
        pc = new PlayerControls(); 
        pc.UI.Enable(); //set up input system

        //clear event selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set new default selected
        EventSystem.current.SetSelectedGameObject(mainFirstButton);

        pc.UI.Cancel.performed += GoBack;
    }


    //B button pressed in menus
    public void GoBack(InputAction.CallbackContext context)
    {

        //if the main menu is not active
        if (mainMenu.activeSelf == false)
        {
            //return to the menu
            BackToMainMenu();
        }

    }

    //play button
    public void PlayGame()
    {
        SceneManager.LoadScene("BaseScene");
    }

    //exit button
    public void QuitGame()
    {
        Debug.Log("Game Closed");
        Application.Quit();
    }

    //view the control menu
    public void GoToControls()
    {
        lastMainButton = mainThirdButton;

        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        controlMenu.SetActive(true);

        //clear event selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set new default selected
        EventSystem.current.SetSelectedGameObject(controlsFirstButton);

    }

    //view the settings menu
    public void GoToSettings()
    {
        lastMainButton = mainSecondButton;

        mainMenu.SetActive(false);
        controlMenu.SetActive(false);
        settingsMenu.SetActive(true);

        //clear event selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set new default selected
        EventSystem.current.SetSelectedGameObject(settingsFirstButton);

    }

    //adjust the camera sensitivity
    public void AdjustCamSens(float sens)
    {
        gameSettings.GetComponent<GameSettings>().freelookSens = sens;
    }

    //reset the camera sensitivity
    public void ResetCamSens()
    {
        settingsMenu.GetComponentsInChildren<Slider>()[0].value = 0.5f;
        gameSettings.GetComponent<GameSettings>().freelookSens = 0.5f;
    }

    //adjust the camera sensitivity
    public void AdjustVolume(float vol)
    {
        AudioListener.volume = vol * 2; //main menu audiolistener is seperate so chnage it as well
        gameSettings.GetComponent<GameSettings>().gameVolume = vol; //default vol is 1 (0.5 on slider)
    }

    //reset the camera sensitivity
    public void ResetVolume()
    {
        settingsMenu.GetComponentsInChildren<Slider>()[1].value = 0.5f;
        gameSettings.GetComponent<GameSettings>().gameVolume = 0.5f;
    }

    //view the main menu
    public void BackToMainMenu()
    {

        mainMenu.SetActive(true);
        controlMenu.SetActive(false);
        settingsMenu.SetActive(false);

        //clear event selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set new default selected
        EventSystem.current.SetSelectedGameObject(lastMainButton);

    }
    public void DisableMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Mouse.current.MakeCurrent();
        InputSystem.DisableDevice(Mouse.current);
    }

    private void OnDestroy()
    {
        pc.UI.Cancel.performed -= GoBack;
    }
}
