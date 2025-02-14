using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tendril_Behavior : MonoBehaviour
{
    private Animator m_animator;


    // Start is called before the first frame update
    void Start()
    {
        m_animator = this.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_animator.CrossFadeInFixedTime("Strike", 0.2f, 0, 0.2f);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.tag == "Damage Source")
        //{
        //    collision = false;
        //}
    }
}
