using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MurScript : MonoBehaviour
{

    private MeshFilter _meshFilter;

    private float _dx = 5F;
    private float _dy = 5F;
    // Start is called before the first frame update
    void Start()
    {
        _meshFilter = gameObject.AddComponent<MeshFilter>();


        //Wall made of 2 triangles
        Vector3[] vertices = new Vector3[4]{
            new Vector3(0, 0, 0),
            new Vector3(0, _dy, 0),           
            new Vector3(_dx, 0, 0),           
            new Vector3(_dx, _dy,0), 
        };

        Vector2[] uv = new Vector2[4]{
            new Vector2(0,0),           
            new Vector2(0, _dy),
            new Vector2(_dx, 0),           
            new Vector2(_dx, _dy),
        };
 
        int[] triangles = new int[6]{
            //Add the triangles clockwise
            1,3,2,
            0,1,2,
        };


        Mesh meshs = new Mesh();
        meshs.vertices = vertices;
        meshs.uv = uv;
        meshs.triangles = triangles;

        transform.localScale = new Vector3(_dx,_dy,1f);

        GetComponent<MeshFilter>().mesh = meshs;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
