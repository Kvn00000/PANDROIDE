using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    private MeshFilter MeshBack;
    private BoxCollider BackCollider;
    private Mesh meshsback;

    public void Init(Vector3[] vertices)
    {
        MeshBack = gameObject.AddComponent<MeshFilter>();

        // BackCollider = gameObject.AddComponent<BoxCollider>();
        // BackCollider.center = new Vector3(size,size/2F,size/2F);
        // BackCollider.size = new Vector3(0F,size,size);

        int[] mytriangles = new int[6]{
            7,4,6,
            7,5,4,
        };

        meshsback = new Mesh();
        meshsback.vertices = vertices;
        meshsback.triangles = mytriangles;
        meshsback.RecalculateNormals();
        MeshBack.mesh = meshsback;
    }
    public void changeBackHeight(Vector3[] height){
        meshsback.vertices = height;
    }
}
