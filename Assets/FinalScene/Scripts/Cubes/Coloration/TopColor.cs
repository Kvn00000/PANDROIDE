using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopColor : MonoBehaviour
{
    //We add a new child for each side to change the color of the cube's side when grabbed

    private MeshFilter MeshTop;
    private Mesh meshsTop;
    private MeshRenderer _MeshRenderer ;

    private float size = 0.5f;


    public void Start()
    {
        MeshTop = gameObject.AddComponent<MeshFilter>();

        Vector3[] vertice = new Vector3[8]{
            new Vector3(-size, -size, -size),
            new Vector3(-size, size, -size),
            new Vector3(size, -size, -size),
            new Vector3(size, size, -size),
            new Vector3(-size, -size, size),
            new Vector3(-size, size, size),
            new Vector3(size, -size, size),
            new Vector3(size, size, size)
        };


        // Top
        int[] mytriangles = new int[6]{
            //Add the triangles clockwise
            5,3,1,
            5,7,3,
        };

        meshsTop = new Mesh();
        meshsTop.vertices = vertice;
        meshsTop.triangles = mytriangles;
        meshsTop.RecalculateNormals();
        MeshTop.mesh = meshsTop;

    }


    public void setNewMesh(Material newMat){
         _MeshRenderer.material = newMat;
    }

}
