using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ligthFlicker : MonoBehaviour
{

    //https://discussions.unity.com/t/flickering-light/376930/3
    public GameObject spotLight;
    private float minFlickerON = 0.1f;
    private float maxFlickerON = 0.5f;

    private float minFlickerOFF = 0.5f;
    private float maxFlickerOFF = 2.0f;

    private Coroutine flickering;

    void Start()
    {
        flickering = StartCoroutine(FlickerLight());
    }

    IEnumerator FlickerLight()
    {
        while (true)
        {
            spotLight.SetActive(true);
            yield return new WaitForSeconds(Random.Range(minFlickerON, maxFlickerON));

            spotLight.SetActive(false);
            yield return new WaitForSeconds(Random.Range(minFlickerOFF, maxFlickerOFF));
        }
    }
}
