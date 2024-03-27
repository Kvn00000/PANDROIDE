using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RightWall : MonoBehaviour
{
    private MeshFilter MeshRight;
    //private Rigidbody RightRb;
    private BoxCollider RightCollider;


    // Start is called before the first frame update
    public void Init(float wallsize)
    {
        var size = wallsize*0.5F;
        MeshRight = gameObject.AddComponent<MeshFilter>();
    
        /*
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        */

        RightCollider = gameObject.AddComponent<BoxCollider>();
        RightCollider.center = new Vector3(0,wallsize*0.25F,-size);
        RightCollider.size = new Vector3(wallsize,size,0F);
        

    
        //Cube made of 2 triangles
        Vector3[] vertices = new Vector3[4]{
            new Vector3(-size, 0, -size),
            new Vector3(-size, size, -size),   
            new Vector3(size, 0,-size), 
            new Vector3(size,size,-size), 

        };

        int[] triangles = new int[6]{
            //Add the triangles clockwise
            3,0,2,
            3,1,0,
        };

        Mesh meshs = new Mesh();
        meshs.vertices = vertices;
        meshs.triangles = triangles;
        GetComponent<MeshFilter>().mesh = meshs;
    }
}