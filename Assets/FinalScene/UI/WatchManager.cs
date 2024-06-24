using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;


public class WatchManager : MonoBehaviour
{   
    [Header("Menu and Positions")] 
    public GameObject menuLeft;
    public GameObject left;

    public Vector3 positionOffsetLeft = new Vector3(0.52f, -0.1f, -0.24f);
    public Vector3 rotationOffsetLeft = new Vector3(90, 0, 0);

    public Vector3 positionOffsetRight = new Vector3(-0.52f, -0.1f, 0.06f);
    public Vector3 rotationOffsetRight = new Vector3(90, 180, 0);

    [Header("Spray and Positions")]
    public GameObject bottleSpray;

    public Vector3 SprayPositionOffset = new Vector3(-0.52f, -0.1f, 0.06f);
    public Vector3 SprayrotationOffset = new Vector3(90, 180, 0);

    [Header("Controllers")] 

    public GameObject LeftController;
    private SpawnBoidScript leftSpawner;
    public GameObject RightController;
    private SpawnBoidScript RightSpawner;
    


    void Start(){
        RightSpawner = RightController.GetComponent<SpawnBoidScript>();
        leftSpawner = LeftController.GetComponent<SpawnBoidScript>();
    }

    void Update(){

        if(left.activeSelf == true){
            menuLeft.transform.position = LeftController.transform.position + LeftController.transform.TransformVector(positionOffsetLeft);
            menuLeft.transform.rotation =  LeftController.transform.rotation * Quaternion.Euler(rotationOffsetLeft);
            bottleSpray.transform.position = RightController.transform.position;
            bottleSpray.transform.rotation = RightController.transform.rotation;
            leftSpawner.enabled = false;
            RightSpawner.enabled = true;

        }else{
            menuLeft.transform.position = RightController.transform.position + RightController.transform.TransformVector(positionOffsetRight);
            menuLeft.transform.rotation =  RightController.transform.rotation * Quaternion.Euler(rotationOffsetRight);
            bottleSpray.transform.position = LeftController.transform.position;
            bottleSpray.transform.rotation = LeftController.transform.rotation;
            leftSpawner.enabled = true;
            RightSpawner.enabled = false;
            
        }
    }
}
