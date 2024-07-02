using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CircleWallScript : MonoBehaviour
{
    //mesh properties
    Mesh mesh;
    private MeshCollider _meshCollider;

    public Vector3[] points;
    public int[] _triangles;

    private float height;
    public void DrawWall(int sides, float radius, float height)
    {
        mesh = new Mesh();
        _meshCollider = gameObject.AddComponent<MeshCollider>();
        this.GetComponent<MeshFilter>().mesh = mesh;

        points = GetPoints(sides, radius, height).ToArray();
        _triangles = DrawTriangles(points);

        mesh.vertices = points;
        mesh.triangles = _triangles;
        _meshCollider.sharedMesh = mesh;
        setHeight(height);
    }

    public void DrawTop(List<Vector3> points)
    {
        mesh = new Mesh();
        _meshCollider = gameObject.AddComponent<MeshCollider>();
        this.GetComponent<MeshFilter>().mesh = mesh;

        this.points = points.ToArray();
        _triangles = DrawTriangles(this.points);

        mesh.vertices = this.points;
        mesh.triangles = _triangles;
        _meshCollider.sharedMesh = mesh;
        setHeight(this.points[0].y);
    }



    List<Vector3> GetPoints(int sides, float radius, float height)
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
        }
        return points;
    }


    int[] DrawTriangles(Vector3[] points)
    {
        int triangleAmount = points.Length;
        List<int> newTriangles = new List<int>();
        for (int i = 0; i < triangleAmount; i = i + 2)
        {
            newTriangles.Add(i);
            newTriangles.Add((i + 1) % triangleAmount);
            newTriangles.Add((i + 3) % triangleAmount);

            newTriangles.Add(i);
            newTriangles.Add((i + 3) % triangleAmount);
            newTriangles.Add((i + 2) % triangleAmount);


            newTriangles.Add(i);
            newTriangles.Add((i + 3) % triangleAmount);
            newTriangles.Add((i + 1) % triangleAmount);

            newTriangles.Add(i);
            newTriangles.Add((i + 2) % triangleAmount);
            newTriangles.Add((i + 3) % triangleAmount);

        }
        return newTriangles.ToArray();
    }
    private void setHeight(float height)
    {
        this.height = height;
    }


    public static List<Vector3> mergeTwoVerticesList(List<Vector3> pointsInt, List<Vector3> pointsExt)
    {
        List<Vector3> finalList = new List<Vector3>();
        for (int i = 0; i < pointsInt.Count; i++)
        {
            finalList.Add(pointsInt[i]);
            finalList.Add(pointsExt[i]);
        }
        return finalList;
    }
    public List<Vector3> getTopPoints()
    {
        List<Vector3> topPoints = new List<Vector3>();
        foreach (Vector3 v in points)
        {
            if (v.y == height)
            {
                topPoints.Add(v);
            }
        }
        return topPoints;
    }
    public void setNewMesh(Material newMat)
    {
        this.GetComponent<MeshRenderer>().material = newMat;
    }

}