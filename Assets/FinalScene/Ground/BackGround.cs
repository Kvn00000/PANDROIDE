using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    // Arriere du cube avec une layer Mur pour la detection du boid sur un mur
    private MeshFilter MeshBack;
    private MeshCollider BackCollider;
    private Mesh meshsback;

    public void Init(Vector3[] vertices)
    {
        MeshBack = gameObject.AddComponent<MeshFilter>();

        //Une seule face du cube (Back)
        int[] mytriangles = new int[6]{
            7,4,6,
            7,5,4,
        };

        meshsback = new Mesh();
        meshsback.vertices = vertices;
        meshsback.triangles = mytriangles;

        meshsback.RecalculateNormals();
        MeshBack.mesh = meshsback;

        //Pour la detection
        BackCollider = gameObject.AddComponent<MeshCollider>();
        BackCollider.sharedMesh = meshsback;
    }

    public void changeBackHeight(Vector3[] height){
        meshsback.vertices = height;
    }
}
