using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformScript : MonoBehaviour
{

    public GameObject box;
    private mainGameScript mainGameScript;

    // Start is called before the first frame update
    void Start()
    {
        mainGameScript = GameObject.Find("WorldManager").GetComponent<mainGameScript>();

        StartCoroutine(spawnItems());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator spawnItems()
    {
       // Debug.Log("mainGameScript.currentScene" + mainGameScript.currentScene);
      //  Debug.Log("SceneManager.GetActiveScene().name" + SceneManager.GetActiveScene().name);

        while (true)
        {
            if (mainGameScript.currentScene == SceneManager.GetActiveScene().name)
            {
               // Debug.Log("yooo");
                Instantiate(box, this.transform.position, Quaternion.identity);

            }

            yield return new WaitForSeconds(1);
        }
        
    }
}
