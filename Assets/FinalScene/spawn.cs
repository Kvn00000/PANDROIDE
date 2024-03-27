using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{
    public Transform init_transform;
    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(obj, new Vector3(2,2,0), init_transform.rotation);
        Instantiate(obj, new Vector3(2,2,2), init_transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
