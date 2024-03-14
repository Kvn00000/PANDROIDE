using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeDetection : MonoBehaviour
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
        if (other.gameObject.layer == LayerMask.NameToLayer("MUR"))
        {
            //Debug.Log("Wall encoutered");
            Testbox parent = (Testbox)transform.parent.GetComponent<Testbox>();
            Vector3 mypos = parent.transform.position;
            Vector3 clp = other.ClosestPoint(mypos);
            float angle = Vector3.Angle(this.transform.forward, clp);
            //Debug.Log("WALL ANGLE ENTER is " + angle);
            if (angle < 90)
            {
                parent.AddCollider(other);
            }
            
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("MUR"))
        {
            Testbox parent = (Testbox)transform.parent.GetComponent<Testbox>();
            Vector3 mypos = parent.transform.position;
            Vector3 clp = other.ClosestPoint(mypos);
            float angle = Vector3.Angle(this.transform.forward, clp);
            //Debug.Log("WALL ANGLE STAY is " + angle);
            parent.AddCollider(other);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        Testbox parent = (Testbox)transform.parent.GetComponent<Testbox>();
        parent.RemoveCollider(other, other.gameObject.layer);
    }
}
