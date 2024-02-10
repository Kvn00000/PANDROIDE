using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testK : MonoBehaviour
{
    public GameObject boid;
    public Vector3 force = new Vector3(1f, 0f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        //Invoke("ShowObject", 2);
        boid.GetComponent<Rigidbody>().AddForce(force,ForceMode.Impulse);
    }

    // Update is called once per frame
    public float speed = 2;
    


    void Update()
    {

        //boid.GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        
        
    }
}
