using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Controller : MonoBehaviour
{
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Public Fields                                                                                                                                                                                * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public TMP_Text UI_Player_HP;
    public TMP_Text UI_Player_CanDash;
    public TMP_Text UI_BossEnemy_HP;
    public TMP_Text UI_BossEnemy_Label;

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Private/Protected Attributes                                                                                                           * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    GameObject Player_GameObject;
    GameObject BossEnemy_GameObject;

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Start Function                                                                                                                                                                               * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Update Function                                                                                                                                                                              * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        if (Player_GameObject == null)
        {
            Player_GameObject = GameObject.FindWithTag("Player");
        } 
        else
        {
            UI_Update_PlayerHP();
            UI_Update_PlayerCanDash();
        }

        if (BossEnemy_GameObject == null)
        {
            BossEnemy_GameObject = GameObject.FindWithTag("Boss Enemy");
        }
        else
        {
            UI_Update_BossEnemy_HP();
        }
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Object Reference Functions                                                                                                                                                                   * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Update UI Element Functions                                                                                                                                                                  * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void UI_Update_PlayerHP()
    {
        if (Player_GameObject.GetComponent<PlayerController>().playerCurrenthealth == 3)
        {
            UI_Player_HP.text = "♥ ♥ ♥";
        }
        else if (Player_GameObject.GetComponent<PlayerController>().playerCurrenthealth == 2)
        {
            UI_Player_HP.text = "♥ ♥  ";
        }
        else if (Player_GameObject.GetComponent<PlayerController>().playerCurrenthealth == 1)
        {
            UI_Player_HP.text = "♥    ";
        }
        else if (Player_GameObject.GetComponent<PlayerController>().playerCurrenthealth == 0)
        {
            UI_Player_HP.text = "     ";
        }
    }

    public void UI_Update_PlayerCanDash()
    {
        if (Player_GameObject.GetComponent<PlayerController>().canDash == true)
        {
            UI_Player_CanDash.text = "*DASH*";
        }
        else
        {
            UI_Player_CanDash.text = "";
        }
    }

    public void UI_Update_BossEnemy_HP()
    {
        float HP_Max = BossEnemy_GameObject.GetComponent<BossEnemy>().HP_ReturnMax();
        float HP_Current = BossEnemy_GameObject.GetComponent<BossEnemy>().HP_ReturnCurrent();
        float HP_Percentage = (HP_Current / HP_Max) * 100f;

        UI_BossEnemy_HP.text = "";

        if (HP_Percentage > 0)
        {
            UI_BossEnemy_HP.text += "♥ ";
        }

        if (HP_Percentage > 10)
        {
            UI_BossEnemy_HP.text += "♥ ";
        }

        if (HP_Percentage > 20)
        {
            UI_BossEnemy_HP.text += "♥ ";
        }

        if (HP_Percentage > 30)
        {
            UI_BossEnemy_HP.text += "♥ ";
        }

        if (HP_Percentage > 40)
        {
            UI_BossEnemy_HP.text += "♥ ";
        }

        if (HP_Percentage > 50)
        {
            UI_BossEnemy_HP.text += "♥ ";
        }

        if (HP_Percentage > 60)
        {
            UI_BossEnemy_HP.text += "♥ ";
        }

        if (HP_Percentage > 70)
        {
            UI_BossEnemy_HP.text += "♥ ";
        }

        if (HP_Percentage > 80)
        {
            UI_BossEnemy_HP.text += "♥ ";
        }

        if (HP_Percentage > 90)
        {
            UI_BossEnemy_HP.text += "♥ ";
        }
    }
}
