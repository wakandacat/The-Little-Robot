using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuScript : MonoBehaviour
{
    //main menu references: https://www.youtube.com/watch?v=zc8ac_qUXQY and https://www.youtube.com/watch?v=SXBgBmUcTe0&t=1s

    //public canvas objects
    public GameObject mainMenu;
    public GameObject controlMenu;

    //menu items for event system
    public GameObject mainFirstButton;
    public GameObject controlsFirstButton;

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
        controlMenu.SetActive(true);

        //clear event selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set new default selected
        EventSystem.current.SetSelectedGameObject(controlsFirstButton);

    }

    //view the main menu
    public void BackToMainMenu()
    {

        mainMenu.SetActive(true);
        controlMenu.SetActive(false);

        //clear event selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set new default selected
        EventSystem.current.SetSelectedGameObject(mainFirstButton);

    }

}
