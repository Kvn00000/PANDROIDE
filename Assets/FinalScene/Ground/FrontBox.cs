using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FrontBox : TopBox
{
    private MeshFilter MeshFront;
    //private Rigidbody rb;
    private BoxCollider FrontCollider;


    // Start is called before the first frame update
    void Start()
    {

        MeshFront = gameObject.AddComponent<MeshFilter>();
        /*
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        */
        FrontCollider = gameObject.AddComponent<BoxCollider>();
        FrontCollider.center = new Vector3(0F,size/2F,size/2F);
        FrontCollider.size = new Vector3(0F,size,size);
        
        //Cube made of 2 triangles
        /*Vector3[] vertices = new Vector3[4]{
            new Vector3(0, 0, 0),
            new Vector3(0, size, 0),
            new Vector3(0, 0, size),
            new Vector3(0, size, size),
        };*/
        //Je sais pas ca sert a quoi uv mdr
        /*
        Vector2[] uv = new Vector2[4]{
            new Vector2(0,0),           
            new Vector2(0, size),
            new Vector2(size, 0),           
            new Vector2(size, size),
        };
        */
        int[] triangles = new int[6]{
            //Add the triangles clockwise
            1,3,0,
            3,2,0,
        };


        Mesh meshs = new Mesh();
        meshs.vertices = vertices;
        //meshs.uv = uv;
        meshs.triangles = triangles;

        GetComponent<MeshFilter>().mesh = meshs;
        
    }
}
