using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideBoxScript : MonoBehaviour{
    private MeshFilter _meshFilter;
    protected float wallsize = 1F;

    private Rigidbody rb;
    private BoxCollider bc1;
    private BoxCollider bc2;
    private BoxCollider bc3;
    private BoxCollider bc4;

    // Start is called before the first frame update
    void Start(){
        
        _meshFilter = gameObject.AddComponent<MeshFilter>();

        /*
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        */

        bc1 = gameObject.AddComponent<BoxCollider>();
        bc1.center = new Vector3(0,wallsize/2F,wallsize/2F);
        bc1.size = new Vector3(wallsize,wallsize,0);
        bc1.isTrigger = true;

        bc2 = gameObject.AddComponent<BoxCollider>();
        bc2.center = new Vector3(0,wallsize/2F,-wallsize/2F);
        bc2.size = new Vector3(wallsize,wallsize,0);
        bc2.isTrigger = true;

        bc3 = gameObject.AddComponent<BoxCollider>();
        bc3.center = new Vector3(wallsize/2F,wallsize/2F,0);
        bc3.size = new Vector3(0,wallsize,wallsize);
        bc3.isTrigger = true;

        bc4 = gameObject.AddComponent<BoxCollider>();
        bc4.center = new Vector3(-wallsize/2F,wallsize/2F,0);
        bc4.size = new Vector3(0,wallsize,wallsize);
        bc4.isTrigger = true;

        
        Vector3[] vertices = new Vector3[8]{
            new Vector3(-wallsize/2F, 0, -wallsize/2F),
            new Vector3(-wallsize/2F, wallsize, -wallsize/2F),           
            new Vector3(-wallsize/2F, 0, wallsize/2F),           
            new Vector3(-wallsize/2F, wallsize,wallsize/2F),
            new Vector3(wallsize/2F, 0,-wallsize/2F), 
            new Vector3(wallsize/2F,wallsize,-wallsize/2F), 
            new Vector3(wallsize/2F, 0,wallsize/2F), 
            new Vector3(wallsize/2F, wallsize,wallsize/2F), 
        };

        /*Vector2[] uv = new Vector2[4]{
            new Vector2(0,0),           
            new Vector2(0, _dy),
            new Vector2(_dx, 0),           
            new Vector2(_dx, _dy),
        };
        */

        int[] triangles = new int[24]{
            //Add the triangles clockwise
            1,3,2,
            0,1,2,
            4,5,0,
            0,5,1,
            6,7,5,
            6,5,4,
            3,6,2,
            3,7,6,
        };
        
        Mesh meshs = new Mesh();
        meshs.vertices = vertices;
        //meshs.uv = uv;
        meshs.triangles = triangles;

        //transform.localScale = new Vector3(_dx,_dy,1f);
        GetComponent<MeshFilter>().mesh = meshs;
        
    }
}
