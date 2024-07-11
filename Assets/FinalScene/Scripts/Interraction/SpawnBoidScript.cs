using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;  
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SpawnBoidScript : MonoBehaviour
{
    public InputActionProperty inputAction;

    public int toInstantiate; // 0 pour Nothing, 1 pour Boid, 2 pour Cube
    public float spawnSpeed = 2;

    private InitSceneScript initScript;

    [Header("Prefab")]
    public GameObject BoidPrefab;
    public GameObject CubePrefab;
    [SerializeField]
    private GameObject rig;

    [Header("Boid Settings")]
    public float speed = 200.0f;
    public bool withGoto = false;
    public bool withCohesion = false;
    public bool withAvoid = false;
    public float wallRay=0.6f;
    public float avoidRay = 0.6f;
    public float cohesionRay = 1.0f;
    public float attractionRay = 1.1f;
    public float filter = 5;
    [Header("Cube Settings Button")]
    public float CubeSize = 0.01f;

    private ScenePlaneDetectController detect;
    private Transform child;

    // Update is called once per frame
    void Start()
    {
        
        child = this.transform.GetChild(1);
        Debug.Log("           Spawner ");
        detect = rig.GetComponent<ScenePlaneDetectController>();

    }
    void Update(){
        //To set the right position
        if(inputAction.action.WasPressedThisFrame() && toInstantiate > 0){
            if(toInstantiate == 1){ // Instanciate Boid
                //Give the controller rotation
                if ((initScript !=null) &&(initScript.getBoidListCount() < 100))
                {
                    Quaternion rotation = new Quaternion(0, this.transform.rotation.y, 0,this.transform.rotation.w) ;
                    GameObject boid = Instantiate(BoidPrefab,child.position, rotation);
                    boid.GetComponent<boidTuning>().Init(speed, wallRay, avoidRay,cohesionRay, attractionRay, filter);
                    boid.GetComponent<FadeOut>().setScene(initScript);
                    boid.GetComponent<FadeOut>().setDetect(detect);
                    initScript.addBoidList(boid);
                }
            }
            else if(toInstantiate == 2){//Instanciate Cube
                Vector3 cubePos = new Vector3(child.position.x, child.position.y, child.position.z);
                cubePos = child.InverseTransformPoint(cubePos);
                cubePos.z += 0.05f;
                GameObject cube = Instantiate(CubePrefab, child.TransformPoint(cubePos), Quaternion.identity);
                cube.layer = LayerMask.NameToLayer("MUR");
            }
        }
    }
    public void setScene(InitSceneScript sc) 
    {
        this.initScript = sc;
    }
    public InitSceneScript getScene()
    {
        return this.initScript;
    }

}
