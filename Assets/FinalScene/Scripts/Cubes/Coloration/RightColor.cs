using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightColor : MonoBehaviour
{
    //We add a new child for each side to change the color of the cube's side when grabbed
    
    private MeshFilter MeshRight;
    private Mesh meshsright;
    private MeshRenderer _MeshRenderer ;

    private float size = 0.5f;


    public void Start()
    {
        MeshRight = gameObject.AddComponent<MeshFilter>();


        // Right
        int[] mytriangles = new int[6]{
            //Add the triangles clockwise
            3,6,2,
            3,7,6,
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

        meshsright = new Mesh();
        meshsright.vertices = vertices;
        meshsright.triangles = mytriangles;
        meshsright.RecalculateNormals();
        MeshRight.mesh = meshsright;

    }

    public void setNewMesh(Material newMat){
         _MeshRenderer.material = newMat;
    }

}
