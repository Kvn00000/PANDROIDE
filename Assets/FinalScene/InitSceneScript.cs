using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;


public class InitSceneScript : MonoBehaviour
{
    //Taille de l'arene
    private static int _dx = 10;
    private static int _dz = 10;

    //Taille d'une case
    private double boxsize = 1;

    

    //Tableau de toutes les cases
    private GameObject[,] elements = new GameObject[_dx*_dx,_dz*_dz];

    //Murs
    private GameObject walls;

    //Je sais pas ca sert a quoi ca vient du prof
    public Transform init_transform;

    //Agent
    public GameObject agent;

    //Environnement Prefab
    public GameObject box;
    public GameObject wall;


    

    // Start is called before the first frame update
    void Start()
    {
        walls = Instantiate(wall, new Vector3(0,0,0), init_transform.rotation);

        //Coords d'une case
        float x_ref = -_dx/2;
        float y_ref = 0;
        float z_ref = -_dz/2;

        //Compteur de nombre de case
        int cptx = 0;
        int cptz = 0;
        for ( double x  = 0 ; x != _dx ; x = x + boxsize ) {
            for ( double z  = 0 ; z != _dz ; z = z + boxsize ) {
                Vector3 pos = new Vector3 ( (float)(x_ref+x) , (float)y_ref-1 , (float)(z_ref+z) );
                elements[cptx,cptz] = Instantiate(box, pos, init_transform.rotation);
                cptz = cptz +1;
            }
            cptx = cptx + 1;
        }
    }
}
