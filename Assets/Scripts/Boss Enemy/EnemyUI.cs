using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    public GameObject enemyUI;
    public Image enemyHealthBar;
    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        decrementBar();
    }
    public void decrementBar()
    {
        var maxenemyHealth = enemy.GetComponent<BossEnemy>().HP_Maximum;
        var enemyCurrentHealth = enemy.GetComponent<BossEnemy>().HP_ReturnCurrent();

        enemyHealthBar.fillAmount = (enemyCurrentHealth / maxenemyHealth);
    }
}
