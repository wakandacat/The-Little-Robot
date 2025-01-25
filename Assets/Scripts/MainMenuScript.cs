using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    //main menu references: https://www.youtube.com/watch?v=zc8ac_qUXQY and https://www.youtube.com/watch?v=SXBgBmUcTe0&t=1s

    //public canvas objects
    public GameObject mainMenu;
    public GameObject controlMenu;
    public GameObject settingsMenu;

    //menu items for event system
    public GameObject mainFirstButton;
    public GameObject controlsFirstButton;
    public GameObject settingsFirstButton;

    //game setting values
    public GameObject gameSettings;

    void Awake()
    {
        //clear event selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set new default selected
        EventSystem.current.SetSelectedGameObject(mainFirstButton);
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
        settingsMenu.GetComponentInChildren<Slider>().value = 0.5f;
        gameSettings.GetComponent<GameSettings>().freelookSens = 0.5f;
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
        EventSystem.current.SetSelectedGameObject(mainFirstButton);

    }

}
