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
        var size = wallsize*0.5F;

        FrontCollider = gameObject.AddComponent<BoxCollider>();
        FrontCollider.center = new Vector3(-size,size*0.5F,0);
        FrontCollider.size = new Vector3(0F,size,wallsize);
        
        Vector3[] vertices = new Vector3[4]{
        new Vector3(-size, 0, -size),
            new Vector3(-size, size, -size),           
            new Vector3(-size, 0, size),           
            new Vector3(-size, size,size),
        };
        
        int[] triangles = new int[6]{
            //Add the triangles clockwise
            1,3,0,
            3,2,0,
        };

        Mesh meshs = new Mesh();
        meshs.vertices = vertices;
        meshs.triangles = triangles;
        GetComponent<MeshFilter>().mesh = meshs;
        
    }
}
