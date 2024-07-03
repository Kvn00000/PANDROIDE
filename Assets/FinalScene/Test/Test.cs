using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Mesh meshs;

    private MeshFilter _meshFilter;
    private MeshCollider _collider;
    private MeshRenderer _MeshRenderer ;


    protected Vector3[] vertices;

    // Start is called before the first frame update
    void Start(){
        _meshFilter = gameObject.AddComponent<MeshFilter>();
        _collider = gameObject.AddComponent<MeshCollider>();
        _MeshRenderer = gameObject.GetComponent<MeshRenderer>();

        vertices = GetPoints(4, 1f, 1.5f, 1).ToArray();

        int[] triangles = DrawTriangles(vertices);


        meshs = new Mesh();
        meshs.vertices = vertices;
        meshs.triangles = triangles;
        _meshFilter.mesh = meshs;

        _collider.sharedMesh = meshs;
    }


    int[] DrawTriangles(Vector3[] points)
    {
        int triangleAmount = points.Length;
        List<int> newTriangles = new List<int>();
        for (int i = 0; i < triangleAmount; i = i + 2)
        {
            //Interieur
            newTriangles.Add(i);
            newTriangles.Add((i + 5) % triangleAmount);
            newTriangles.Add((i + 1) % triangleAmount);

            newTriangles.Add(i);
            newTriangles.Add((i + 4) % triangleAmount);
            newTriangles.Add((i + 5) % triangleAmount);

            //Top
            newTriangles.Add((i + 2) % triangleAmount);
            newTriangles.Add((i + 4) % triangleAmount);
            newTriangles.Add((i) % triangleAmount);

            newTriangles.Add((i + 2) % triangleAmount);
            newTriangles.Add((i + 6) % triangleAmount);
            newTriangles.Add((i + 4) % triangleAmount);


            //Ext
            newTriangles.Add((i + 3) % triangleAmount);
            newTriangles.Add((i + 6) % triangleAmount);
            newTriangles.Add((i + 2) % triangleAmount);

            newTriangles.Add((i + 3) % triangleAmount);
            newTriangles.Add((i + 7) % triangleAmount);
            newTriangles.Add((i + 6) % triangleAmount);


            //Bottom
            newTriangles.Add((i + 1) % triangleAmount);
            newTriangles.Add((i + 7) % triangleAmount);
            newTriangles.Add((i + 3) % triangleAmount);

            newTriangles.Add((i + 1) % triangleAmount);
            newTriangles.Add((i + 5) % triangleAmount);
            newTriangles.Add((i + 7) % triangleAmount);

        }
        return newTriangles.ToArray();
    }



    List<Vector3> GetPoints(int sides, float radius,float radiusExt, float height)
    {
        List<Vector3> points = new List<Vector3>();
        float PointStep = (float)1 / sides;
        float TAU = 2 * Mathf.PI;
        //Ecart entre deux points
        float radianStep = PointStep * TAU;

        for (int i = 0; i < sides; i++)
        {
            float currentRadian = radianStep * i;
            points.Add(new Vector3(Mathf.Cos(currentRadian) * radius, height, Mathf.Sin(currentRadian) * radius));
            points.Add(new Vector3(Mathf.Cos(currentRadian) * radius, 0, Mathf.Sin(currentRadian) * radius));

            points.Add(new Vector3(Mathf.Cos(currentRadian) * radiusExt, height, Mathf.Sin(currentRadian) * radiusExt));
            points.Add(new Vector3(Mathf.Cos(currentRadian) * radiusExt, 0, Mathf.Sin(currentRadian) * radiusExt));
        }
        return points;
    }
}
