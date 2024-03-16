using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TopBox : MonoBehaviour
{
    private MeshFilter _meshFilter;
    private Rigidbody rb;
    private BoxCollider TopCollider;
    protected static float size = 1F;

    protected static Vector3[] vertices;

    // Start is called before the first frame update
    public void Start()
    {

        _meshFilter = gameObject.AddComponent<MeshFilter>();

        /*
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        */

        TopCollider = gameObject.AddComponent<BoxCollider>();
        TopCollider.center = new Vector3(size/2F,size,size/2F);
        TopCollider.size = new Vector3(size,0F,size);
        
        vertices =vertices = new Vector3[8]{
            new Vector3(0, size, 0),
            new Vector3(0, 0, 0),
            new Vector3(0, size, size),
            new Vector3(0, 0, size),
            new Vector3(size, size, 0),
            new Vector3(size, 0, 0),
            new Vector3(size, size, size),
            new Vector3(size, 0, size),
        };

        //Je sais pas ca sert a quoi uv mdr
        /*
        Vector2[] uv = new Vector2[4]{
            new Vector2(0,0),           
            new Vector2(0, size),
            new Vector2(size, 0),           
            new Vector2(size, size),
        };
        */

        int[] triangles = new int[6]{
            //Add the triangles clockwise
            4,0,2,
            6,4,2,
            
        };
        Mesh meshs = new Mesh();
        meshs.vertices = vertices;
        //meshs.uv = uv;
        meshs.triangles = triangles;
        transform.localScale = new Vector3((float)size,1f,(float)size);
        GetComponent<MeshFilter>().mesh = meshs;
    }
}
