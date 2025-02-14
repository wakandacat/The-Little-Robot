using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI : MonoBehaviour
{
    PlayerController Health;
    public GameObject player;
    public GameObject bar_3;
    public GameObject bar_2;
    public GameObject bar_1;
    public GameObject bar_0;
    public Image dashBar;



    // Start is called before the first frame update
    void Start()
    {
        Health = player.GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        Health_Bar_display();
        dash_bar_display();
        if (dashBar.fillAmount == 0)
        {
            dashBar.fillAmount += Time.deltaTime;

        }
    }

    public void Health_Bar_display()
    {
        if(Health.playerCurrenthealth == 3)
        {
            bar_3.SetActive(true);
            bar_2.SetActive(false);
            bar_1.SetActive(false);
            bar_0.SetActive(false);
        }
        if (Health.playerCurrenthealth == 2)
        {
            bar_3.SetActive(false);
            bar_2.SetActive(true);
            bar_1.SetActive(false);
            bar_0.SetActive(false);
        }
        if (Health.playerCurrenthealth == 1)
        {
            bar_3.SetActive(false);
            bar_2.SetActive(false);
            bar_1.SetActive(true);
            bar_0.SetActive(false);
        }
        if (Health.playerCurrenthealth == 0)
        {
            bar_3.SetActive(false);
            bar_2.SetActive(false);
            bar_1.SetActive(false);
            bar_0.SetActive(true);
        }
    }
    //https://www.youtube.com/watch?v=ju1dfCpDoF8
    public void dash_bar_display()
    {
        Debug.Log("The player can dash: " + Health.canDash);
        if(Health.canDash == true)
        {
            dashBar.fillAmount = 1;
        }
        else if(Health.canDash == false)
        {
            dashBar.fillAmount = 0;

        }
    }
}
