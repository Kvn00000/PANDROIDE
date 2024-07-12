using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;
using Unity.VisualScripting;
using UnityEngine.PlayerLoop;
using System.Runtime.InteropServices;


public class InitSceneScript : MonoBehaviour
{
    //Prefabs and other link
    public GameObject boid;
    [Header("Link with controllers")]
    [SerializeField]
    private GameObject controllerSpawnerR;
    [SerializeField]
    private GameObject controllerSpawnerL;
    [SerializeField]
    private GameObject MenuR;
    [SerializeField]
    private GameObject MenuL;
    [SerializeField]
    private GameObject parentArenaPrefab; // --> parent  of the arena
    //Nombre de face pour le mur
    public int side;


    [Header("Environnement prefab")]

    public Transform init_transform;
    public GameObject box;
    public GameObject plane;
    public GameObject wall;


    [Header("Arena Size")]

    public float arenaSize = 0;
    public bool damier = false;

    [Header("Boid Parameters")]
    
    public int BoidNumber;
    public float BoidSpeed;
    public float wallRay;
    public float avoidRay;
    public float cohesionRay;
    public float attractionRay;
    public float filter;


    //Taille d'une case
    private float boxsize = 1;
    
    //Tableau de toutes les cases
    private GameObject[,] elements;

    //Walls parameters
    private GameObject parentArena;
    private GameObject walls;
    private GameObject walls2;
    private CircleWallScript component_wall;
    private CircleWallScript component_wall2;
    private GameObject topArenaWall;
    private CircleWallScript component_topWall;
    private float wallThickness= 0.01f;
    private GameObject _plane;
    
    //Materials prefabs
    [SerializeField]
    private Material MatWallInt;
    [SerializeField]
    private Material MatWallMed;
    [SerializeField]
    private Material MatWallExt;

    //
    private float x_ref;
    private float y_ref;
    private float z_ref;


    private List<GameObject> boidList = new List<GameObject>();
    private Vector3 boidSpawnPos;
    
    void Awake()
    {
        Physics.gravity = new Vector3(0, -1F, 0);
    }

    // Start is called before the first frame update
    
    public void Init(Vector3 _spawnPos,float _sizeTable,Quaternion _spawnRotation,bool boidSpawn=false,bool _damier=false)
    {
        boidSpawnPos = _spawnPos;
        //Debug.Log("_SPAWN POS  ="+_spawnPos);
        arenaSize = _sizeTable;
        // Scaling Boids parameters
        BoidSpeed = 100;
        wallRay = arenaSize * 0.09f;
        avoidRay=wallRay;
        cohesionRay=arenaSize*0.5f;
        attractionRay=arenaSize*0.6f;
        filter=3;
        // Init boid spawn parameters
        SpawnBoidScript tomodifR = controllerSpawnerR.GetComponent<SpawnBoidScript>();
        SpawnBoidScript tomodifL = controllerSpawnerL.GetComponent<SpawnBoidScript>();
        initSpawnerParameters(tomodifR);
        initSpawnerParameters(tomodifL);
        CircularMenuManager mr = MenuR.GetComponent<CircularMenuManager>() ;
        CircularMenuManager ml = MenuL.GetComponent<CircularMenuManager>();
        mr.setScene(this);
        ml.setScene(this);

        //
        
        //Debug.Log("ARENA SIZE "+arenaSize);
        //Coords d'une case
        x_ref = -arenaSize * 0.5F;
        y_ref = 0;
        z_ref = -arenaSize * 0.5F;
        //if (damier)
        //{
        //    elements = new GameObject[arenaSize * arenaSize, arenaSize * arenaSize];
        //}
        init_transform.position = _spawnPos;
        //On ajoute le mur
        DrawCircularOrSidedArena();
        //
        if (damier){
            //Add tiles to the planes
            //count the number of tiles
            int cptx = 0;
            int cptz = 0;
            for ( double x  = boxsize*0.5 ; x <= arenaSize ; x = x + boxsize ) {
                for ( double z  = boxsize*0.5 ; z <= arenaSize ; z = z + boxsize ) {
                    Vector3 pos = new Vector3 ( (float)(x_ref+x) , (float)(y_ref-boxsize*0.5F), (float)(z_ref+z) );
                    elements[cptx,cptz] = Instantiate(box, pos, init_transform.rotation);
                    cptz = cptz +1;
                }
                cptx = cptx + 1;
            }
        }else{
            //Add the plane
            _plane = Instantiate(plane, new Vector3(0,-arenaSize*0.5F,0), init_transform.rotation);
            _plane = Instantiate(plane, _spawnPos, init_transform.rotation);
            _plane.GetComponent<Plane>().Init(side, arenaSize * 0.5f);
            _plane.layer = 7;
        }


        // Spawn des boids
        if (boidSpawn)
        {
            spawnBoidsInit();
        }
    }


