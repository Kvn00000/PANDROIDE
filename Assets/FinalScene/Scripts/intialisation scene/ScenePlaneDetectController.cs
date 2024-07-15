using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
[RequireComponent(typeof(ARPlaneManager))]
public class ScenePlaneDetectController : MonoBehaviour
{
    //SerializeField allow private object to be seen in inspector 
    [SerializeField]
    private InputActionReference togglePlanesDetectedAction;
    [SerializeField]
    private GameObject toSpawn;
    [SerializeField]
    private GameObject planDense;

    // AR Parameters
    private ARPlaneManager _planeManager;
    private int numberOfAddedPlane = 0;
    List<ARPlane> destroList;
    GameObject underP; //Bounding box under the table
    //Mode boolean
    private bool isOn = false;
    private int _FadeOut=1;

    //Arena parameter
    private GameObject _arena;
    private bool _arenaSpawned=false;
    private float _arenaSize;
    private Vector3 _arenaSpawnPos;
    private Quaternion _arenaSpawnRotation;
    private Vector3 _arenaScale;

    //Layers Informations
    private int _layerwall;
    private int _layerground;
    /*
    Initializing all parameters 
    */

    // Start is called before the first frame update
    void Start()
    {
        _planeManager = GetComponent<ARPlaneManager>();
        if(_planeManager is null)
        {
            Debug.LogError("ARPlaneManager not found");
        }

        initSavedParameters();
        foreach (var plane in _planeManager.trackables)
        {
            SetPlaneAlpha(plane, 0.0f, 0.0f);
        }
        destroList = new List<ARPlane>();
        _layerground = LayerMask.NameToLayer("SOL");
        _layerwall = LayerMask.NameToLayer("MUR");
        // Subscribing to events -->don't forget to unsubscribe in onDestroy()
        _planeManager.planesChanged += OnPlanesChanged;

    }
    private void initSavedParameters()
    {
        if (PlayerPrefs.HasKey("ArenaSize"))
        {
            _arenaSize = PlayerPrefs.GetFloat("ArenaSize");
            Debug.Log("Found Value for ArenaSize " + _arenaSize);
        }
        if (PlayerPrefs.HasKey("ArenaSpawnRotationX"))
        {
            float x = PlayerPrefs.GetFloat("ArenaSpawnRotationX");
            float y = PlayerPrefs.GetFloat("ArenaSpawnRotationY");
            float z = PlayerPrefs.GetFloat("ArenaSpawnRotationZ");
            float w = PlayerPrefs.GetFloat("ArenaSpawnRotationW");
            _arenaSpawnRotation = new Quaternion(x, y, z, w);
        }
        if (PlayerPrefs.HasKey("ArenaScaleX"))
        {
            float scX = PlayerPrefs.GetFloat("ArenaScaleX");
            float scY = PlayerPrefs.GetFloat("ArenaScaleY");
            float scZ = PlayerPrefs.GetFloat("ArenaScaleZ");
            _arenaScale = new Vector3(scX, scY, scZ);
        }
        if (PlayerPrefs.HasKey("FadeOut"))
        {
            _FadeOut = PlayerPrefs.GetInt("FadeOut");
            Debug.Log("Started with Fade Out mode " + _FadeOut);
        }
    }
    /*
    Make the plane appears for debug 
    */
    public void OnTogglePlanesAction()
    {

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
    
    //change Plan
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
            // Init first color
            Color startColor = lineRenderer.startColor;
            Color endColor = lineRenderer.endColor;

            startColor.a = alphaValueLine;
            endColor.a = alphaValueLine;

            lineRenderer.startColor = startColor;
            lineRenderer.endColor = endColor;
        }
    }

    /*
    Called to set the planes 
    */
    private void OnPlanesChanged(ARPlanesChangedEventArgs arguments)
    {
        if (arguments.added.Count > 0)
        {
            numberOfAddedPlane = 0;
            foreach (var plane in _planeManager.trackables)
            {
                numberOfAddedPlane++;
                //PrintPanelLabel(plane);
                Vector3 spawnPosition;
                //Check if plane is a table --> Spawn an arena on it
                if (plane.classification == UnityEngine.XR.ARSubsystems.PlaneClassification.Table)
                {

                    //Debug.Log("Table Found");
                    if (!_arenaSpawned)
                    {

                        spawnPosition = plane.center;
                        float sizeTable = plane.extents.sqrMagnitude;

                        
                        float sizeX = plane.size.x;
                        float sizeZ = plane.size.y;
                        
                        float sizeTable2;
                        if (sizeX < sizeZ)
                        {
                            sizeTable2 = sizeX;
                        }
                        else
                        {
                            sizeTable2 = sizeZ;
                        }

                        spawnPosition.y +=0.01f;
                        // Setting layer
                        plane.gameObject.layer = LayerMask.NameToLayer("SOL");
                        plane.gameObject.AddComponent<ARAnchor>();
                        
                        //add under plan to limit boid dicgin into the table
                        Vector3 underplane = new Vector3(plane.transform.position.x, plane.transform.position.y-0.00000001f, plane.transform.position.z);
                        GameObject dense=Instantiate(planDense, underplane, plane.transform.rotation);
                        dense.transform.localScale = new Vector3(plane.size.x, dense.transform.localScale.y*0.40f, plane.size.y);
                        underP = dense;
                        // Instanciation scene
                        GameObject scene=Instantiate(toSpawn, spawnPosition, Quaternion.identity);
                        // for arena
                        Quaternion spawnRotation = plane.transform.rotation;
                        _arena = scene;
                        _arena.AddComponent<ARAnchor>();
                        //if (_arena != null){ Debug.Log("Arena Init"); }
                        //else { Debug.Log("_ARena Null"); }


                        //  Arene mode automatique
                        _arenaSize = sizeTable2;
                        _arenaSpawnRotation=spawnRotation;
                        _arenaSpawnPos = spawnPosition;
                        _arenaSpawnRotation = spawnRotation;
                        scene.GetComponent<InitSceneScript>().Init(_arenaSpawnPos, _arenaSize*0.95f,_arenaSpawnRotation,true);

                        //if manuel apply previous scale
                        _arenaSpawned = true;

                    }
                }
                //Check if plane is a ground --> Add a component that destro all other objects
                if ((plane.classification == UnityEngine.XR.ARSubsystems.PlaneClassification.Floor) || (plane.classification == UnityEngine.XR.ARSubsystems.PlaneClassification.Wall)) 
                {
                    /*
                    Mesh planeMesh = new Mesh();
                    ARPlaneMeshGenerators.GenerateMesh(planeMesh, Pose.identity, plane.boundary);
                    MeshCollider planeMeshCollider=plane.gameObject.AddComponent<MeshCollider>();
                    planeMeshCollider.sharedMesh = planeMesh;
                    planeMeshCollider.isTrigger = true;
                    */
                    
                    plane.gameObject.AddComponent<BoxCollider>();
                    
                    //add under plane
                    if (plane.classification == UnityEngine.XR.ARSubsystems.PlaneClassification.Floor)
                    {
                        Vector3 underplane = new Vector3(plane.transform.position.x, plane.transform.position.y-0.5f, plane.transform.position.z);
                        GameObject dense = Instantiate(planDense, underplane, plane.transform.rotation);
                        dense.transform.localScale = new Vector3(plane.size.x, dense.transform.localScale.y * 0.40f, plane.size.y);
                        dense.GetComponent<MeshRenderer>().enabled = false;
                        dense.AddComponent<DestroyGroundScript>();
                        plane.gameObject.layer =_layerground ;
                    }
                    if (plane.classification == UnityEngine.XR.ARSubsystems.PlaneClassification.Wall)
                    {
                        plane.gameObject.layer = _layerwall;
                    }
                    //Add destructor
                    BoxCollider boxCollider = plane.GetComponent<BoxCollider>();
                    boxCollider.size = plane.gameObject.transform.localScale;
                    boxCollider.center = plane.center;
                    boxCollider.isTrigger = true;
                    
                    plane.gameObject.AddComponent<DestroyGroundScript>();
                    destroList.Add(plane);

                    //Debug.Log("Destroyer Added");

                }
            }
            //Debug.Log("Number of Planes " + _planeManager.trackables.count);
            //Debug.Log("Number of planes found " + numberOfAddedPlane);
            propagateFadeOut();
        }
    }

    /*
    Called to rebuild the arena
    */
    private void rebuild()
    {
        numberOfAddedPlane = 0;
        foreach (var plane in _planeManager.trackables)
        {
            numberOfAddedPlane++;
            //PrintPanelLabel(plane);
            Vector3 spawnPosition;
            //Check if plane is a table --> Spawn an arena on it
            if (plane.classification == UnityEngine.XR.ARSubsystems.PlaneClassification.Table)
            {

                //Debug.Log("Table Found");
                if (!_arenaSpawned)
                {
                    spawnPosition = plane.center;
                    float sizeTable = plane.extents.sqrMagnitude;


                    float sizeX = plane.size.x;
                    float sizeZ = plane.size.y;

                    float sizeTable2;
                    if (sizeX < sizeZ)
                    {
                        sizeTable2 = sizeX;
                    }
                    else
                    {
                        sizeTable2 = sizeZ;
                    }

                    spawnPosition.y += 0.01f;
                    // Setting layer
                    plane.gameObject.layer = LayerMask.NameToLayer("SOL");
                    plane.gameObject.AddComponent<ARAnchor>();

                    //add under plane
                    Vector3 underplane = new Vector3(plane.transform.position.x, plane.transform.position.y - 0.25f, plane.transform.position.z);
                    GameObject dense = Instantiate(planDense, underplane, plane.transform.rotation);
                    dense.transform.localScale = new Vector3(plane.size.x, dense.transform.localScale.y * 0.40f, plane.size.y);
                    underP = dense;
                    // instanciate scene
                    GameObject scene = Instantiate(toSpawn, spawnPosition, Quaternion.identity);
                    // for arena
                    Quaternion spawnRotation = plane.transform.rotation;
                    _arena = scene;
                    _arena.AddComponent<ARAnchor>();
                    //if (_arena != null){ Debug.Log("Arena Init"); }
                    //else { Debug.Log("_ARena Null"); }


                    //  arena mode auto
                    _arenaSize = sizeTable2;
                    _arenaSpawnRotation = spawnRotation;
                    _arenaSpawnPos = spawnPosition;
                    _arenaSpawnRotation = spawnRotation;
                    scene.GetComponent<InitSceneScript>().Init(_arenaSpawnPos, _arenaSize * 0.95f, _arenaSpawnRotation,true);
                    _arenaSpawned = true;

                }
            }
            //Check if plane is a ground --> Add a component that destro all other objects
            if ((plane.classification == UnityEngine.XR.ARSubsystems.PlaneClassification.Floor) || (plane.classification == UnityEngine.XR.ARSubsystems.PlaneClassification.Wall))
            {
                /*
                Mesh planeMesh = new Mesh();
                ARPlaneMeshGenerators.GenerateMesh(planeMesh, Pose.identity, plane.boundary);
                MeshCollider planeMeshCollider=plane.gameObject.AddComponent<MeshCollider>();
                planeMeshCollider.sharedMesh = planeMesh;
                planeMeshCollider.isTrigger = true;
                */

                plane.gameObject.AddComponent<BoxCollider>();

                //add under plane
                if (plane.classification == UnityEngine.XR.ARSubsystems.PlaneClassification.Floor)
                {
                    Vector3 underplane = new Vector3(plane.transform.position.x, plane.transform.position.y - 0.5f, plane.transform.position.z);
                    GameObject dense = Instantiate(planDense, underplane, plane.transform.rotation);
                    dense.transform.localScale = new Vector3(plane.size.x, dense.transform.localScale.y * 0.40f, plane.size.y);
                    dense.AddComponent<DestroyGroundScript>();
                    plane.gameObject.layer = _layerground;
                }
                if (plane.classification == UnityEngine.XR.ARSubsystems.PlaneClassification.Wall)
                {
                    plane.gameObject.layer = _layerwall;
                }
                //add destructor
                BoxCollider boxCollider = plane.GetComponent<BoxCollider>();
                boxCollider.size = plane.gameObject.transform.localScale;
                boxCollider.center = plane.center;
                boxCollider.isTrigger = true;

                plane.gameObject.AddComponent<DestroyGroundScript>();

                destroList.Add(plane);

                //Debug.Log("Destroyer Added");

            }
        }
        //Debug.Log("Number of Planes " + _planeManager.trackables.count);
        //Debug.Log("Number of planes found " + numberOfAddedPlane);
        propagateFadeOut();
    }
    public void ArenaChanges(float newSize)
    {
        //Debug.Log("ENTERING ARENA CHANGES");
        Destroy(underP);
        underP = null;
        foreach(var d in destroList)
        {
            Destroy(d.GetComponent<DestroyGroundScript>()) ;
        }
        destroList.Clear();
        destroList = new List<ARPlane>();
        GameObject oldArena = _arena;
        oldArena.GetComponent<InitSceneScript>().Thanos2();
        if (_arena== null)
        {
            Debug.Log("                  IS NULL 1");
        }
        oldArena.GetComponent<InitSceneScript>().CleanArena();
        if (_arena == null)
        {
            Debug.Log("                  IS NULL  after Clean");
        }
        Destroy(oldArena);
        Debug.Log("Old Arena Destroyed");
        rebuild();
        propagateFadeOut();
    }
    public void clearSavedConfig()
    {
        PlayerPrefs.DeleteAll();
    }
    public GameObject getArena()
    {
        return _arena;
    } 
    /*
    Changing modes  
    */
    public void changeFadeOutMod()
    {
        //Debug.Log("ENTERING CHANGE FADE OUT");
        if (_FadeOut == 0)
        {
            Debug.Log("Fade Out Mode : ON");
            _FadeOut = 1;
        }
        else
        {
            Debug.Log("Fade Out Mode : OFF");
            _FadeOut = 0;
        }
        propagateFadeOut();
    }
    public void propagateFadeOut()
    {
        //Debug.Log("Propagating " + _FadeOut);
        foreach (ARPlane d in destroList)
        {
            d.GetComponent<DestroyGroundScript>().setMod(_FadeOut);
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
    /*
    Saving functions 
    */
    private void OnApplicationFocus(bool focus)
    {
        if (!focus) {
            Debug.Log("Focus Lost : saving parameters ... ");
            _arenaScale= _arena.GetComponent<InitSceneScript>().GetParentArena().transform.localScale;
            PlayerPrefs.SetFloat("ArenaSize", _arenaSize);
            PlayerPrefs.SetInt("FadeOut", _FadeOut);
            PlayerPrefs.SetFloat("ArenaSpawnRotationX", _arenaSpawnRotation.x);
            PlayerPrefs.SetFloat("ArenaSpawnRotationY", _arenaSpawnRotation.y);
            PlayerPrefs.SetFloat("ArenaSpawnRotationZ", _arenaSpawnRotation.z);
            PlayerPrefs.SetFloat("ArenaSpawnRotationW", _arenaSpawnRotation.w);
            PlayerPrefs.SetFloat("ArenaScaleX",_arenaScale.x);
            PlayerPrefs.SetFloat("ArenaScaleY", _arenaScale.y);
            PlayerPrefs.SetFloat("ArenaScaleZ", _arenaScale.z);
           
        }
        else
        {
            _arenaSpawned = false;
            ArenaChanges(_arenaSize);
        }
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("On Pause : saving parameters ... ");
            PlayerPrefs.SetFloat("ArenaSize", _arenaSize);
            PlayerPrefs.SetInt("FadeOut", _FadeOut);
            PlayerPrefs.SetFloat("ArenaSpawnRotationX", _arenaSpawnRotation.x);
            PlayerPrefs.SetFloat("ArenaSpawnRotationY", _arenaSpawnRotation.y);
            PlayerPrefs.SetFloat("ArenaSpawnRotationZ", _arenaSpawnRotation.z);
            PlayerPrefs.SetFloat("ArenaSpawnRotationW", _arenaSpawnRotation.w);
            PlayerPrefs.SetFloat("ArenaScaleX", _arenaScale.x);
            PlayerPrefs.SetFloat("ArenaScaleY", _arenaScale.y);
            PlayerPrefs.SetFloat("ArenaScaleZ", _arenaScale.z);


        }
        else
        {
            _arenaSpawned = false;
            ArenaChanges(_arenaSize);
        }
    }
    private void OnApplicationQuit()
    {
        Debug.Log("On Quit : saving parameters ... ");
        PlayerPrefs.SetFloat("ArenaSize", _arenaSize);
        PlayerPrefs.SetInt("FadeOut", _FadeOut);
        PlayerPrefs.SetFloat("ArenaSpawnRotationX", _arenaSpawnRotation.x);
        PlayerPrefs.SetFloat("ArenaSpawnRotationY", _arenaSpawnRotation.y);
        PlayerPrefs.SetFloat("ArenaSpawnRotationZ", _arenaSpawnRotation.z);
        PlayerPrefs.SetFloat("ArenaSpawnRotationW", _arenaSpawnRotation.w);
        PlayerPrefs.SetFloat("ArenaScaleX", _arenaScale.x);
        PlayerPrefs.SetFloat("ArenaScaleY", _arenaScale.y);
        PlayerPrefs.SetFloat("ArenaScaleZ", _arenaScale.z);

    }
    /*
    Unsub the plane changement event
    Called before destruction
    */
    private void OnDestroy()
    {
        //Debug.Log("Calling Destructor for ScenePlaneDetectController");
        _planeManager.planesChanged -= OnPlanesChanged;
    }
}

