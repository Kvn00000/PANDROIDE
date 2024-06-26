using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;
using Unity.VisualScripting;
using UnityEngine.PlayerLoop;
using System.Runtime.InteropServices;


public class InitSceneScript : MonoBehaviour
{
    //Environnement Prefab
    public Transform init_transform;
    public GameObject box;
    public GameObject plane;
    public GameObject wall;

    //Boid Prefab
    public GameObject boid;
    [SerializeField]
    private GameObject controllerSpawner;

    //Nombre de face pour le mur
    public int side;

    //Taille de l'arene
    public float arenaSize = 0;
    public bool damier = false;

    //Nombre de Boid
    public int BoidNumber;

    public float BoidSpeed;
    public float wallRay;
    public float avoidRay;
    public float cohesionRay;
    public float attractionRay;
    public float filter;
    

    private List<boidTuning> boidsList = new List<boidTuning>();

    //Taille d'une case
    private float boxsize = 1;
    
    //Tableau de toutes les cases
    private GameObject[,] elements;

    //Murs
    private GameObject walls;
    private CircleWallScript component_wall;
    private GameObject _plane;
    

    private float x_ref;
    private float y_ref;
    private float z_ref;

    
    void Awake()
    {
        
    }

    // Start is called before the first frame update
    public void Init(Vector3 _spawnPos,float _sizeTable,bool _damier)
    {
        arenaSize = _sizeTable;
        // Scaling Boids parameters
        BoidSpeed = 100;
        wallRay = arenaSize * 0.09f;
        avoidRay=wallRay;
        cohesionRay=arenaSize*0.5f;
        attractionRay=arenaSize*0.6f;
        filter=3;
        SpawnBoidScript tomodif = controllerSpawner.GetComponent<SpawnBoidScript>();
        tomodif.speed =BoidSpeed;
        tomodif.wallRay = wallRay;
        tomodif.avoidRay = avoidRay;
        tomodif.cohesionRay = cohesionRay;
        tomodif.attractionRay = attractionRay;
        tomodif.filter = filter;
        Debug.Log("ARENA SIZE "+arenaSize);
    //Coords d'une case
    x_ref = -arenaSize * 0.5F;
        y_ref = 0;
        z_ref = -arenaSize * 0.5F;
        //if (damier)
        //{
        //    elements = new GameObject[arenaSize * arenaSize, arenaSize * arenaSize];
        //}
        init_transform.position = _spawnPos;
        //On ajoute le mur
        walls = Instantiate(wall, init_transform.position, init_transform.rotation);
        component_wall = walls.GetComponent<CircleWallScript>();
        component_wall.DrawWall(side,arenaSize/2,arenaSize/12f);


        if(damier){
            //Ajout du damier

            //Compteur de nombre de case
            int cptx = 0;
            int cptz = 0;
            for ( double x  = boxsize*0.5 ; x <= arenaSize ; x = x + boxsize ) {
                for ( double z  = boxsize*0.5 ; z <= arenaSize ; z = z + boxsize ) {
                    Vector3 pos = new Vector3 ( (float)(x_ref+x) , (float)(y_ref-boxsize*0.5F), (float)(z_ref+z) );
                    elements[cptx,cptz] = Instantiate(box, pos, init_transform.rotation);
                    cptz = cptz +1;
                }
                cptx = cptx + 1;
            }

        }else{
            //Ajout du plane
            //_plane = Instantiate(plane, new Vector3(0,-arenaSize*0.5F,0), init_transform.rotation);
            //_plane = Instantiate(plane, _spawnPos, init_transform.rotation);
            //_plane.GetComponent<Plane>().Init(arenaSize*0.5F);
            //Debug.Log("THE Plane LAYER IS " + _plane.layer);
            //_plane.layer = 7;
        }


        // Spawn des boids

        for (int i = 0; i < BoidNumber; i++){
            //Coordonnées aléatoire
            Vector3 spawnPosition = new Vector3(
                Random.Range(-arenaSize/4f, arenaSize/ 4f),
                0.5F,
                Random.Range(-arenaSize /4f,arenaSize/4f)
            );
            //Angle aléatoire
            float randomAngleY = Random.Range(0f, 360f);
            Quaternion spawnRotation = Quaternion.Euler(0f, randomAngleY, 0f);
            Vector3 otherSpawn = new Vector3(_spawnPos.x, _spawnPos.y+1,_spawnPos.z) ;
            boidTuning obj=Instantiate(boid, otherSpawn,spawnRotation).GetComponent<boidTuning>();
            obj.Init(BoidSpeed, wallRay, avoidRay, cohesionRay, attractionRay, filter);
            obj.withDEBUG = false;
            boidsList.Add(obj);
        
        }
    }


    private void updateSpeed(){

        foreach(boidTuning b in boidsList){
            b.speed = BoidSpeed;
        }

    }

    private void updateWallRay(){
        foreach(boidTuning b in boidsList){
            b.wallRay = wallRay;
        }
    }

    private void updateAvoidRay(){
        foreach(boidTuning b in boidsList){
            b.avoidRay = avoidRay;
        }
    }

    private void updateCohesionRay(){
        foreach(boidTuning b in boidsList){
            b.cohesionRay = cohesionRay;
        }
    }

    private void updateAttractionRay(){
        foreach(boidTuning b in boidsList){
            b.attractionRay = attractionRay;
        }
    }

    private void updateFilter(){
        foreach(boidTuning b in boidsList){
            b.filter = filter;
        }
    }


    /*
    void Update(){

        if(BoidSpeed != boidsList[0].speed){
            updateSpeed();
        }
        if(wallRay != boidsList[0].wallRay){
            updateWallRay();
        }
        if(avoidRay != boidsList[0].avoidRay){
            updateAvoidRay();
        }
        if(cohesionRay != boidsList[0].cohesionRay){
            updateCohesionRay();
        }
        if(attractionRay != boidsList[0].attractionRay){
            updateAttractionRay();
        }
        if(filter != boidsList[0].filter){
            updateFilter();
        }
    }
    */
}