using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class InitialSpawnScript : MonoBehaviour
{
    private static int _dx = 10;
    private static int _dz = 10;

    private double boxsize = 0.25;

    private GameObject[,] elements = new GameObject[_dx*_dx*16,_dz*_dz*16];

    public Transform init_transform;

    public GameObject box;
    public ProBuilderMesh wall;
    //public Vector3 wallSize = new Vector3(_dx*2f, 0.5f, 0.5f);
    private int _iteration;
    private bool _toroidal;
    // Start is called before the first frame update
    void Start()
    {
        _iteration = 0;

        _toroidal = true;

        float x_ref = -_dx/2;
        float y_ref = 0;
        float z_ref = -_dz/2;
        int cptx = 0;
        int cptz = 0;
        float id = 0;
        //wall.transform.localScale = wallSize;
        //Instantiate(wall, wallSize, init_transform.rotation);
        for ( double x  = 0 ; x != _dx ; x = x + boxsize ) {
            for ( double z  = 0 ; z != _dz ; z = z + boxsize ) {
                if(x == _dx-boxsize || z == _dz-boxsize || x == 0 || z == 0){
                    Vector3 pos = new Vector3 ( (float)(x_ref+x) , (float)(y_ref+0.5) , (float)(z_ref+z) );
                    elements[cptx,cptz] = Instantiate(box, pos, init_transform.rotation);
                }else{
                    Vector3 pos = new Vector3 ( (float)(x_ref+x) , (float)y_ref , (float)(z_ref+z) );
                    elements[cptx,cptz] = Instantiate(box, pos, init_transform.rotation);
                }
                
                cptz = cptz +1;
                //OctoCube el = ((GameObject)elements[x,z]).GetComponent<OctoCube>();
                //el.setId(id);
                //el.setCoordinates(x,z);
                //id = id + 1;
            }
            cptx = cptx + 1;
        }      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
