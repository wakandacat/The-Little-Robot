using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public bool enemyCollision = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //somewhere here collide with smth i am not sure what though
    public void OnCollisionEnter(Collision collision)
    {
       //Debug.Log("in enemyCollision before if");
        if (collision.gameObject.tag == "Boss Enemy")
        {
            enemyCollision = true;
            //Debug.Log("EnemyCollision: enemyCollision = true");
        }
    }
    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Boss Enemy")
        {
            enemyCollision = true;
            //Debug.Log("EnemyCollision: enemyCollision = true");
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Boss Enemy")
        {
            enemyCollision = false;
            //Debug.Log("EnemyCollision: enemyCollision = false");
        }
    }
}
