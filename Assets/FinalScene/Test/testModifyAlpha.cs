using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testModifyAlpha : MonoBehaviour
{
    // Start is called before the first frame update
    private float alphaA;
    public GameObject cube;
    void Start()
    {
        StartCoroutine(FadeToZeroAlpha(cube, 5.0f));
        
    }

    
    IEnumerator FadeToZeroAlpha(GameObject targetObject, float duration)
    {
        Renderer renderer = targetObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material material = renderer.material;
            Color color = material.color;
            float startAlpha = color.a;

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                float blend = t / duration;
                color.a = Mathf.Lerp(startAlpha, 0, blend);
                material.color = color;
                yield return null;
            }

            // Assurez-vous que l'alpha est exactement 0 après la fin de la boucle
            Destroy(targetObject);
            color.a = 0;
            material.color = color;
        }
    }
}
