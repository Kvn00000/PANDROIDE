using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RightBox : TopBox
{
    private MeshFilter MeshRight;
    //private Rigidbody RightRb;
    private BoxCollider RightCollider;


    // Start is called before the first frame update
    void Start()
    {
        MeshRight = gameObject.AddComponent<MeshFilter>();
    
        /*
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        */


        RightCollider = gameObject.AddComponent<BoxCollider>();
        RightCollider.center = new Vector3(size/2F,size/2F,0F);
        RightCollider.size = new Vector3(size,size,0F);
        

        
        /*
        //Cube made of 2 triangles
        Vector3[] vertices = new Vector3[4]{
            new Vector3(0, 0, 0),
            new Vector3(0, size, 0),
            new Vector3(size, 0, 0),
            new Vector3(size, size, 0),

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
            5,1,0,
            4,5,0,
        };

        Mesh meshs = new Mesh();
        meshs.vertices = vertices;
        //meshs.uv = uv;
        meshs.triangles = triangles;
        GetComponent<MeshFilter>().mesh = meshs;
    }
}