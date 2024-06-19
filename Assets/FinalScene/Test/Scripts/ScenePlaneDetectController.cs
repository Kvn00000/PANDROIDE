using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
[RequireComponent(typeof(ARPlaneManager))]
public class ScenePlaneDetectController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference togglePlanesDetectedAction;
    private ARPlaneManager _planeManager;
    private bool isOn = false;
    private int numberOfAddedPlane = 0;
    // Start is called before the first frame update
    void Start()
    {
        _planeManager = GetComponent<ARPlaneManager>();
        if(_planeManager is null)
        {
            Debug.LogError("ARPlaneManager not found");
        }
        togglePlanesDetectedAction.action.performed += OnTogglePlanesAction;
        _planeManager.planesChanged += OnPlanesChanged;
    }


    private void OnTogglePlanesAction(InputAction.CallbackContext obj)
    {
        // Change la valeur du bool qui indique si l'on affiche ou non
        isOn = !isOn;
        float alphaValueFill;
        float alphaValueLine;
       if(isOn)
       {
            alphaValueFill = 0.3f;
            alphaValueLine = 1.0f;
        }
        else
        {
            alphaValueFill = 0.0f;
            alphaValueLine = 0.0f;
        }
       foreach(var plane in _planeManager.trackables)
        {
            SetPlaneAlpha(plane, alphaValueFill, alphaValueLine);
        }

    }

    private void SetPlaneAlpha(ARPlane plane, float alphaValueFill, float alphaValueLine)
    {
        var meshRenderer = plane.GetComponentInChildren<MeshRenderer>();
        var lineRenderer = plane.GetComponentInChildren<LineRenderer>();
        if (meshRenderer!= null)
        {
            Color color = meshRenderer.material.color;
            color.a = alphaValueFill;
            meshRenderer.material.color = color;
        }
        if (lineRenderer !=null)
        {
            // Init couleur depart
            Color startColor = lineRenderer.startColor;
            Color endColor = lineRenderer.endColor;
            // Affectation des valeurs de alpha
            startColor.a = alphaValueLine;
            endColor.a = alphaValueLine;
            //Application changements
            lineRenderer.startColor = startColor;
            lineRenderer.endColor = endColor;
        }
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs arguments)
    {
        if (arguments.added.Count > 0)
        {
            numberOfAddedPlane++;
            foreach(var plane in _planeManager.trackables)
            {
                PrintPanelLabel(plane);
            }
            Debug.Log("Number of Planes " + _planeManager.trackables.count);
            Debug.Log("Number of planes found " + numberOfAddedPlane);
        }
    }

    private void PrintPanelLabel(ARPlane plane)
    {
        string label= plane.classification.ToString();
        string log = $"Plane ID : { plane.trackableId}, Label : {label}";
        Debug.Log(log);
    }

    private void OnDestroy()
    {
        Debug.Log("Calling Destructor for ScenePlaneDetectController");
        togglePlanesDetectedAction.action.performed -= OnTogglePlanesAction;
        _planeManager.planesChanged -= OnPlanesChanged;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
