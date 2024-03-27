using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackWall : MonoBehaviour
{
    private MeshFilter MeshBack;
    //private Rigidbody rb;
    private BoxCollider BackCollider;

    // Start is called before the first frame update
    public void Init(float wallsize)
    {
        MeshBack = gameObject.AddComponent<MeshFilter>();
        var size = wallsize*0.5F;

        BackCollider = gameObject.AddComponent<BoxCollider>();
        BackCollider.center = new Vector3(size,wallsize*0.25F,0);
        BackCollider.size = new Vector3(0F,size,wallsize);

        //Cube made of 2 triangles
        Vector3[] vertices = new Vector3[4]{
            new Vector3(size, 0,size), 
            new Vector3(size, size,size), 
            new Vector3(size, 0,-size), 
            new Vector3(size,size,-size), 
            
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
