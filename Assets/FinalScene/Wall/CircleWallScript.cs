using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CircleWallScript : MonoBehaviour
{
    //mesh properties
    Mesh mesh;
    private MeshCollider _meshCollider;
    public Vector3[] points;

    private Vector3[] pointsInt;
    private Vector3[] extpoints;

    public int[] _triangles;
    private List<Vector3> _topPoints;
    private List<Vector3> _bottomPoints;

    private float height;
    private bool interior = false;

    public void DrawWall(int sides, float radius, float height, bool intern = false)
    {
        mesh = new Mesh();
        _meshCollider = gameObject.GetComponent<MeshCollider>();

        points = GetPoints(sides, radius, height).ToArray();

        _triangles = DrawTriangles(points);
        this.interior = intern;
        mesh.vertices = points;
        mesh.triangles = _triangles;

        _meshCollider.sharedMesh = mesh;
        _meshCollider.convex = true;
        this.GetComponent<MeshFilter>().mesh = mesh;

        setHeight(height);
    }
    public void DrawWall2(int sides, float radius,float height,bool intern=false)
    {
        mesh = new Mesh();
        _meshCollider = gameObject.GetComponent<MeshCollider>();

        points = GetPoints(sides, radius,radius+0.03f, height).ToArray();

        _triangles = DrawTriangles2(points);
        this.interior = intern; 
        mesh.vertices = points;
        mesh.triangles = _triangles;
        
        _meshCollider.sharedMesh = mesh;
        this.GetComponent<MeshFilter>().mesh = mesh;

        setHeight(height);
    }

    public void DrawTop(List<Vector3> points)
    {
        mesh = new Mesh();
        _meshCollider = gameObject.GetComponent<MeshCollider>();

        this.points = points.ToArray();
        _triangles = DrawTriangles(this.points);

        mesh.vertices = this.points;
        mesh.triangles = _triangles;
        _meshCollider.sharedMesh = mesh;
        this.GetComponent<MeshFilter>().mesh = mesh;
        setHeight(this.points[0].y);
    }

    
    List<Vector3> GetPoints(int sides, float radius,float height)   
    {
        List<Vector3> points = new List<Vector3>();
        float PointStep = (float)1/sides;
        float TAU = 2*Mathf.PI;
        //Ecart entre deux points
        float radianStep = PointStep*TAU;
        
        for(int i = 0; i<sides; i++)
        {
            float currentRadian = radianStep*i;
            points.Add(new Vector3(Mathf.Cos(currentRadian)*radius,height, Mathf.Sin(currentRadian)*radius));
            points.Add(new Vector3(Mathf.Cos(currentRadian)*radius,0, Mathf.Sin(currentRadian)*radius));
        }
        return points;
    }
    List<Vector3> GetPoints(int sides, float radiusint, float radiusext, float height)
    {
        List<Vector3> points = new List<Vector3>();
        float PointStep = (float)1 / sides;
        float TAU = 2 * Mathf.PI;
        //Ecart entre deux points
        float radianStep = PointStep * TAU;

        for (int i = 0; i < sides; i++)
        {
            float currentRadian = radianStep * i;
            points.Add(new Vector3(Mathf.Cos(currentRadian) * radiusint, height, Mathf.Sin(currentRadian) * radiusint));
            points.Add(new Vector3(Mathf.Cos(currentRadian) * radiusint, 0, Mathf.Sin(currentRadian) * radiusint));
            points.Add(new Vector3(Mathf.Cos(currentRadian) * radiusext, height, Mathf.Sin(currentRadian) * radiusext));
            points.Add(new Vector3(Mathf.Cos(currentRadian) * radiusext, 0, Mathf.Sin(currentRadian) * radiusext));
        }
        return points;
    }


    int[] DrawTriangles(Vector3[] points)
    {   
        int triangleAmount = points.Length;
        List<int> newTriangles = new List<int>();
        for(int i = 0; i<triangleAmount; i = i+2)
        {
            newTriangles.Add(i);
            newTriangles.Add((i+1)%triangleAmount);
            newTriangles.Add((i+3)%triangleAmount);

            newTriangles.Add(i);
            newTriangles.Add((i+3)%triangleAmount);
            newTriangles.Add((i+2)%triangleAmount);

        }
        return newTriangles.ToArray();
    }
    int[] DrawTriangles2(Vector3[] points)
    {
        int triangleAmount = points.Length;
        List<int> newTriangles = new List<int>();
        for (int i = 0; i < triangleAmount; i = i + 4)
        {
            //0 5 1
            newTriangles.Add(i);
            newTriangles.Add((i + 5) % triangleAmount);
            newTriangles.Add((i + 1) % triangleAmount);
            //0 4 5
            newTriangles.Add(i);
            newTriangles.Add((i + 4) % triangleAmount);
            newTriangles.Add((i + 5) % triangleAmount);

            //2 4 0 
            newTriangles.Add(i+2);
            newTriangles.Add((i + 4) % triangleAmount);
            newTriangles.Add((i) % triangleAmount);
            //2 6 4
            newTriangles.Add(i+2);
            newTriangles.Add((i + 6) % triangleAmount);
            newTriangles.Add((i + 4) % triangleAmount);

            //3 6 2
            newTriangles.Add(i + 3);
            newTriangles.Add((i + 6) % triangleAmount);
            newTriangles.Add((i + 2) % triangleAmount);
            //3 7 6
            newTriangles.Add(i + 3);
            newTriangles.Add((i + 7) % triangleAmount);
            newTriangles.Add((i + 6) % triangleAmount);

            //1 7 3
            newTriangles.Add(i + 1);
            newTriangles.Add((i + 7) % triangleAmount);
            newTriangles.Add((i + 3) % triangleAmount);
            //1 5 7
            newTriangles.Add(i + 1);
            newTriangles.Add((i + 5) % triangleAmount);
            newTriangles.Add((i + 7) % triangleAmount);

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
        for (int i =0; i < pointsInt.Count; i++)
        {
            finalList.Add(pointsInt[i]);
            finalList.Add(pointsExt[i]);
        }
        return finalList;
    }
    public List<Vector3> getTopPoints(Vector3[] listparcours)
    {
        List<Vector3> topPoints = new List<Vector3>();
        foreach(Vector3 v in listparcours)
        {
            if (v.y == height)
            {
                topPoints.Add(new Vector3(v.x,v.y,v.z));
            }
        }
        return topPoints;
    }
    public List<Vector3> getBottomPoints(Vector3[] listparcours)
    {
        List<Vector3> topPoints = new List<Vector3>();
        foreach (Vector3 v in listparcours)
        {
            if (v.y != height)
            {
                topPoints.Add(new Vector3(v.x, v.y, v.z));
            }
        }
        return topPoints;
    }
    public void setNewMesh(Material newMat)
    {
        this.GetComponent<MeshRenderer>().material = newMat;
    }
    public bool isInterrior()
    {
        return interior;
    }
    /*
    public void UpdateTopArenaWall() {
        CircleWallScript[] children= this.transform.GetComponentsInChildren<CircleWallScript>();
        List<Vector3> inter = children[0].getTopPoints();
        List<Vector3> exter = children[1].getTopPoints();
        Vector3[] newpoints = CircleWallScript.mergeTwoVerticesList(inter, exter).ToArray();
        this.DrawTriangles(newpoints);
    }
    */
}