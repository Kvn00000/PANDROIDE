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
    public int side;

    //Taille de l'arene
    public int arenaSize = 0;

    public bool damier = false;

    public int BoidNumber;
    //Tableau de boids
    private GameObject[] Boids;

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

        

        walls = Instantiate(wall, new Vector3(0,0,0), init_transform.rotation);
        component_wall = walls.GetComponent<CircleWallScript>();
        component_wall.DrawWall(side,arenaSize/2,arenaSize/2);



        if(damier){
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
            // _plane = Instantiate(plane, new Vector3(0,-arenaSize*0.5F,0), init_transform.rotation);
            // _plane.GetComponent<Plane>().Init(arenaSize*0.5F);
        }


        // Spawn des boids


        // for (int i = 0; i < BoidNumber; i++){
            


        //     float angle = Random.Range(0, 2 * Mathf.PI);

        //     // Calculer la position x et y à l'intérieur du cercle
        //     float x = arenaSize*0.5F * Mathf.Cos(angle);
        //     float y = arenaSize*0.5F * Mathf.Sin(angle);


        //     Vector3 spawnPos = new Vector3(x,0.5f,y);

        //     float randomAngleY = Random.Range(0f, 360f);
        //     Quaternion spawnRotation = Quaternion.Euler(0f, randomAngleY, 0f);

        //     Instantiate(boid, spawnPos,spawnRotation);
        //     // Boids[i] = Instantiate(boid, spawnPos,init_transform.rotation);
        // }


        
        for (int i = 0; i < BoidNumber; i++){
            Vector3 spawnPosition = new Vector3(
                Random.Range(-arenaSize/4f, arenaSize/ 4f),
                0.5F,
                Random.Range(-arenaSize /4f,arenaSize/4f)
            );
            float randomAngleY = Random.Range(0f, 360f);
            Quaternion spawnRotation = Quaternion.Euler(0f, randomAngleY, 0f);

            Instantiate(boid, spawnPosition,spawnRotation);
        }


    }
}