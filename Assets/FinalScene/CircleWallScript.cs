using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CircleWallScript : MonoBehaviour
{
    //mesh properties
    Mesh mesh;
    private MeshCollider _meshCollider;
    public Vector3[] polygonPoints;
    public int[] polygonTriangles;
 
    //polygon properties
    public int polygonSides;
    public float polygonRadius;
    
    
    
    
    // void Update()
    // {
    //     DrawWall(polygonSides,polygonRadius);
    // }
 
    public void DrawWall(int sides, float radius,int height)
    {
        mesh = new Mesh();
        _meshCollider = gameObject.AddComponent<MeshCollider>();
        this.GetComponent<MeshFilter>().mesh = mesh;
        polygonPoints = GetPoints(sides,radius,height).ToArray();
        polygonTriangles = DrawTriangles(polygonPoints);

        mesh.vertices = polygonPoints;
        mesh.triangles = polygonTriangles;

        _meshCollider.sharedMesh = mesh;
    }
 

    
    List<Vector3> GetPoints(int sides, float radius,int height)   
    {
        List<Vector3> points = new List<Vector3>();
        float PointStep = (float)1/sides;
        float TAU = 2*Mathf.PI;
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