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

        Transform child = this.transform.GetChild(1);
        if(inputAction.action.WasPressedThisFrame() && toInstantiate > 0 && !isLeftRayHovering && !isRightRayHovering ){
            if(toInstantiate == 1){
                //Debug.Log("ici j'ai changé les params regarde :::: ");

                //Debug.Log(speed+" "+ wallRay+ " " + avoidRay+ " " + cohesionRay+ " " + attractionRay +" " + filter);
                Quaternion rotation = new Quaternion(0, this.transform.rotation.y, 0,this.transform.rotation.w) ;
                GameObject boid = Instantiate(BoidPrefab,child.position, rotation);
                boid.GetComponent<boidTuning>().Init(speed, wallRay, avoidRay,
                                                     cohesionRay, attractionRay, filter);
            }
            else if(toInstantiate == 2){
                Vector3 cubePos = new Vector3(child.position.x, child.position.y, child.position.z+0.02f);
                GameObject cube = Instantiate(CubePrefab, cubePos, Quaternion.identity);
                cube.layer = LayerMask.NameToLayer("MUR");
                //cube.GetComponent<Cube>().Init(CubeSize);
            }
        }
    }
}
