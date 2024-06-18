using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;  
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject BoidPrefab;
    public float spawnSpeed = 2;
    public InputActionProperty inputAction;


    // Update is called once per frame
    void Update()
    {
        if(inputAction.action.WasPressedThisFrame()){
            GameObject boid = Instantiate(BoidPrefab,transform.position, transform.rotation);
            
        }
    }
}
