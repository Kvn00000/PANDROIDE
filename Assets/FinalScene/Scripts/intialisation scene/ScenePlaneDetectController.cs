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
    private InputActionReference _fadeOutModeChange;

    [SerializeField]
    private GameObject toSpawn;
    [SerializeField]
    private GameObject planDense;
    private ARPlaneManager _planeManager;
    private bool isOn = false;
    private int numberOfAddedPlane = 0;
    public bool damier = false;
    // Start is called before the first frame update
    private ARPlane _planeArena;
    private GameObject _arena;
    private float _planeSize;
    private float _arenaSize;
    private bool _arenaSpawned=false;
    private Vector3 _arenaSpawnPos;
    private Quaternion _arenaSpawnRotation;
    private Vector3 _arenaScale;
    // Mode : 0 --> Automatique ; 1 --> Manuel 
    private int _mode = 0;
    private int _FadeOut=0;
    List<ARPlane> destroList;
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
            if (_mode == 0)
            {
                Debug.Log("Launch in auto mode");
            }
            else
            {
                Debug.Log("Launch in manual mode");
            }
        }
        if (PlayerPrefs.HasKey("ArenaSize"))
        {
            _arenaSize = PlayerPrefs.GetFloat("ArenaSize");
            Debug.Log("Found Value for ArenaSize " + _arenaSize);
        }
        if (PlayerPrefs.HasKey("ArenaSpawnRotationX"))
        {
            float x=PlayerPrefs.GetFloat("ArenaSpawnRotationX");
            float y= PlayerPrefs.GetFloat("ArenaSpawnRotationY");
            float z=PlayerPrefs.GetFloat("ArenaSpawnRotationZ");
            float w=PlayerPrefs.GetFloat("ArenaSpawnRotationW");
            _arenaSpawnRotation = new Quaternion(x, y, z, w);
        }
        if (PlayerPrefs.HasKey("ArenaScaleX"))
        {
            float scX= PlayerPrefs.GetFloat("ArenaScaleX");
            float scY= PlayerPrefs.GetFloat("ArenaScaleY");
            float scZ= PlayerPrefs.GetFloat("ArenaScaleZ");
            _arenaScale = new Vector3(scX, scY,scZ);
        }
        if (PlayerPrefs.HasKey("FadeOut"))
        {
            _FadeOut = PlayerPrefs.GetInt("FadeOut");
        }
        foreach (var plane in _planeManager.trackables)
        {
            SetPlaneAlpha(plane, 0.0f, 0.0f);
        }
        destroList = new List<ARPlane>();
        
        // On s'abonne aux evenements --> Ne pas oublier de se desabonner dans onDestroy()
        togglePlanesDetectedAction.action.performed += OnTogglePlanesAction;
        _planeManager.planesChanged += OnPlanesChanged;
        _fadeOutModeChange.action.performed += changeFadeOutMod;


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
                        // Recup infos plan
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
                        
                        //Ajout sous plan
                        Vector3 underplane = new Vector3(plane.transform.position.x, plane.transform.position.y-0.25f, plane.transform.position.z);
                        GameObject dense=Instantiate(planDense, underplane, plane.transform.rotation);
                        dense.transform.localScale = new Vector3(plane.size.x, dense.transform.localScale.y*0.40f, plane.size.y);
                        // Instanciation scene
                        GameObject scene=Instantiate(toSpawn, spawnPosition, Quaternion.identity);
                        // Récup infos pour l'arene
                        Quaternion spawnRotation = plane.transform.rotation;
                        _arena = scene;
                        _arena.AddComponent<ARAnchor>();
                        //if (_arena != null){ Debug.Log("Arena Init"); }
                        //else { Debug.Log("_ARena Null"); }


                        //  Arene mode automatique
                        if ((_mode == 0) || (float.IsNaN(_arenaSize)))
                        {
                            _arenaSize = sizeTable2;
                            _arenaSpawnRotation=spawnRotation;
                        }

                        _planeSize = sizeTable2;
                        _arenaSpawnPos = spawnPosition;
                        _arenaSpawnRotation = spawnRotation;
                        scene.GetComponent<InitSceneScript>().Init(_arenaSpawnPos, _arenaSize*0.95f,_arenaSpawnRotation,true,damier);

                        // Si manuel appliquer la valeur d'echelle precedemment enregistre
                        if (_mode == 1)
                        {
                            scene.GetComponent<InitSceneScript>().GetParentArena().transform.localScale = _arenaScale;
                        }
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
                    
                    //Ajout sous plan
                    if (plane.classification == UnityEngine.XR.ARSubsystems.PlaneClassification.Floor)
                    {
                        Vector3 underplane = new Vector3(plane.transform.position.x, plane.transform.position.y-0.5f, plane.transform.position.z);
                        GameObject dense = Instantiate(planDense, underplane, plane.transform.rotation);
                        dense.transform.localScale = new Vector3(plane.size.x, dense.transform.localScale.y * 0.40f, plane.size.y);
                        dense.GetComponent<MeshRenderer>().enabled = false;
                    }
                    //Ajout Destructeur
                    BoxCollider boxCollider = plane.GetComponent<BoxCollider>();
                    boxCollider.size = plane.gameObject.transform.localScale;
                    boxCollider.center = plane.center;
                    boxCollider.isTrigger = true;
                    
                    plane.gameObject.AddComponent<DestroyGroundScript>();
                    destroList.Add(plane);

                    Debug.Log("Destroyer Added");

                }
            }
            //Debug.Log("Number of Planes " + _planeManager.trackables.count);
            //Debug.Log("Number of planes found " + numberOfAddedPlane);
            propagateFadeOut();
        }
    }

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
                    // Recup infos plan
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

                    //Ajout sous plan
                    Vector3 underplane = new Vector3(plane.transform.position.x, plane.transform.position.y - 0.25f, plane.transform.position.z);
                    GameObject dense = Instantiate(planDense, underplane, plane.transform.rotation);
                    dense.transform.localScale = new Vector3(plane.size.x, dense.transform.localScale.y * 0.40f, plane.size.y);
                    // Instanciation scene
                    GameObject scene = Instantiate(toSpawn, spawnPosition, Quaternion.identity);
                    // Récup infos pour l'arene
                    Quaternion spawnRotation = plane.transform.rotation;
                    _arena = scene;
                    _arena.AddComponent<ARAnchor>();
                    //if (_arena != null){ Debug.Log("Arena Init"); }
                    //else { Debug.Log("_ARena Null"); }


                    //  Arene mode automatique
                    if ((_mode == 0) || (float.IsNaN(_arenaSize)))
                    {
                        _arenaSize = sizeTable2;
                        _arenaSpawnRotation = spawnRotation;
                    }

                    _planeSize = sizeTable2;
                    _arenaSpawnPos = spawnPosition;
                    _arenaSpawnRotation = spawnRotation;
                    scene.GetComponent<InitSceneScript>().Init(_arenaSpawnPos, _arenaSize * 0.95f, _arenaSpawnRotation);

                    // Si manuel appliquer la valeur d'echelle precedemment enregistre
                    if (_mode == 1)
                    {
                        scene.GetComponent<InitSceneScript>().GetParentArena().transform.localScale = _arenaScale;
                    }
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

                //Ajout sous plan
                if (plane.classification == UnityEngine.XR.ARSubsystems.PlaneClassification.Floor)
                {
                    Vector3 underplane = new Vector3(plane.transform.position.x, plane.transform.position.y - 0.5f, plane.transform.position.z);
                    GameObject dense = Instantiate(planDense, underplane, plane.transform.rotation);
                    dense.transform.localScale = new Vector3(plane.size.x, dense.transform.localScale.y * 0.40f, plane.size.y);
                }
                //Ajout Destructeur
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
        //Debug.Log("Calling Destructor for ScenePlaneDetectController");
        togglePlanesDetectedAction.action.performed -= OnTogglePlanesAction;
        _planeManager.planesChanged -= OnPlanesChanged;
        _fadeOutModeChange.action.performed -= changeFadeOutMod;
    }
    // Update is called once per frame

    public void ArenaChanges(float newSize)
    {
        //Debug.Log("ENTERING ARENA CHANGES");

        destroList.Clear();
        destroList = new List<ARPlane>();
        GameObject oldArena = _arena;
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
        /*GameObject newArena = Instantiate(toSpawn, _planeArena.center, Quaternion.identity);
        newArena.GetComponent<InitSceneScript>().Init(_arenaSpawnPos, newSize, _arenaSpawnRotation, damier);
        _arena = newArena;*/
    }
    public void clearSavedConfig()
    {
        PlayerPrefs.DeleteAll();
    }
    private void OnApplicationFocus(bool focus)
    {
        if (!focus) {
            Debug.Log("Focus Lost : saving parameters ... ");
            _arenaScale= _arena.GetComponent<InitSceneScript>().GetParentArena().transform.localScale;
            PlayerPrefs.SetFloat("ArenaSize", _arenaSize);
            PlayerPrefs.SetInt("ModeArene", _mode);
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
            PlayerPrefs.SetInt("ModeArene", _mode);
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
            //_arenaSpawned = false;
            //ArenaChanges(_arenaSize);
        }
    }
    private void OnApplicationQuit()
    {
        Debug.Log("On Quit : saving parameters ... ");
        PlayerPrefs.SetFloat("ArenaSize", _arenaSize);
        PlayerPrefs.SetInt("ModeArene", _mode);
        PlayerPrefs.SetInt("FadeOut", _FadeOut);
        PlayerPrefs.SetFloat("ArenaSpawnRotationX", _arenaSpawnRotation.x);
        PlayerPrefs.SetFloat("ArenaSpawnRotationY", _arenaSpawnRotation.y);
        PlayerPrefs.SetFloat("ArenaSpawnRotationZ", _arenaSpawnRotation.z);
        PlayerPrefs.SetFloat("ArenaSpawnRotationW", _arenaSpawnRotation.w);
        PlayerPrefs.SetFloat("ArenaScaleX", _arenaScale.x);
        PlayerPrefs.SetFloat("ArenaScaleY", _arenaScale.y);
        PlayerPrefs.SetFloat("ArenaScaleZ", _arenaScale.z);

    }
    private void changeFadeOutMod(InputAction.CallbackContext obj)
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

    private void propagateFadeOut()
    {
        foreach (ARPlane d in destroList)
        {
            d.GetComponent<DestroyGroundScript>().setMod(_FadeOut);
        }
    }

    /**/
    public void ChangeMod()
    {
        if (_mode == 0)
        {
            Debug.Log("Switching from automatic to manual");
            _mode = 1;
            _arenaSpawned = false;
            rebuild();
        }
        else
        {
            Debug.Log("Switching from manual to automatic");
            _mode = 0;
            _arenaSpawned = false;
            rebuild();
        }
    }
}

