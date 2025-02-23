using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_fx_behaviors : MonoBehaviour
{
    audioManager m_audio;
    private GameObject enemy;
    public Light[] eyes;

    // Start is called before the first frame update
    void Start()
    {
        m_audio = GameObject.Find("AudioManager").GetComponent<audioManager>();
        enemy = GameObject.FindGameObjectWithTag("Boss Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator turnOnEyes()
    {

        //for as long as player is using joystick
        while (enemy.GetComponent<BossEnemy>().HP_ReturnCurrent() > 0 && enemy.GetComponent<BossEnemy>().returnCurrentEnergy() > 0)
        {
            //for each eye
            for (int i = 0; i < eyes.Length; i++)
            {
                //Debug.Log("turning it on ");
                if (i != 4)
                {
                    if(eyes[i].intensity <= 0.05f)
                    {
                        eyes[i].intensity += 0.01f;
                    }
                }
                else
                {
                    if(eyes[i].intensity <= 0.25f)
                    {
                        eyes[i].intensity += 0.01f;
                    }
                }

            }

            yield return new WaitForSeconds(0.15f);
        }

    }

    public IEnumerator turnOffEyes()
    {

        //for as long as player is using joystick
        while (enemy.GetComponent<BossEnemy>().HP_ReturnCurrent() <= 0)
        {
            //for each eye
            for (int i = 0; i < eyes.Length; i++)
            {

                if (eyes[i].intensity > 0.0f)
                {
                    eyes[i].intensity -= 0.01f;
                }

            }

            yield return new WaitForSeconds(0.1f);
        }

    }
}
