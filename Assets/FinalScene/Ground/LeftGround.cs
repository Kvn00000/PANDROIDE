using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftGround : MonoBehaviour
{
    private MeshFilter MeshLeft;
    private BoxCollider LeftCollider;

    private Mesh meshsleft;

    public void Init(Vector3[] vertices)
    {
        MeshLeft = gameObject.AddComponent<MeshFilter>();

        // LeftCollider = gameObject.AddComponent<BoxCollider>();
        // LeftCollider.center = new Vector3(size/2F,size/2F,size);
        // LeftCollider.size = new Vector3(size,size,0);
        
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
    }
    public void changeLeftHeight(Vector3[] height){
        meshsleft.vertices = height;
    }
}