 /*   
private void Start()
{
   float _sizeTable=2f;
   Vector3 _spawnPos = new Vector3(0, 0, 0);
   arenaSize = _sizeTable;
   // Scaling Boids parameters
   BoidSpeed = 100;
   wallRay = arenaSize * 0.09f;
   avoidRay = wallRay;
   cohesionRay = arenaSize * 0.5f;
   attractionRay = arenaSize * 0.6f;
   filter = 3;

   //SpawnBoidScript tomodif = controllerSpawner.GetComponent<SpawnBoidScript>();
   //tomodif.speed = BoidSpeed;
   //tomodif.wallRay = wallRay;
   //tomodif.avoidRay = avoidRay;
   //tomodif.cohesionRay = cohesionRay;
   //tomodif.attractionRay = attractionRay;
   //tomodif.filter = filter;

   Debug.Log("ARENA SIZE " + arenaSize);
   //Coords d'une case
   x_ref = -arenaSize * 0.5F;
   y_ref = 0;
   z_ref = -arenaSize * 0.5F;
   //if (damier)
   //{
   //    elements = new GameObject[arenaSize * arenaSize, arenaSize * arenaSize];
   //}
   init_transform.position = _spawnPos;
   //On ajoute le mur
   DrawCircularOrSidedArena();

   //
   if (damier)
   {
       //Ajout du damier

       //Compteur de nombre de case
       int cptx = 0;
       int cptz = 0;
       for (double x = boxsize * 0.5; x <= arenaSize; x = x + boxsize)
       {
           for (double z = boxsize * 0.5; z <= arenaSize; z = z + boxsize)
           {
               Vector3 pos = new Vector3((float)(x_ref + x), (float)(y_ref - boxsize * 0.5F), (float)(z_ref + z));
               elements[cptx, cptz] = Instantiate(box, pos, init_transform.rotation);
               cptz = cptz + 1;
           }
           cptx = cptx + 1;
       }

   }
   else
   {
       //Ajout du plane
       _plane = Instantiate(plane, new Vector3(0,-arenaSize*0.5F,0), init_transform.rotation);
       _plane = Instantiate(plane, _spawnPos, init_transform.rotation);
       _plane.GetComponent<Plane>().Init(side,arenaSize*0.5F);
       Debug.Log("THE Plane LAYER IS " + _plane.layer);
       _plane.layer = 7;
   }


   // Spawn des boids

   for (int i = 0; i < BoidNumber; i++)
   {
       //Coordonnées aléatoire
       Vector3 spawnPosition = new Vector3(
           Random.Range(-arenaSize / 4f, arenaSize / 4f),
           0.5F,
           Random.Range(-arenaSize / 4f, arenaSize / 4f)
       );
       //Angle aléatoire
       float randomAngleY = Random.Range(0f, 360f);
       Quaternion spawnRotation = Quaternion.Euler(0f, randomAngleY, 0f);
       Vector3 otherSpawn = new Vector3(_spawnPos.x, _spawnPos.y + 1, _spawnPos.z);
       boidTuning obj = Instantiate(boid, otherSpawn, spawnRotation).GetComponent<boidTuning>();
       obj.Init(BoidSpeed, wallRay, avoidRay, cohesionRay, attractionRay, filter);
       obj.withDEBUG = false;
       boidsList.Add(obj);

   }
}
/**/

