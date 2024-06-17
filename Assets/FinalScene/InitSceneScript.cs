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
    public float BoidSpeed;

    //Nombre de face pour le mur
    public int side;

    //Taille de l'arene
    public int arenaSize = 1;
    public bool damier = false;

    //Nombre de Boid
    public int BoidNumber;
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
        //Coords d'une case
        x_ref = -arenaSize*0.5F;
        y_ref = 0;
        z_ref = -arenaSize*0.5F;
        if(damier){
            elements= new GameObject[arenaSize*arenaSize,arenaSize*arenaSize];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //On ajoute le mur
        walls = Instantiate(wall, new Vector3(0,0,0), init_transform.rotation);
        component_wall = walls.GetComponent<CircleWallScript>();
        component_wall.DrawWall(side,arenaSize/2,arenaSize/4);


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
            _plane = Instantiate(plane, new Vector3(0,-arenaSize*0.5F,0), init_transform.rotation);
            _plane.GetComponent<Plane>().Init(arenaSize*0.5F);
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

            boidTuning obj=Instantiate(boid, spawnPosition,spawnRotation).GetComponent<boidTuning>();
            boidsList.Add(obj);
        
        }

        


    }


    private void updateSpeed(){

        foreach(boidTuning b in boidsList){
            b.speed = BoidSpeed;
        }

    }

    void update(){
        Debug.Log("test ???");
        if(BoidSpeed != boidsList[0].speed){
            updateSpeed();
        }
    }
}