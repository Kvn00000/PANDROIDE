using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Invoke("ShowObject", 2);
    }

    // Update is called once per frame
    public float speed = 2;
    void Update()
    {
        // Moves the object forward one unit every frame.
        //transform.position += new Vector3(0, 0, 0.001f);
        rb.AddForce(transform.forward * speed);
    }
}
