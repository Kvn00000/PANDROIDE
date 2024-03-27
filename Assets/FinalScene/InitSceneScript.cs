using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;
using Unity.VisualScripting;
using UnityEngine.PlayerLoop;


public class InitSceneScript : MonoBehaviour
{
    //Taille de l'arene

    private static int _dx = 10;
    private static int _dz = 10;



    //Taille d'une case
    private float boxsize = 1;
    

    //Tableau de toutes les cases
    private GameObject[,] elements = new GameObject[_dx*_dx,_dz*_dz];
    private Top[,] component_box = new Top[_dx*_dx,_dz*_dz];

    //Murs
    private GameObject walls;
    private WallsScript component_wall;

    //Je sais pas ca sert a quoi ca vient du prof
    public Transform init_transform;

    //Agent
    public GameObject agent;

    //Environnement Prefab
    public GameObject box;
    public GameObject wall;

    public float something;


    private float x_ref;
    private float y_ref;
    private float z_ref;

    
    void Awake()
    {
        //Coords d'une case
        x_ref = -_dx*0.5F;
        y_ref = 0;
        z_ref = -_dz*0.5F;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate(box, new Vector3(2,2,0), init_transform.rotation);
        walls = Instantiate(wall, new Vector3(0,0,0), init_transform.rotation);
        component_wall = walls.GetComponent<WallsScript>();
        component_wall.Init(_dx);

        //Compteur de nombre de case
        int cptx = 0;
        int cptz = 0;
        for ( double x  = boxsize*0.5 ; x <= _dx ; x = x + boxsize ) {
            for ( double z  = boxsize*0.5 ; z <= _dz ; z = z + boxsize ) {
                Vector3 pos = new Vector3 ( (float)(x_ref+x) , (float)y_ref-boxsize*0.5F, (float)(z_ref+z) );
                elements[cptx,cptz] = Instantiate(box, pos, init_transform.rotation);
                // component_box[cptx,cptz] = elements[cptx,cptz].GetComponent<Top>();
                // component_box[cptx,cptz].Init(boxsize);
                cptz = cptz +1;
            }
            cptx = cptx + 1;
        }
    }
    void Update()
    {
        //component_box[0,0].changeHeight(something);
    }
}