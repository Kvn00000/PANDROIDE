using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{

    public Vector3 force = new Vector3(0.01f, 0f, 0f);
    //private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {

        //rb = GetComponent<Rigidbody>();
        //Invoke("ShowObject", 2);
    }

    // Update is called once per frame
    public float speed = 2;
    


    void Update()
    {
        // Moves the object forward one unit every frame.
        //transform.position += new Vector3(0, 0, 0.001f);
        //rb.GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        //boid.GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        

    }
}
