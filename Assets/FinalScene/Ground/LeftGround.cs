using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftGround : MonoBehaviour
{
    // Coté gauche du cube avec layer Mur
    private MeshFilter MeshLeft;
    private MeshCollider LeftCollider;
    private Mesh meshsleft;

    public void Init(Vector3[] vertices)
    {
        MeshLeft = gameObject.AddComponent<MeshFilter>();

        //Coté gauche
        int[] mytriangles = new int[6]{
            //Add the triangles clockwise
            5,0,4,
            5,1,0,
        };

        meshsleft = new Mesh();
        meshsleft.RecalculateNormals();
        meshsleft.vertices = vertices;
        meshsleft.triangles = mytriangles;
        MeshLeft.mesh = meshsleft;

        //Collision
        LeftCollider = gameObject.AddComponent<MeshCollider>();
        LeftCollider.sharedMesh = meshsleft;
    }
    public void changeLeftHeight(Vector3[] height){
        meshsleft.vertices = height;
    }
}
