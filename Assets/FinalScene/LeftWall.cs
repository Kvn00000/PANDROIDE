using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LeftWall : MonoBehaviour
{
    private MeshFilter MeshLeft;
    //private Rigidbody rb;
    private BoxCollider LeftCollider;

    private float size = 5F;
    // Start is called before the first frame update
    void Start()
    {
        

        MeshLeft = gameObject.AddComponent<MeshFilter>();


        /*
        RightRb = gameObject.AddComponent<Rigidbody>();
        RightRb.useGravity = false;
        RightRb.isKinematic = true;
        RightRb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        */


        LeftCollider = gameObject.AddComponent<BoxCollider>();
        LeftCollider.center = new Vector3(0/2F,2.5F,1);
        LeftCollider.size = new Vector3(2,5,0);
        
        

        //Cube made of 2 triangles
        Vector3[] vertices = new Vector3[4]{
            new Vector3(0, 0, size),
            new Vector3(0, size/2F, size),
            new Vector3(size, 0, size), 
            new Vector3(size, size/2F, size), 
        };

        
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
            3,7,2,
            7,6,2,
        };

        Mesh meshs = new Mesh();
        meshs.vertices = vertices;
        //meshs.uv = uv;
        meshs.triangles = triangles;
        transform.localScale = new Vector3((float)size,1f,(float)size);
        GetComponent<MeshFilter>().mesh = meshs;
    }
}
