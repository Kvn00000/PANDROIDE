using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontColor : MonoBehaviour
{
    //We add a new child for each side to change the color of the cube's side when grabbed
    
    private MeshFilter MeshFront;
    private MeshRenderer _MeshRenderer ;

    private Mesh meshsfront;

    private float size = 0.5f;
    public void Start()
    {
        MeshFront = gameObject.AddComponent<MeshFilter>();

        //Front
        int[] mytriangles = new int[6]{
            //Add the triangles clockwise
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
