using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontColor : MonoBehaviour
{
    private MeshFilter MeshFront;
    private MeshRenderer _MeshRenderer ;

    private Mesh meshsfront;

    private float size = 0.5f;
    public void Start()
    {
        MeshFront = gameObject.AddComponent<MeshFilter>();

        //Face Avant du cube
        int[] mytriangles = new int[6]{
            //Add the triangles clockwise
            1,2,0,
            1,3,2,
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

        meshsfront = new Mesh();
        meshsfront.vertices = vertices;
        meshsfront.triangles = mytriangles;
        meshsfront.RecalculateNormals();
        MeshFront.mesh = meshsfront;

    }

    public void setNewMesh(Material newMat){
         _MeshRenderer.material = newMat;
    }

}
