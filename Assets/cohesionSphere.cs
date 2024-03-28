using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cohesionSphere : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("BOID"))
        {
            Testbox parent = (Testbox)transform.parent.GetComponent<Testbox>();
            Vector3 pPos = parent.GetComponent<Rigidbody>().position;
            float dist = Vector3.Distance(pPos, other.ClosestPoint(pPos));
            //CHECK IF NOT COLLIDED WITH SELF
            if (dist > 0.1f)
            {
                parent.addCohesionBoid(other);
            }

        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("BOID"))
        {
            Testbox parent = (Testbox)transform.parent.GetComponent<Testbox>();
            Vector3 pPos = parent.GetComponent<Rigidbody>().position;
            float dist = Vector3.Distance(pPos, other.ClosestPoint(pPos));
            //CHECK IF NOT COLLIDED WITH SELF
            if (dist > 0.1f)
            {
                parent.addCohesionBoid(other);
            }
        }

    }
}
