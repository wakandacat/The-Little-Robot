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
            GameObject spawnedBox = Instantiate(box, this.transform.position, Quaternion.identity);
            spawnedBox.transform.SetParent(this.transform);
            yield return new WaitForSeconds(3);
        }

    }
}