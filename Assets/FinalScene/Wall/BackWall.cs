using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackWall : MonoBehaviour
{
    private MeshFilter MeshBack;
    //private Rigidbody rb;
    private BoxCollider BackCollider;

    private float wallsize = 10F;
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
        BackCollider.center = new Vector3(wallsize/2F,wallsize/4F,0);
        BackCollider.size = new Vector3(0F,wallsize/2F,wallsize);

        

        //Cube made of 2 triangles
        Vector3[] vertices = new Vector3[4]{
            new Vector3(wallsize/2F, 0,wallsize/2F), 
            new Vector3(wallsize/2F, wallsize/2F,wallsize/2F), 
            new Vector3(wallsize/2F, 0,-wallsize/2F), 
            new Vector3(wallsize/2F,wallsize/2F,-wallsize/2F), 
            
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
