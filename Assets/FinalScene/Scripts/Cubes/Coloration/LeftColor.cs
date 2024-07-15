using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftColor : MonoBehaviour
{
    //We add a new child for each side to change the color of the cube's side when grabbed

    private MeshFilter MeshLeft;
    private Mesh meshsleft;
    private MeshRenderer _MeshRenderer ;

    private float size = 0.5f;

    public void Start()
    {
        MeshLeft = gameObject.AddComponent<MeshFilter>();

        //Left
        int[] mytriangles = new int[6]{
            //Add the triangles clockwise
            5,0,4,
            5,1,0,
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


        meshsleft = new Mesh();
        meshsleft.RecalculateNormals();
        meshsleft.vertices = vertices;
        meshsleft.triangles = mytriangles;
        MeshLeft.mesh = meshsleft;

    }

    public void setNewMesh(Material newMat){
         _MeshRenderer.material = newMat;
    }

}
