using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BackColor : MonoBehaviour
{
    private MeshFilter MeshBack;
    private MeshRenderer _MeshRenderer ;

    private Mesh meshsback;
    private float size = 0.5f;

    public void Start()
    {
        MeshBack = gameObject.AddComponent<MeshFilter>();

        //Une seule face du cube (Back)
        int[] mytriangles = new int[6]{
            7,4,6,
            7,5,4,
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

        meshsback = new Mesh();
        meshsback.vertices = vertices;
        meshsback.triangles = mytriangles;

        meshsback.RecalculateNormals();
        MeshBack.mesh = meshsback;

    }

    public void setNewMesh(Material newMat){
         _MeshRenderer.material = newMat;
    }


}
