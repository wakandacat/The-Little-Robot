using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConveyorSpawn : MonoBehaviour
{
    public GameObject box;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnItems());
    }

    IEnumerator spawnItems()
    {
        while (true)
        {
            //x offset for spawn location due to parent xyz coordinate
            GameObject spawnedBox = Instantiate(box, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
            spawnedBox.transform.SetParent(this.transform);
            yield return new WaitForSeconds(3);
        }

    }
}