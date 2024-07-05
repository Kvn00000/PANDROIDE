using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightColor : MonoBehaviour
{
    //Coté droit du cube sur la layer Mur
    private MeshFilter MeshRight;
    private Mesh meshsright;
    private MeshRenderer _MeshRenderer ;

    private float size = 0.5f;


    public void Start()
    {
        MeshRight = gameObject.AddComponent<MeshFilter>();


        // Droite
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
