using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
[RequireComponent(typeof(ARPlaneManager))]
public class ScenePlaneDetectController : MonoBehaviour
{
    // Le SerializeField permet de le voir dans l'�diteur de unity
    [SerializeField]
    private InputActionReference togglePlanesDetectedAction;
    [SerializeField]
    private GameObject toSpawn;

    private ARPlaneManager _planeManager;
    private bool isOn = false;
    private int numberOfAddedPlane = 0;
    public bool damier = false;
    // Start is called before the first frame update
    void Start()
    {
        _planeManager = GetComponent<ARPlaneManager>();
        if(_planeManager is null)
        {
            Debug.LogError("ARPlaneManager not found");
        }
        // On s'abonne aux �v�nements --> Ne pas oublier de se d�sabonner dans onDestroy()
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
            foreach(var plane in _planeManager.trackables)
            {
                numberOfAddedPlane++;
                PrintPanelLabel(plane);
                Vector3 spawnPosition;
                if (plane.classification == UnityEngine.XR.ARSubsystems.PlaneClassification.Table)
                {
                    //Debug.Log("Table Found");
                    spawnPosition = plane.center;
                    spawnPosition.y +=0.0001f;
                    float sizeTable = plane.extents.sqrMagnitude;
                    GameObject scene=Instantiate(toSpawn, spawnPosition, Quaternion.identity);
                    scene.GetComponent<InitSceneScript>().Init(spawnPosition, sizeTable,damier);
                }
            }
            Debug.Log("Number of Planes " + _planeManager.trackables.count);
            Debug.Log("Number of planes found " + numberOfAddedPlane);

        }
    }

    private void PrintPanelLabel(ARPlane plane)
    {
        string label= plane.classification.ToString();
        float meter = plane.size.sqrMagnitude;
        Vector2 ex = plane.extents;
        string log = $"Plane ID : { plane.trackableId}, Label : {label}, Size : {meter}, pos : {ex.sqrMagnitude}";
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
    { }
}

