using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fungusBlink : MonoBehaviour
{
    //red: https://stackoverflow.com/questions/57866803/how-to-change-the-basemap-property-of-a-shader-from-script-universal-rp-tem

    private float blinkTime = 1f;
    Color regularCol = new Color(1f, 1f, 1f, 1f); 
    Color blinkCol = new Color(157f / 255f, 0f / 255f, 0f / 255f, 1f); 

    private List<MeshRenderer> meshList = new List<MeshRenderer>(); //array of all child funguses
    private List<Material> matList = new List<Material>();
    public bool isBlinking = true;

    private Coroutine blinkMethod;

    // Start is called before the first frame update
    void Start()
    {
        // meshRenderer = this.GetComponent<MeshRenderer>();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            meshList.Add(this.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>());
            if (this.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>() != null)
            {
                // Use material if you want to modify at runtime
                matList.Add(this.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material);
            }
        }

        blinkMethod = StartCoroutine(blinkCoroutine());
    }

    //coroutine to blink between 2 colours
    private IEnumerator blinkCoroutine()
    {
        while (isBlinking)
        {
            float timer = 0;
            while (timer < 1)
            {
                for (int i = 0; i < matList.Count; i++)
                {
                    matList[i].SetColor("_BaseColor", Color.Lerp(regularCol, blinkCol, timer));
                }
                timer += Time.deltaTime / blinkTime;
                yield return null;
            }

            timer = 0;
            while (timer < 1)
            {
                for (int i = 0; i < matList.Count; i++)
                {
                    matList[i].SetColor("_BaseColor", Color.Lerp(blinkCol, regularCol, timer));
                }
                timer += Time.deltaTime / blinkTime;
                yield return null;
            }
        }
    }
}
