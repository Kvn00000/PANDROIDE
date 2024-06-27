using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildComponent : MonoBehaviour{

    private MeshFilter _meshFilter;
    private Mesh myMesh;
    public Vector3[] myVertices;


    private int[] myTriangles;


    //Child mesh
    public MeshFilter meshFilterTop;
    public MeshFilter meshFilterFront;
    public MeshFilter meshFilterLeft;
    public MeshFilter meshFilterRight;
    public MeshFilter meshFilterBack;
    public MeshFilter meshFilterBottom;

    //Child Transform
    private Transform transformTop;
    private Transform transformFront;
    private Transform transformLeft;
    private Transform transformRight;
    private Transform transformBack;
    private Transform transformBottom;

    //Child Transform
    public BoxCollider colliderTop;
    public BoxCollider colliderFront;
    public BoxCollider colliderLeft;
    public BoxCollider colliderRight;
    public BoxCollider colliderBack;
    public BoxCollider colliderBottom;


    void Awake(){
        _meshFilter = GetComponent<MeshFilter>();
        myMesh = _meshFilter.mesh;
        myVertices = new Vector3[8]{
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f)
        };

        myTriangles = new int[36]{
            //Add the triangles clockwise
            //Front
            1,2,0,
            1,3,2,
            //Top
            5,3,1,
            5,7,3,
            //left
            5,0,4,
            5,1,0,
            //Right
            3,6,2,
            3,7,6,
            //Back
            7,4,6,
            7,5,4,
            //Bottom
            0,6,4,
            0,2,6
        };

        myMesh.vertices = myVertices;
        myMesh.triangles = myTriangles;
    }
    // Start is called before the first frame update
    void Start(){
        foreach (Transform child in transform){
            if (child.gameObject.GetComponent<MeshFilter>() == null){
                switch(child.name){
                    case "Top":
                        transformTop = child;
                        meshFilterTop = child.gameObject.AddComponent<MeshFilter>();
                        colliderTop = child.gameObject.GetComponent<BoxCollider>();
                        int[] triangleTop = new int[6]{
                            5,3,1,
                            5,7,3,
                        };
                        Vector3[] verticeTop = new Vector3[4]{
                            myVertices[5],
                            myVertices[3],
                            myVertices[1],
                            myVertices[7]
                        };


                        InitOneChild(triangleTop, meshFilterTop, transformTop,CalculateMeshCenter(verticeTop) ,CalculateMeshSize(verticeTop));
                        break;
                    case "Front":
                        transformFront = child;
                        meshFilterFront = child.gameObject.AddComponent<MeshFilter>();
                        colliderFront = child.gameObject.GetComponent<BoxCollider>();

                        int[] triangleFront = new int[6]{
                            1,2,0,
                            1,3,2,
                        };

                        Vector3[] verticeFront = new Vector3[4]{
                            myVertices[1],
                            myVertices[2],
                            myVertices[0],
                            myVertices[3]
                        };
                        InitOneChild(triangleFront, meshFilterFront, transformFront, CalculateMeshCenter(verticeFront), CalculateMeshSize(verticeFront));
                        break;
                    case "Left":
                        transformLeft = child;
                        meshFilterLeft = child.gameObject.AddComponent<MeshFilter>();
                        colliderLeft = child.gameObject.GetComponent<BoxCollider>();

                        int[] triangleLeft = new int[6]{
                            5,0,4,
                            5,1,0,
                        };

                        Vector3[] verticeLeft = new Vector3[4]{
                            myVertices[5],
                            myVertices[0],
                            myVertices[4],
                            myVertices[1]
                        };
                        InitOneChild(triangleLeft, meshFilterLeft, transformLeft, CalculateMeshCenter(verticeLeft), CalculateMeshSize(verticeLeft));
                        break;
                    case "Right":
                        transformRight = child;
                        meshFilterRight = child.gameObject.AddComponent<MeshFilter>();
                        colliderRight = child.gameObject.GetComponent<BoxCollider>();

                        int[] triangleRight = new int[6]{
                            3,6,2,
                            3,7,6,
                        };

                        Vector3[] verticeRight = new Vector3[4]{
                            myVertices[3],
                            myVertices[6],
                            myVertices[2],
                            myVertices[7]
                        };
                        InitOneChild(triangleRight, meshFilterRight, transformRight, CalculateMeshCenter(verticeRight), CalculateMeshSize(verticeRight));
                        break;
                    case "Back":
                        transformBack = child;
                        meshFilterBack = child.gameObject.AddComponent<MeshFilter>();
                        colliderBack = child.gameObject.GetComponent<BoxCollider>();
                        
                        int[] triangleBack = new int[6]{
                            7,4,6,
                            7,5,4,
                        };

                        Vector3[] verticeBack = new Vector3[4]{
                            myVertices[7],
                            myVertices[4],
                            myVertices[6],
                            myVertices[5]
                        };
                        InitOneChild(triangleBack, meshFilterBack, transformBack, CalculateMeshCenter(verticeBack), CalculateMeshSize(verticeBack));
                        break;
                    case "Bottom":
                        transformBottom = child;
                        meshFilterBottom = child.gameObject.AddComponent<MeshFilter>();
                        colliderBottom = child.gameObject.GetComponent<BoxCollider>();

                        int[] triangleBottom = new int[6]{
                            0,6,4,
                            0,2,6
                        };

                        Vector3[] verticeBottom = new Vector3[4]{
                            myVertices[0],
                            myVertices[6],
                            myVertices[4],
                            myVertices[2]
                        };
                        InitOneChild(triangleBottom, meshFilterBottom, transformBottom, CalculateMeshCenter(verticeBottom), CalculateMeshSize(verticeBottom));
                        break;
                }

                Debug.Log(" les childs : " + child.GetType());
            }
        }
    }


    void InitOneChild(int[] triangles, MeshFilter m, Transform t, Vector3 center, Vector3 size){
        Mesh meshs = new Mesh();
        meshs.vertices = myVertices;
        meshs.triangles = triangles;
        m.mesh = meshs;
        BoxCollider collider = t.gameObject.GetComponent<BoxCollider>();
        // collider.center = center;
        // collider.size = size;
    }


    public Vector3 CalculateMeshSize(Vector3[] vertices)
    {
        Vector3 min = vertices[0];
        Vector3 max = vertices[0];

        foreach (Vector3 vertex in vertices)
        {
            min = Vector3.Min(min, vertex);
            max = Vector3.Max(max, vertex);
        }

        Vector3 size = max - min;
        return size;
    }


    public Vector3 CalculateMeshCenter(Vector3[] vertices)
    {
        Vector3 center = Vector3.zero;
        
        foreach (Vector3 vertex in vertices)
        {
            center += vertex;
        }
        center /= vertices.Length;
        return center;
    }

    
    public void updateVertices(Vector3[] newVertice, Vector3[] colliderVertice, MeshFilter m, BoxCollider bc){
        m.mesh.vertices = newVertice;
        myVertices = newVertice;
        bc.size = CalculateMeshSize(colliderVertice);
        bc.center = CalculateMeshCenter(colliderVertice);

        foreach (Transform child in transform){
            child.GetComponent<MeshFilter>().mesh.vertices = newVertice;
        }

    }
    
    public void updateBoxCollider(BoxCollider bc, Vector3[] colliderVertice){
        Debug.Log("et ded   nnnns");

        Debug.Log(colliderVertice[0]);
        Debug.Log(colliderVertice[1]);
        Debug.Log(colliderVertice[2]);
        Debug.Log(colliderVertice[3]);
        bc.size = CalculateMeshSize(colliderVertice);
        bc.center = CalculateMeshCenter(colliderVertice);
    }





    // private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles){
    //     Vector3 dir = point - pivot; // get point direction relative to pivot
    //     dir = Quaternion.Euler(angles)* dir; // rotate it
    //     Vector3 nPoint = dir + pivot;
    //     return nPoint; // return it
    // }



    public void resizeCube(float amount, string axis, bool inverse)
    {
        Debug.Log("Here");
        amount = amount * 0.1;
        switch (axis)
        {
            case "x":
                if (!inverse)
                {
                    Debug.Log("ICI");
                    this.transform.position = new Vector3(this.transform.position.x+(amount/2), this.transform.position.y, this.transform.position.z);
                    this.transform.localScale = new Vector3(this.transform.localScale.x+amount, this.transform.localScale.y, this.transform.localScale.z);
                }
                else
                {
                    
                    this.transform.position = new Vector3(this.transform.position.x-(amount/2), this.transform.position.y, this.transform.position.z);
                    this.transform.localScale = new Vector3(this.transform.localScale.x-amount, this.transform.localScale.y, this.transform.localScale.z);
                }
                break;
            case "y":
                if (!inverse)
                {
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + (amount / 2), this.transform.position.z);
                    this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y+amount, this.transform.localScale.z);
                }
                else
                {
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y-(amount / 2), this.transform.position.z);
                    this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y-amount, this.transform.localScale.z);
                }
                break;
            case "z":
                if (!inverse)
                {
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + (amount / 2));
                    this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z + amount);
                }
                else
                {
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z- (amount / 2));
                    this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z- amount);
                }
                break;
            default:
                break;
        
        }
   }

}