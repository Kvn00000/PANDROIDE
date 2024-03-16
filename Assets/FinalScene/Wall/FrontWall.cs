using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FrontWall : MonoBehaviour
{
    private MeshFilter MeshFront;
    //private Rigidbody rb;
    private BoxCollider FrontCollider;

    // Start is called before the first frame update
    public void Init(float wallsize)    {
        MeshFront = gameObject.AddComponent<MeshFilter>();
        /*
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        */
        FrontCollider = gameObject.AddComponent<BoxCollider>();
        FrontCollider.center = new Vector3(-wallsize/2F,wallsize/4F,0);
        FrontCollider.size = new Vector3(0F,wallsize/2F,wallsize);
        
        

        Vector3[] vertices = new Vector3[4]{
        new Vector3(-wallsize/2F, 0, -wallsize/2F),
            new Vector3(-wallsize/2F, wallsize/2F, -wallsize/2F),           
            new Vector3(-wallsize/2F, 0, wallsize/2F),           
            new Vector3(-wallsize/2F, wallsize/2F,wallsize/2F),


        };

        //Je sais pas ca sert a quoi uv mdr
        /*
        Vector2[] uv = new Vector2[4]{
            new Vector2(0,0),           
            new Vector2(0, wallsize),
            new Vector2(wallsize, 0),           
            new Vector2(wallsize, wallsize),
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
