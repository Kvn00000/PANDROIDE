using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine.AI;
using System;

public class InitialSpawnScript : MonoBehaviour
{
    //Taille de l'arene
    private static int _dx = 10;
    private static int _dz = 10;

    //Taille d'une case
    private double boxsize = 1;

    

    //Tableau de toutes les cases
    private GameObject[,] elements = new GameObject[_dx*_dx,_dz*_dz];

    private GameObject Walls ;


    //Je sais pas ca sert
    public Transform init_transform;


    //Environnement
    public GameObject box;
    public GameObject wall;
    private NavMeshSurface navSurface;


    private BoxCollider bc;


    // Start is called before the first frame update
    void Start()
    {
        bc = gameObject.AddComponent<BoxCollider>();
        bc.center = new Vector3(0,0,0);
        bc.size = new Vector3(_dx,0.01F,_dz);
        Walls = Instantiate(wall, new Vector3(0,0,0), init_transform.rotation);

        //Coords d'une case
        float x_ref = -_dx/2;
        float y_ref = 0;
        float z_ref = -_dz/2;

        //Compteur de nombre de case (au cas ou la case est de taille <1)
        int cptx = 0;
        int cptz = 0;
        for ( double x  = 0 ; x != _dx ; x = x + boxsize ) {
            for ( double z  = 0 ; z != _dz ; z = z + boxsize ) {
                //On place toutes les cases
                Vector3 pos = new Vector3 ( (float)(x_ref+x) , (float)y_ref , (float)(z_ref+z) );
                elements[cptx,cptz] = Instantiate(box, pos, init_transform.rotation);
                cptz = cptz +1;
            }
            cptx = cptx + 1;
        }
    }
}
