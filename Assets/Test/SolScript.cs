using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Sol2 : MonoBehaviour
{
    private MeshFilter _meshFilter;
    private Rigidbody rb;
    private BoxCollider bc;

    //Taille d'une case
    public float _dx = 1F;
    public float _dz = 1F;

    // Start is called before the first frame update
    void Start()
    {

        _meshFilter = gameObject.AddComponent<MeshFilter>();
        
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        bc = gameObject.AddComponent<BoxCollider>();
        bc.center = new Vector3(_dx/2F,-0.05F,_dz/2F);
        bc.size = new Vector3(_dx,0.1F,_dz);

        //Cube made of 2 triangles
        Vector3[] vertices = new Vector3[4]{
            new Vector3(0, 0, 0),
            new Vector3(0, 0, _dz),
            new Vector3(_dx, 0, 0),
            new Vector3(_dx, 0, _dz), 
        };
        //Je sais pas ca sert a quoi uv mdr
        /*
        Vector2[] uv = new Vector2[4]{
            new Vector2(0,0),           
            new Vector2(0, _dz),
            new Vector2(_dx, 0),           
            new Vector2(_dx, _dz),
        };
        */
        int[] triangles = new int[6]{
            //Add the triangles clockwise
            1,3,2,
            0,1,2,
        };


        Mesh meshs = new Mesh();
        meshs.vertices = vertices;
        //meshs.uv = uv;
        meshs.triangles = triangles;


        transform.localScale = new Vector3((float)_dx,1f,(float)_dz);

        GetComponent<MeshFilter>().mesh = meshs;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
