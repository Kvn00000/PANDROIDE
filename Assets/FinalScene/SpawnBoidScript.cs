using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;  
using UnityEngine;

public class SpawnBoidScript : MonoBehaviour
{

    public GameObject BoidPrefab;

    public GameObject CubePrefab;

    public int toInstantiate; // 0 pour Nothing, 1 pour Boid, 2 pour Cube
    public float spawnSpeed = 2;
    public InputActionProperty inputAction;



    // Boid Param
    public float speed = 200.0f;
    public bool withGoto = false;
    public bool withCohesion = false;
    public bool withAvoid = false;
    public float wallRay=0.6f;
    public float avoidRay = 0.6f;
    public float cohesionRay = 1.0f;
    public float attractionRay = 1.1f;
    public float filter = 5;


    //Cube Param
    public float CubeSize = 0.01f;

    // Update is called once per frame
    void Update()
    {


        if(inputAction.action.WasPressedThisFrame() && toInstantiate > 0){
            if(toInstantiate == 1){
                GameObject boid = Instantiate(BoidPrefab,transform.position, Quaternion.identity);
                boid.GetComponent<boidTuning>().Init(speed, wallRay, avoidRay,
                                                     cohesionRay, attractionRay, filter);
            }
            else if(toInstantiate == 2){
                GameObject cube = Instantiate(CubePrefab,transform.position, Quaternion.identity);
                cube.GetComponent<Cube>().Init(CubeSize);
            }
        }
    }
}
