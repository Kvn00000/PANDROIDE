using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightGround : MonoBehaviour
{
    private MeshFilter MeshRight;
    //private Rigidbody RightRb;

    private Mesh meshsright;


    public void Init(Vector3[] vertices)
    {
        MeshRight = gameObject.AddComponent<MeshFilter>();
 
        // RightCollider = gameObject.AddComponent<BoxCollider>();
        // RightCollider.center = new Vector3(size/2F,size/2F,0F);
        // RightCollider.size = new Vector3(size,size,0F);

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
    }
    public void changeRightHeight(Vector3[] height){
        meshsright.vertices = height;
    }
}