    /*
     Draw Arena function
    */
    private void initSpawnerParameters(SpawnBoidScript tomodif)
    {

        tomodif.speed = BoidSpeed;
        tomodif.wallRay = wallRay;
        tomodif.avoidRay = avoidRay;
        tomodif.cohesionRay = cohesionRay;
        tomodif.attractionRay = attractionRay;
        tomodif.filter = filter;
        tomodif.setScene(this);
    }
    private void DrawCircularOrSidedArena()
    {
        //Instanciate parent
        parentArena = Instantiate(parentArenaPrefab, init_transform.position, init_transform.rotation);
        BoxCollider box =parentArena.GetComponent<BoxCollider>();
        parentArena.GetComponent<ResizableWallScript>().SetCenterInit(init_transform.position);
        parentArena.GetComponent<ResizableWallScript>().SetTableRotation(init_transform.rotation);
        //Instanciate inside wall
        walls = Instantiate(wall, init_transform.position, init_transform.rotation);
        component_wall = walls.GetComponent<CircleWallScript>();
        component_wall.inter = true;
        component_wall.DrawWall(side, arenaSize * 0.5f, arenaSize * 0.04f);
        //Instanciate ext wall
        walls2 = Instantiate(wall, init_transform.position, init_transform.rotation);
        walls2.layer = LayerMask.NameToLayer("MUR");
        component_wall2 = walls2.GetComponent<CircleWallScript>();
        component_wall2.DrawWall(side, (arenaSize * 0.5f) + wallThickness, arenaSize * 0.04f);

        box.center = component_wall2.GetComponent<MeshCollider>().bounds.center;
        box.size = component_wall2.GetComponent<MeshCollider>().bounds.size;

        //Instanciate top wall
        topArenaWall = Instantiate(wall, init_transform.position, init_transform.rotation);
        topArenaWall.layer = LayerMask.NameToLayer("MUR");
        component_topWall = topArenaWall.GetComponent<CircleWallScript>();
        List<Vector3> intTopWall = component_wall.getTopPoints();
        List<Vector3> extTopWall = component_wall2.getTopPoints();
        List<Vector3> mergedList = CircleWallScript.mergeTwoVerticesList(intTopWall, extTopWall);
        component_topWall.DrawTop(mergedList);
        component_topWall.setThickness(wallThickness);
        
        GameObject child= box.transform.GetChild(0).gameObject;
        child.transform.position = box.center;
        child.transform.localScale = new Vector3(box.size.x,box.size.y,box.size.z);
        
        // Setting parents
        walls.transform.parent = topArenaWall.transform;
        walls2.transform.parent = topArenaWall.transform;
        topArenaWall.transform.parent = parentArena.transform;
        // Setting Materials
        component_wall.setNewMesh(MatWallInt);
        component_topWall.setNewMesh(MatWallMed);
        component_wall2.setNewMesh(MatWallExt);

    }
    private void DestroySidedArena()
    {
        for (int i =0; i< parentArena.transform.childCount; i++)
        {
            GameObject child = parentArena.transform.GetChild(i).gameObject;
            Destroy(child);
        }
        Destroy(parentArena);
    }
    public GameObject GetParentArena()
    {
        return parentArena;
    }
    public void CleanArena()
    {
        //Arena Destruction
        DestroySidedArena();
    }


    /*
    Functions related to resetting the arena
    */
    public void spawnBoidsInit()
    {
        //Debug.Log("SPAWN POSITION "+boidSpawnPos);
        for (int i = 0; i < BoidNumber; i++)
        {
            //Angle al�atoire
            float randomAngleY = Random.Range(0f, 360f);
            Quaternion spawnRotation = Quaternion.Euler(0f, randomAngleY, 0f);
            Vector3 otherSpawn = new Vector3(boidSpawnPos.x, boidSpawnPos.y + 1, boidSpawnPos.z);
            GameObject obj = Instantiate(boid, otherSpawn, spawnRotation);
            boidTuning tmp = obj.GetComponent<boidTuning>();
            tmp.Init(BoidSpeed, wallRay, avoidRay, cohesionRay, attractionRay, filter);
            tmp.withDEBUG = false;
            boidList.Add(obj);
        }
    }

    public void Thanos()
    {
        // Destroy all the boids and make them respawn
        for (int i = boidList.Count - 1; i >= 0; i--)
        {
            GameObject boid = boidList[i];
            boidList.RemoveAt(i);
            Destroy(boid);
        }

        spawnBoidsInit();
    }
    public void Thanos2()
    {
        // Only destroy all the boids
        for (int i = boidList.Count - 1; i >= 0; i--)
        {
            GameObject boid = boidList[i];
            boidList.RemoveAt(i);
            Destroy(boid);
        }
    }

    public void CleanUpDestroyedObjects()
    {
        // clean the list of all the null elements
        boidList.RemoveAll(item => item == null);
    }
    /*
    Functions related the manipulation of the boid list
    */
    public void addBoidList(GameObject boid){
        boidList.Add(boid);
    }

    public int getBoidListCount(){
        return boidList.Count;
    }
    public List<GameObject> getBoidList()
    {
        return boidList;
    }
}
