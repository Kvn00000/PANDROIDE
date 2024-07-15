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

    private float thickness;
    private float height;
    public bool inter = false;
    private float radius;
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
        this.radius = radius;
        setHeight(height);
    }

    public void DrawTop(List<Vector3> points)
    {
        mesh = new Mesh();
        if (gameObject.GetComponent<MeshCollider>() != null)
        {
            Destroy(gameObject.GetComponent<MeshCollider>());
        }
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
        //gap between two points
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

    public void updateTop() {
        GameObject parent = this.transform.parent.gameObject;
        //Debug.Log("Entering update top");
        CircleWallScript topWall = parent.GetComponent<CircleWallScript>();
        List<Vector3> circleInt = getTopPoints();
        List<Vector3> circleIntNpos = new List<Vector3>();
        Vector3 sc=this.transform.localScale;
        //Debug.Log("Scale = " + sc);
        foreach (Vector3 p in circleInt)
        {
            Vector3 newPoint = new Vector3(p.x*sc.x,p.y*sc.y,p.z*sc.z);
            Debug.DrawLine(p, newPoint, Color.magenta);
            circleIntNpos.Add(newPoint);
        }
        List<Vector3> circleExt = parent.transform.GetChild(1).GetComponent<CircleWallScript>().getTopPoints();
        List<Vector3> mergedList = CircleWallScript.mergeTwoVerticesList(circleIntNpos, circleExt);
        float newThickness = Vector3.Distance(circleInt[0], circleExt[0]);

        topWall.DrawTop(mergedList);
        topWall.setThickness(newThickness);
    }
    public void setThickness(float value)
    {
        this.thickness = value;
    }
    public float getThickness()
    {
        return thickness;
    }
    public float getRadius()
    {
        return this.radius;
    }
}