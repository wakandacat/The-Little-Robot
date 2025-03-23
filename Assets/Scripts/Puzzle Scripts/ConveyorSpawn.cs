using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConveyorSpawn : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>();
    public float spawnRate = 5;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnItems());
    }

    IEnumerator spawnItems()
    {

        while (true)
        {

            //get a random item from the list to instantiate
            int num = Random.Range(0, items.Count);
            GameObject currItem = items[num];

            //it is a fungus projectile
            if (currItem.gameObject.tag == "Projectile")
            {
                GameObject spawnedBox = Instantiate(currItem, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
                spawnedBox.transform.SetParent(this.transform);
            }
            else //otherwise it is a box
            {
                //x offset for spawn location due to parent xyz coordinate
                GameObject spawnedBox = Instantiate(currItem, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), currItem.transform.rotation);
                spawnedBox.transform.SetParent(this.transform);
            }

            
            yield return new WaitForSeconds(spawnRate);
        }

    }
}