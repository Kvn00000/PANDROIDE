using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
public class FrontGround : MonoBehaviour
{   
    // Avant du cube avec une layer Mur pour la detection du boid sur un mur

    private MeshFilter MeshFront;
    private MeshCollider FrontCollider;

    private Mesh meshsfront;
    public void Init(Vector3[] vertices)
    {
        MeshFront = gameObject.AddComponent<MeshFilter>();

        //Face Avant du cube
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

        //Detection de la face avant
        FrontCollider = gameObject.AddComponent<MeshCollider>();
        FrontCollider.sharedMesh = meshsfront;
    }
    public void changeFrontHeight(Vector3[] height){
        meshsfront.vertices = height;
    }
}
