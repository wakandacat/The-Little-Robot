using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_fx_behaviors : MonoBehaviour
{
    audioManager m_audio;
    private GameObject enemy;
    public Light[] eyes;
    public Coroutine eyesOnCoroutine;
    public Coroutine eyesOffCoroutine;

    //Particle system
    public ParticleSystem Slam_rings;
    public ParticleSystem debris;
    public ParticleSystem stand_up_vfx;
    public ParticleSystem hand_impact;

    private ParticleSystem poleL_lightning;
    private ParticleSystem poleR_lightning;

    public bool meleeVFXBool = false;

    // Start is called before the first frame update
    void Start()
    {
        m_audio = GameObject.Find("AudioManager").GetComponent<audioManager>();
        enemy = GameObject.FindGameObjectWithTag("Boss Enemy");

        //particle system
        poleL_lightning = GameObject.Find("Lightning_L").GetComponent<ParticleSystem>();
        poleR_lightning = GameObject.Find("Lightning_R").GetComponent<ParticleSystem>();
        VFX_stopPoles();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //vfx_behaviour();
    }

    public IEnumerator turnOnEyes()
    {

        //for as long as enemy is alive and not in downed state
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

        //for as long as enemy is dead
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

    public void VFX_startPoles()
    {
        poleL_lightning.Play();
        poleR_lightning.Play();
    }
    public void VFX_stopPoles()
    {
        poleL_lightning.Stop();
        poleR_lightning.Stop();
    }
    //public void vfx_behaviour()
    //{
    //    meleeVFXBool = enemy.GetComponent<BossState>().returnMeleeVFXBool()
    //    if (enemy.GetComponent<BossState>().playMeleeVFX == true)
    //    {
    //        Invoke("meleeVFX", 3.0f);
    //    }
    //}

    //public void meleeVFX() 
    //{
    //    Slam_rings.Play();
    //    debris.Play();
    //    stand_up_vfx.Play();
    //    hand_impact.Play();
    //}
}
