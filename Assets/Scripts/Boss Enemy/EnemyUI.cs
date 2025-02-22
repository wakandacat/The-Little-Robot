using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyUI : MonoBehaviour
{
    public GameObject enemyUI;
    public Image enemyHealthBar;
    private GameObject enemy;
    private PlayerController playerScript;
    UnityEngine.SceneManagement.Scene currentScene;
    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("currentScene" + SceneManager.GetActiveScene().name);
/*        if (SceneManager.GetActiveScene().name == "Combat1")
        {
            enemyUI.SetActive(true);
            Debug.Log("Help");
        }
        else
        {
            enemyUI.SetActive(false);
        }
*/
    }
}
