using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BackColor : MonoBehaviour
{
    //We add a new child for each side to change the color of the cube's side when grabbed
    private MeshFilter MeshBack;
    private MeshRenderer _MeshRenderer ;

    private Mesh meshsback;
    private float size = 0.5f;

    public void Start()
    {
        MeshBack = gameObject.AddComponent<MeshFilter>();

        // (Back)
        int[] mytriangles = new int[6]{
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
