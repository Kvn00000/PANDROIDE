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

public class Top : MonoBehaviour{

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

    //Childs
    private RightGround right;
    private LeftGround left;
    private FrontGround front;
    private BackGround back;


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

        //Pour la reflection de la lumiere
        // meshs.RecalculateNormals();
        
        //On ajoute l'interaction et on ajoute les autres faces de layer Mur
        simpleInteract = gameObject.AddComponent<Pull>();
        StartChild();
    }


    public void updateVertices(float height){
        // On met a jour les vertices
        vertices = new Vector3[8]{
            new Vector3(-size, -size, -size),
            new Vector3(-size, size+height, -size),
            new Vector3(size, -size, -size),
            new Vector3(size, size+height, -size),
            new Vector3(-size, -size, size),
            new Vector3(-size, size+height, size),
            new Vector3(size, -size, size),
            new Vector3(size,size+height, size)
        };

        meshs.vertices = vertices;
        _collider.size = new Vector3(size*2F,2*size+height,size*2F);
        _collider.center = new Vector3(0,height*0.5F,0);

        //On met a jour les autres faces Mur
        left.changeLeftHeight(vertices);
        right.changeRightHeight(vertices);
        back.changeBackHeight(vertices);
        front.changeFrontHeight(vertices);
    }

    public void setNewMesh(Material newMat){
         _MeshRenderer.material = newMat;
    }

    private void StartChild(){
        front = GetComponentInChildren<FrontGround>();
        front.Init(vertices);
        left = GetComponentInChildren<LeftGround>();
        left.Init(vertices);
        right = GetComponentInChildren<RightGround>();
        right.Init(vertices);
        back = GetComponentInChildren<BackGround>();
        back.Init(vertices);
    }

}
