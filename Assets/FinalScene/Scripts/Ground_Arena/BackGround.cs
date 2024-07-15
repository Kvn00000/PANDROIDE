using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{

    //Back of the cube and set to layer Mur to detect the wall
    private MeshFilter MeshBack;
    private MeshCollider BackCollider;
    private Mesh meshsback;

    public void Init(Vector3[] vertices)
    {
        MeshBack = gameObject.AddComponent<MeshFilter>();

        //Back
        int[] mytriangles = new int[6]{
            7,4,6,
            7,5,4,
        };

        meshsback = new Mesh();
        meshsback.vertices = vertices;
        meshsback.triangles = mytriangles;

        meshsback.RecalculateNormals();
        MeshBack.mesh = meshsback;

        //for the detection
        BackCollider = gameObject.AddComponent<MeshCollider>();
        BackCollider.sharedMesh = meshsback;
    }

    public void changeBackHeight(Vector3[] height){
        meshsback.vertices = height;
    }
}
