using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackBox : TopBox
{
    private MeshFilter MeshBack;
    //private Rigidbody rb;
    private BoxCollider BackCollider;


    // Start is called before the first frame update
    public void Start()
    {
        
        MeshBack = gameObject.AddComponent<MeshFilter>();

        /*
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        */

        BackCollider = gameObject.AddComponent<BoxCollider>();
        BackCollider.center = new Vector3(size,size/2F,size/2F);
        BackCollider.size = new Vector3(0F,size,size);

        /*

        //Cube made of 2 triangles
        Vector3[] vertices = new Vector3[4]{
            new Vector3(size, 0, size), 
            new Vector3(size, size, size), 
            new Vector3(size, 0, 0),
            new Vector3(size, size, 0),
            
        };

        */
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
            7,5,6,
            5,4,6,
        };

        Mesh meshs = new Mesh();
        meshs.vertices = vertices;
        //meshs.uv = uv;
        meshs.triangles = triangles;
        transform.localScale = new Vector3((float)size,1f,(float)size);
        GetComponent<MeshFilter>().mesh = meshs;
    }
}
