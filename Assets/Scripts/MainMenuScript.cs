using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    //main menu references: https://www.youtube.com/watch?v=zc8ac_qUXQY and https://www.youtube.com/watch?v=SXBgBmUcTe0&t=1s


    //play button
    public void PlayGame()
    {
        SceneManager.LoadScene("BaseScene");
    }

    //controls button -> to show the control scheme or any other instructions

    //exit button
    public void QuitGame()
    {
        Debug.Log("Game Closed");
        Application.Quit();
    }

}
