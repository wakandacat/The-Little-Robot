using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tendril_Behavior : MonoBehaviour
{
    private Animator m_animator;
    public bool hasCollided = false;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = this.GetComponent<Animator>();

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_animator.CrossFadeInFixedTime("Strike", 0.2f, 0, 0.2f);
            this.GetComponent<AudioSource>().PlayOneShot(this.GetComponent<AudioSource>().clip); //play swinging sfx
        }
    }

    public void OnTriggerExit(Collider other)
    {
        hasCollided = false;
    }
}
