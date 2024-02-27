using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;


public class InitNav : MonoBehaviour
{
    //Taille de l'arene
    private static int _dx = 10;
    private static int _dz = 10;

    //Taille d'une case
    private double boxsize = 1;

    

    //Tableau de toutes les cases
    private GameObject[,] elements = new GameObject[_dx*_dx,_dz*_dz];
    private GameObject Walls ;

    //Tableau de mur
    private GameObject[] allwalls = new GameObject[4];

    //Je sais pas ca sert a quoi ca vient du prof
    public Transform init_transform;

    //Agent
    public GameObject agent;
    private NavMeshAgent navAgent;



    //Agent qu'on a créé
    private GameObject prout;

    //Environnement
    public GameObject box;
    public GameObject wall;
    private NavMeshSurface navSurface;


    private BoxCollider bc;


    // Start is called before the first frame update
    void Start()
    {
        
        //On créé un agent
        prout = Instantiate(agent, new Vector3(0,0,-4), init_transform.rotation);
        navAgent = prout.AddComponent<NavMeshAgent>();
        navAgent.speed = 1f;


        //Coords d'une case
        float x_ref = -_dx/2;
        float y_ref = 0;
        float z_ref = -_dz/2;

        //Compteur de nombre de case (au cas ou la case est de taille <1)
        int cptx = 0;
        int cptz = 0;
        for ( double x  = 0 ; x != _dx ; x = x + boxsize ) {
            for ( double z  = 0 ; z != _dz ; z = z + boxsize ) {
                if(x_ref+x == 0 && z_ref+z == 0){
                    //On place un case principale au milieu pour avoir un NavMeshSurface
                    Vector3 pos = new Vector3 ( (float)(x_ref+x) , (float)y_ref , (float)(x_ref+x) );
                    elements[cptx,cptz] = Instantiate(box, pos, init_transform.rotation);
                    navSurface = elements[cptx,cptz].AddComponent<NavMeshSurface>();
                }else{
                    //On place toutes les autres cases
                    Vector3 pos = new Vector3 ( (float)(x_ref+x) , (float)y_ref , (float)(z_ref+z) );
                    elements[cptx,cptz] = Instantiate(box, pos, init_transform.rotation);
                }
                cptz = cptz +1;
            }
            cptx = cptx + 1;

        
        }
         
    }

    // Update is called once per frame
    void Update()
    {
        //On MAJ la surface
        navSurface.BuildNavMesh();

        Vector3 positionEnAvant = prout.transform.position + prout.transform.forward * 1f;
        navAgent.SetDestination(positionEnAvant);
    }

}
