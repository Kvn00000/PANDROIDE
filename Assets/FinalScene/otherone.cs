using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class otherone : MonoBehaviour
{
    private MeshFilter _meshFilter;
    private BoxCollider col;
    private float size = 0.5F;

    Mesh meshs;

    protected static Vector3[] vertices;

    // Start is called before the first frame update
    public void Start(){
        _meshFilter = gameObject.AddComponent<MeshFilter>();
        col = gameObject.AddComponent<BoxCollider>();

        vertices = new Vector3[8]{
            new Vector3(-size, -size, -size),
            new Vector3(-size, size, -size),
            new Vector3(size, -size, -size),
            new Vector3(size, size, -size),
            new Vector3(-size, -size, size),
            new Vector3(-size, size, size),
            new Vector3(size, -size, size),
            new Vector3(size, size, size)
        };

        int[] triangles = new int[6]{
            5,0,4,
            5,1,0,
        };
        meshs = new Mesh();

        meshs.vertices = vertices;
        //meshs.uv = uv;
        meshs.triangles = triangles;
        // GetComponent<MeshFilter>().mesh = meshs;
        _meshFilter.mesh = meshs;

    }
}
