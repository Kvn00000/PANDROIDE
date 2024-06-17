using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class Cube : MonoBehaviour
{
    //Pour le hover on et interaction
    public Material mat;
    public Material originalMat;
    

    //Components
    private MeshFilter _meshFilter;
    private BoxCollider _collider;
    private MeshRenderer _MeshRenderer ;
    private Pull simpleInteract;

    //Cube
    Mesh meshs;
    private float size = 0.5F;
    protected Vector3[] vertices;


    public void Start(){
        //Add Component
        _meshFilter = gameObject.AddComponent<MeshFilter>();
        _collider = gameObject.AddComponent<BoxCollider>();
        _MeshRenderer = gameObject.GetComponent<MeshRenderer>();


        //Vertices du cube
        vertices = new Vector3[8]{
            new Vector3(-size, -size, -size),
            new Vector3(-size, size, -size),
            new Vector3(size, -size, -size),
            new Vector3(size, size, -size),
            new Vector3(-size, -size, size),
            new Vector3(-size, size, size),
            new Vector3(size, -size, size),
            new Vector3(size, size, size)
        };

        //Tout les triangles
        int[] triangles = new int[36]{
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

        meshs = new Mesh();
        meshs.vertices = vertices;
        meshs.triangles = triangles;
        _meshFilter.mesh = meshs;

    }

    public void setNewMesh(Material newMat){
         _MeshRenderer.material = newMat;
    }


}
