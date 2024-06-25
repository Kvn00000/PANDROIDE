using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildComponent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform){

            if (child.gameObject.GetComponent<MeshFilter>() == null)
            {
                child.gameObject.AddComponent<MeshFilter>();
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
