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

public class Top : MonoBehaviour
{
    //Pour le hover on 
    public Material mat;
    public Material originalMat;
    RaycastHit ray;
    private XRSimpleInteractable simpleInteract;

    // Test Grab
    private XRGrabInteractable _grab;

    //Component 
    private Rigidbody rb;
    private MeshFilter _meshFilter;
    private BoxCollider _collider;
    private MeshRenderer _MeshRenderer ;

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


        //The cube
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
        meshs.RecalculateNormals();


        interact();
        // UpperTranslate();


        StartChild();
    }

    private void UpperTranslate(){
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY |
         RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        _grab = gameObject.AddComponent<XRGrabInteractable>();
        _grab.throwOnDetach = false;
        _grab.forceGravityOnDetach = false;
        _grab.useDynamicAttach= true;
        _grab.matchAttachRotation = false;
    }

    private void interact(){
        simpleInteract = gameObject.AddComponent<XRSimpleInteractable>();
        simpleInteract.hoverEntered.AddListener(OnHoverEntered);
        simpleInteract.hoverExited.AddListener(OnHovertExited);
        simpleInteract.selectEntered.AddListener(OnSelectEntered);
    }

    public void OnHoverEntered(HoverEnterEventArgs interactor)
    {
        Debug.Log($"{interactor.interactorObject} hovered over {interactor.interactableObject}", this);
        _MeshRenderer.material = mat;
    }

    public void OnHovertExited(HoverExitEventArgs interactor){
        Debug.Log($"{interactor.interactorObject} exit over {interactor.interactableObject}", this);
        _MeshRenderer.material = originalMat;
    }

    private void OnSelectEntered(SelectEnterEventArgs interactor)
    {
        Debug.Log($"position current 3D Rayhit{ray.point}");
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

    public Vector3[] getVerctices(){
        return vertices;
    }
}
