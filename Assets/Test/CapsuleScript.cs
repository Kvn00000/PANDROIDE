using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using UnityEngine.AI;



public class CapsuleScript : MonoBehaviour
{
    private NavMeshAgent navAgent;
    RaycastHit hit;
    public Vector3 location;
    // Start is called before the first frame update
    void Start()
    {
        /*
        navAgent = gameObject.AddComponent<NavMeshAgent>();
        navAgent.speed = 1f;
        */
        var collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 positionEnAvant = gameObject.transform.position + gameObject.transform.forward * 1f;
        //navAgent.SetDestination(positionEnAvant);
        Cast();
    }

    private void OnCollisionStay(Collision collision)
    {
        //Debug.Log(collision.transform.position);
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position-transform.up, 0);
    }

    private void Cast(){
        if(Physics.SphereCast(transform.position,0,transform.up,out hit)){
            //Debug.Log(hit.collider.gameObject.transform.position);
            Debug.Log(hit.point);
            Debug.Log(hit.distance);

        }
    }
}
