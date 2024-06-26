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
    private GameObject _arena;
    private float _planeSize;
    private float _arenaSize;
    private bool _arenaSpawned=false;
    private Vector3 _arenaSpawnPos;
    // Mode : 0 --> Automatique ; 1 --> Manuel 
    private int _mode = 1;
    //
    void Start()
    {
        _planeManager = GetComponent<ARPlaneManager>();
        if(_planeManager is null)
        {
            Debug.LogError("ARPlaneManager not found");
        }
        if (PlayerPrefs.HasKey("Mode"))
        {
            _mode = PlayerPrefs.GetInt("Mode");
            
        }
        if (PlayerPrefs.HasKey("ArenaSize"))
        {
            _arenaSize = PlayerPrefs.GetFloat("ArenaSize");
            Debug.Log("Found Value for ArenaSize " + _arenaSize);
        }

        // On s'abonne aux evenements --> Ne pas oublier de se desabonner dans onDestroy()
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
                //Check if plane is a table --> Spawn an arena on it
                if (plane.classification == UnityEngine.XR.ARSubsystems.PlaneClassification.Table)
                {

                    //Debug.Log("Table Found");
                    if (!_arenaSpawned)
                    {
                        spawnPosition = plane.center;
                        //float sizeTable = plane.size.sqrMagnitude;
                        float sizeTable = plane.extents.sqrMagnitude;
                        spawnPosition.y -=0.01f;
                        plane.gameObject.layer = LayerMask.NameToLayer("SOL");
                        GameObject scene=Instantiate(toSpawn, spawnPosition, Quaternion.identity);

                        _arena = scene;
                        if (_mode == 0)
                        {
                            _arenaSize = sizeTable;
                        }
                        _planeSize = sizeTable;
                        _arenaSpawnPos = spawnPosition;
                        scene.GetComponent<InitSceneScript>().Init(_arenaSpawnPos, _arenaSize,damier);
                        _arenaSpawned = true;

                    }
                }
                //Check if plane is a ground --> Add a component that destro all other objects
                if (plane.classification == UnityEngine.XR.ARSubsystems.PlaneClassification.Floor)
                {
                    /*
                    Mesh planeMesh = new Mesh();
                    ARPlaneMeshGenerators.GenerateMesh(planeMesh, Pose.identity, plane.boundary);
                    MeshCollider planeMeshCollider=plane.gameObject.AddComponent<MeshCollider>();
                    planeMeshCollider.sharedMesh = planeMesh;
                    planeMeshCollider.isTrigger = true;
                    */
                    
                    plane.gameObject.AddComponent<BoxCollider>();
                    BoxCollider boxCollider = plane.GetComponent<BoxCollider>();
                    boxCollider.size = plane.gameObject.transform.localScale;
                    boxCollider.center = plane.center;
                    boxCollider.isTrigger = true;
                    
                    plane.gameObject.AddComponent<DestroyGroundScript>();
                    Debug.Log("Destroyer Added");
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
        string log = $"Plane ID : { plane.trackableId}, Label : {label}, Size : {meter}, pos : {ex.sqrMagnitude} \n layer : {plane.gameObject.layer}";
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
    public void ArenaChanges(float newSize)
    {
        GameObject oldArena = _arena;
        GameObject newArena = Instantiate(toSpawn, _arenaSpawnPos, Quaternion.identity);
        newArena.GetComponent<InitSceneScript>().Init(_arenaSpawnPos, newSize, damier);
        Destroy(oldArena);
        _arena = newArena;
    }
    private void OnApplicationFocus(bool focus)
    {
        if (!focus) {
            Debug.Log("Focus Lost : saving parameters ... ");
            PlayerPrefs.SetFloat("ArenaSize", _arenaSize);
            PlayerPrefs.SetInt("ModeArene", _mode);
        }
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("On Pause : saving parameters ... ");
            PlayerPrefs.SetFloat("ArenaSize", _arenaSize);
            PlayerPrefs.SetInt("ModeArene", _mode);

        }
    }
    private void OnApplicationQuit()
    {
        Debug.Log("On Quit : saving parameters ... ");
        PlayerPrefs.SetFloat("ArenaSize", _arenaSize);
        PlayerPrefs.SetInt("ModeArene", _mode);
    }
    public void ChangeMod()
    {
        if (_mode == 0)
        {
            Debug.Log("Switching from automatic to manual");
            _mode = 1;
        }
        else
        {
            Debug.Log("Switching from manual to automatic");
            _mode = 0;
            this.ArenaChanges(_planeSize);
        }
    }
}

