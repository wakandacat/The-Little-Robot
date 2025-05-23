using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class conveyorMove : MonoBehaviour
{
    //to use: give the boxes a speed value (since it is multiplied by time.deltatime the value must be very large -> at least 500 to notice it moving)
    //for direction, this will depend on the direction of the prefab in the environment (assign the value 1 to the direction you want the items to move (x, OR y, OR z))


    // tutorial references: https://www.youtube.com/watch?v=cSEg7Xm4A9A and https://www.youtube.com/watch?v=rQyUACEyAVw
    private List<GameObject> movingObjects;
    public float speed;
    public Vector3 direction;

    public bool suckerUpper = false;

    private Material material;

    private void Awake()
    {
        movingObjects = new List<GameObject>();

        material = GetComponent<MeshRenderer>().material;
    }

    void FixedUpdate()
    {
        if (suckerUpper)
        {
            //move the material the opposite way on the conveyor
            material.mainTextureOffset -= new Vector2(0, -1) * (speed / 1000) * Time.deltaTime;
        }
        else
        {
            //move the material on the conveyor
            material.mainTextureOffset -= new Vector2(0, 1) * (speed / 1000) * Time.deltaTime;
        }

        //for each object on the conveyor, add a force to it
        for (int i = 0; i < movingObjects.Count; i++)
        {

            //additively add conveyor velocity so as not to overwrite
            if (movingObjects[i] != null && movingObjects[i].GetComponent<Rigidbody>())
            {
                Rigidbody rb = movingObjects[i].GetComponent<Rigidbody>();
                Vector3 conveyorVelocity = speed * direction * Time.deltaTime;

                //keep the jump velocity
                rb.velocity = new Vector3(conveyorVelocity.x, rb.velocity.y, conveyorVelocity.z);
            }
        }
    }

    //add item when it collides with the belt
    public void OnTriggerEnter(Collider collision)
    {
        //if (collision.gameObject.tag == "Player")
        //{
        //    Debug.Log("hi");
        //}

        movingObjects.Add(collision.gameObject);
    }

    //when something leaves the belt
    public void OnTriggerExit(Collider collision)
    {
        //if (collision.gameObject.tag == "Player")
        //{
        //    Debug.Log("bye");
        //}

        movingObjects.Remove(collision.gameObject);
    }
}
