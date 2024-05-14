using UnityEngine;


public class Plane : MonoBehaviour
{

    //Components
    private MeshFilter _meshFilter;
    private BoxCollider _collider;

    Mesh meshs;
    protected Vector3[] vertices;

    public void Init(float size){
        //Add Component
        _meshFilter = gameObject.AddComponent<MeshFilter>();
        _collider = gameObject.AddComponent<BoxCollider>();
        _collider.center = new Vector3(0,size,0);
        _collider.size = new Vector3(size*2,0.001F,size*2);

        //The cube
        vertices = new Vector3[4]{
            new Vector3(-size, size, -size),
            new Vector3(size, size, -size),
            new Vector3(-size, size, size),
            new Vector3(size, size, size)
        };


        int[] triangles = new int[6]{
            //Add the triangles clockwise
            0,2,3,
            0,3,1
        };

        meshs = new Mesh();
        meshs.vertices = vertices;
        meshs.triangles = triangles;
        _meshFilter.mesh = meshs;
    }
}
