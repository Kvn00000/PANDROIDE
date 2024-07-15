using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
public class FrontGround : MonoBehaviour
{   
    //front of the cube and set to layer Mur to detect the wall
    private MeshFilter MeshFront;
    private MeshCollider FrontCollider;

    private Mesh meshsfront;
    public void Init(Vector3[] vertices)
    {
        MeshFront = gameObject.AddComponent<MeshFilter>();

        //front
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

        FrontCollider = gameObject.AddComponent<MeshCollider>();
        FrontCollider.sharedMesh = meshsfront;
    }
    public void changeFrontHeight(Vector3[] height){
        meshsfront.vertices = height;
    }
}
