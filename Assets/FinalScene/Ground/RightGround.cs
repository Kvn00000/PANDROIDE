using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightGround : MonoBehaviour
{

    //Coté droit du cube sur la layer Mur
    private MeshFilter MeshRight;
    private MeshCollider rightCollider;
    private Mesh meshsright;


    public void Init(Vector3[] vertices)
    {
        MeshRight = gameObject.AddComponent<MeshFilter>();


        // Droite
        int[] mytriangles = new int[6]{
            //Add the triangles clockwise
            3,6,2,
            3,7,6,
        };

        meshsright = new Mesh();
        meshsright.vertices = vertices;
        meshsright.triangles = mytriangles;
        meshsright.RecalculateNormals();
        MeshRight.mesh = meshsright;


        //Collider
        rightCollider = gameObject.AddComponent<MeshCollider>();
        rightCollider.sharedMesh = meshsright;
    }
    public void changeRightHeight(Vector3[] height){
        meshsright.vertices = height;
    }
}
