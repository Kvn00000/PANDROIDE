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
 
    public void DrawWall(int sides, float radius,int height)
    {
        mesh = new Mesh();
        _meshCollider = gameObject.AddComponent<MeshCollider>();
        this.GetComponent<MeshFilter>().mesh = mesh;

        points = GetPoints(sides,radius,height).ToArray();
        _triangles = DrawTriangles(points);

        mesh.vertices = points;
        mesh.triangles = _triangles;
        _meshCollider.sharedMesh = mesh;
    }
 

    
    List<Vector3> GetPoints(int sides, float radius,int height)   
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







}