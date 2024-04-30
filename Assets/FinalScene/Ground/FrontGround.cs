using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
public class FrontGround : MonoBehaviour
{   
    
    private MeshFilter MeshFront;
    private BoxCollider FrontCollider;

    private Mesh meshsfront;
    public void Init(Vector3[] vertices)
    {
        MeshFront = gameObject.AddComponent<MeshFilter>();

        // FrontCollider = gameObject.AddComponent<BoxCollider>();
        // FrontCollider.center = new Vector3(size,size/2F,size/2F);
        // FrontCollider.size = new Vector3(0F,size,size);

        int[] mytriangles = new int[6]{
            //Add the triangles clockwise
            1,2,0,
            1,3,2,
        };

        meshsfront = new Mesh();
        meshsfront.vertices = vertices;
        meshsfront.triangles = mytriangles;
        meshsfront.RecalculateNormals();
        MeshFront.mesh = meshsfront;
    }
    public void changeFrontHeight(Vector3[] height){
        meshsfront.vertices = height;
    }
}
