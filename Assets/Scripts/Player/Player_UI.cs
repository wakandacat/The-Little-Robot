using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI : MonoBehaviour
{
    PlayerController Health;
    public GameObject player;
    public GameObject full_bar;
    public GameObject bar_4;
    public GameObject bar_3;
    public GameObject bar_2;
    public GameObject bar_1;
    public GameObject bar_0;
    public Image dashBar;
    public float dashTimer = 0.0f;
    private Coroutine refillDashBar;



    // Start is called before the first frame update
    void Start()
    {
        Health = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log("death state is " + Health.deathState);
        Health_Bar_display();
        if (Health.deathState == true)
        {
            dashBar.fillAmount = 1.0f;
        }
    }

    public void Health_Bar_display()
    {
        if (Health.playerCurrenthealth == 5)
        {
            full_bar.SetActive(true);
            bar_4.SetActive(false);
            bar_3.SetActive(false);
            bar_2.SetActive(false);
            bar_1.SetActive(false);
            bar_0.SetActive(false);
        }
        if (Health.playerCurrenthealth == 4)
        {
            full_bar.SetActive(false);
            bar_4.SetActive(true);
            bar_3.SetActive(false);
            bar_2.SetActive(false);
            bar_1.SetActive(false);
            bar_0.SetActive(false);
        }
        if (Health.playerCurrenthealth == 3)
        {
            full_bar.SetActive(false);
            bar_4.SetActive(false);
            bar_3.SetActive(true);
            bar_2.SetActive(false);
            bar_1.SetActive(false);
            bar_0.SetActive(false);
        }
        if (Health.playerCurrenthealth == 2)
        {
            full_bar.SetActive(false);
            bar_4.SetActive(false);
            bar_3.SetActive(false);
            bar_2.SetActive(true);
            bar_1.SetActive(false);
            bar_0.SetActive(false);
        }
        if (Health.playerCurrenthealth == 1)
        {
            full_bar.SetActive(false);
            bar_4.SetActive(false);
            bar_3.SetActive(false);
            bar_2.SetActive(false);
            bar_1.SetActive(true);
            bar_0.SetActive(false);
        }
        if (Health.playerCurrenthealth == 0)
        {
            full_bar.SetActive(false);
            bar_4.SetActive(false);
            bar_3.SetActive(false);
            bar_2.SetActive(false);
            bar_1.SetActive(false);
            bar_0.SetActive(true);
        }
    }
    //https://www.youtube.com/watch?v=ju1dfCpDoF8
    public void dash_Bar()
    {
        if (Health.canDash == false)
        {
            dashBar.fillAmount = 0.0f;
            refillDashBar = StartCoroutine(refillBar());
        }
    }
    private IEnumerator refillBar()
    {
        while (Health.canDash == false)
        {
            dashBar.fillAmount += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}