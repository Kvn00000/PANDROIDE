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

    [Header("Prefab")]
    public GameObject BoidPrefab;
    public GameObject CubePrefab;


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
    
    
    private List<GameObject> boidList = new List<GameObject>();
    private Transform child;
    private int boidNumber= 0;
    private Vector3 spawnPosition= Vector3.zero;

    // Update is called once per frame
    void Start()
    {
        
        Transform child = this.transform.GetChild(1);
    }
    void Update(){
        //To set the right position
        if(inputAction.action.WasPressedThisFrame() && toInstantiate > 0){
            if(toInstantiate == 1){ // Instanciate Boid
                //Give the controller rotation
                if (boidList.Count < 100)
                {
                    Quaternion rotation = new Quaternion(0, this.transform.rotation.y, 0,this.transform.rotation.w) ;
                    GameObject boid = Instantiate(BoidPrefab,child.position, rotation);
                    boid.GetComponent<boidTuning>().Init(speed, wallRay, avoidRay,cohesionRay, attractionRay, filter);
                    boidList.Add(boid);
                }
            }
            else if(toInstantiate == 2){//Instanciate Cube
                Vector3 cubePos = new Vector3(child.position.x, child.position.y, child.position.z+0.02f);
                GameObject cube = Instantiate(CubePrefab, cubePos, Quaternion.identity);
                cube.layer = LayerMask.NameToLayer("MUR");
            }
        }
    }

    public void spawnBoidsInit()
    {
        
        for (int i = 0; i < boidNumber; i++)
        {
            //Angle aléatoire
            float randomAngleY = Random.Range(0f, 360f);
            Quaternion spawnRotation = Quaternion.Euler(0f, randomAngleY, 0f);
            Vector3 otherSpawn = new Vector3(spawnPosition.x, spawnPosition.y + 1, spawnPosition.z);
            GameObject obj = Instantiate(BoidPrefab, otherSpawn, spawnRotation);
            boidTuning tmp = obj.GetComponent<boidTuning>();
            
            tmp.Init(speed, wallRay, avoidRay, cohesionRay, attractionRay, filter);
            tmp.withDEBUG = false;
            boidList.Add(obj);
        }
    }


    //

    // Start is called before the first frame update
    public void Thanos()
    {
        foreach (GameObject boids in boidList)
        {
            Destroy(boids);
        }
        spawnBoidsInit();
    }
    public void setBoidNumber(int number)
    {
        this.boidNumber = number;
    }
    public void setSpawnPos(Vector3 position)
    {
        this.spawnPosition = position;
    }
}
