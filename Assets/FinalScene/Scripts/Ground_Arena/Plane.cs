using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Plane : MonoBehaviour
{

    //Components
    private MeshFilter _meshFilter;
    private MeshCollider _collider;

    Mesh meshs;
    protected Vector3[] vertices;

    public void Init(int sides,float radius){
        //Add Component
        _meshFilter = gameObject.AddComponent<MeshFilter>();
        _collider = gameObject.AddComponent<MeshCollider>();

        vertices = GetPoints(sides,radius);

        int[] triangles = DrawTriangles(vertices);

        meshs = new Mesh();
        meshs.vertices = vertices;
        meshs.triangles = triangles;
        _meshFilter.mesh = meshs;

        //Collider prend la forme du mesh
        _collider.sharedMesh = meshs; 
    }



    Vector3[] GetPoints(int sides, float radius)
    {
        List<Vector3> points = new List<Vector3>();
        float PointStep = (float)1 / sides;
        float TAU = 2 * Mathf.PI;
        //Ecart entre deux points
        float radianStep = PointStep * TAU;

        for (int i = 0; i < sides; i++){
            float currentRadian = radianStep * i;
            points.Add(new Vector3(Mathf.Cos(currentRadian) * radius, 0, Mathf.Sin(currentRadian) * radius));
        }
        return points.ToArray();
    }

    int[] DrawTriangles(Vector3[] points){
        int triangleAmount = points.Length - 2;
        List<int> newTriangles = new List<int>();
        for(int i = 0; i < triangleAmount; i++){
            newTriangles.Add(0);
            newTriangles.Add(i + 2);
            newTriangles.Add(i + 1);
        }
        return newTriangles.ToArray();
    }
}
