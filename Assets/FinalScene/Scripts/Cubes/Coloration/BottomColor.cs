using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomColor : MonoBehaviour
{
    private MeshFilter MeshBottom;
    private MeshRenderer _MeshRenderer ;

    private Mesh meshsBottom;

    private float size = 0.5f;

    public void Start()
    {
        MeshBottom = gameObject.AddComponent<MeshFilter>();

        //Une seule face du cube (Back)
        int[] mytriangles = new int[6]{
            0,6,4,
            0,2,6
        };

        Vector3[] vertices = new Vector3[8]{
            new Vector3(-size, -size, -size),
            new Vector3(-size, size, -size),
            new Vector3(size, -size, -size),
            new Vector3(size, size, -size),
            new Vector3(-size, -size, size),
            new Vector3(-size, size, size),
            new Vector3(size, -size, size),
            new Vector3(size, size, size)
        };

        meshsBottom = new Mesh();
        meshsBottom.vertices = vertices;
        meshsBottom.triangles = mytriangles;

        meshsBottom.RecalculateNormals();
        MeshBottom.mesh = meshsBottom;

    }

    public void setNewMesh(Material newMat){
         _MeshRenderer.material = newMat;
    }

}
