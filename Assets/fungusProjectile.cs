using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fungusProjectile : MonoBehaviour
{

    public float speed = 100f;

    // Update is called once per frame
    void FixedUpdate()
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();

        Vector3 fungusVelocity =  this.transform.parent.up * speed * Time.deltaTime;
        rb.transform.Translate(fungusVelocity, Space.World);

        //rb.AddForce(fungusVelocity, ForceMode.VelocityChange);
    }

    //if it collides with anything, destroy itself
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != this.transform.parent.gameObject)
        {
            Destroy(this.gameObject);
        }
    }
}
