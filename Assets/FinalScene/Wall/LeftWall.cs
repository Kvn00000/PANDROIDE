using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LeftWall : MonoBehaviour
{
    private MeshFilter MeshLeft;
    //private Rigidbody rb;
    private BoxCollider LeftCollider;

    // Start is called before the first frame update
    public void Init(float wallsize)
    {
        
        var size = wallsize*0.5F;
        MeshLeft = gameObject.AddComponent<MeshFilter>();


        /*
        RightRb = gameObject.AddComponent<Rigidbody>();
        RightRb.useGravity = false;
        RightRb.isKinematic = true;
        RightRb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        */


        LeftCollider = gameObject.AddComponent<BoxCollider>();
        LeftCollider.center = new Vector3(0,wallsize*0.25F,size);
        LeftCollider.size = new Vector3(wallsize,size,0);
        
        

        //Cube made of 2 triangles
        Vector3[] vertices = new Vector3[4]{
            new Vector3(-size, 0, size),           
            new Vector3(-size, size,size),
            new Vector3(size, 0,size), 
            new Vector3(size, size,size), 
        };

        int[] triangles = new int[6]{
            //Add the triangles clockwise
            0,3,2,
            1,3,0,
        };

        Mesh meshs = new Mesh();
        meshs.vertices = vertices;
        //meshs.uv = uv;
        meshs.triangles = triangles;
        GetComponent<MeshFilter>().mesh = meshs;
    }
}
