using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testModifyAlpha : MonoBehaviour
{
    // Start is called before the first frame update
    private void alphaA;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        changeAlpha(this,alphaA)
    }

    private void changeAlpha(GameObject obj, float alpha){
        if (obj != null)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = renderer.material;
                Color color = material.color;
                color.a = alpha;
                material.color = color;
            }
        }
    }
}
