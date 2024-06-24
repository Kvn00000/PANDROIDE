using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;  
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SpawnBoidScript : MonoBehaviour
{

    public GameObject BoidPrefab;

    public GameObject CubePrefab;

    public int toInstantiate; // 0 pour Nothing, 1 pour Boid, 2 pour Cube
    public float spawnSpeed = 2;
    public InputActionProperty inputAction;


    public XRRayInteractor leftRay;
    public XRRayInteractor RightRay;


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
        bool isLeftRayHovering = leftRay.TryGetHitInfo(out Vector3 leftPos, out Vector3 leftNormal, out int leftNumber, out bool leftValid);
        bool isRightRayHovering = RightRay.TryGetHitInfo(out Vector3 rightPos, out Vector3 rightNormal, out int rightNumber, out bool rightValid);

        if(inputAction.action.WasPressedThisFrame() && toInstantiate > 0 && !isLeftRayHovering && !isRightRayHovering ){
            if(toInstantiate == 1){
                //Debug.Log("ici j'ai changé les params regarde :::: ");

                //Debug.Log(speed+" "+ wallRay+ " " + avoidRay+ " " + cohesionRay+ " " + attractionRay +" " + filter);
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
