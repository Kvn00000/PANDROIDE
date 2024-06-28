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

    public Vector3 positionOffsetLeft ;
    public Vector3 rotationOffsetLeft ;

    public Vector3 positionOffsetRight ;
    public Vector3 rotationOffsetRight ;

    [Header("Spray and Positions")]
    public GameObject bottleSpray;

    public Vector3 SprayPositionOffsetLeft = new Vector3(0f, 0.5f, 0.05f);
    public Vector3 SprayrotationOffsetLeft = new Vector3(0, 0, 0);
    public Vector3 SprayPositionOffsetRight = new Vector3(0f, -0.05f, -0.05f);
    public Vector3 SprayrotationOffsetRight = new Vector3(0, 0, 0);

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

            //Quand le menu est sur la gauche le spray doit etre sur la droite
            bottleSpray.transform.position = RightController.transform.position + RightController.transform.TransformVector(SprayPositionOffsetRight);
            bottleSpray.transform.rotation = RightController.transform.rotation * Quaternion.Euler(SprayrotationOffsetRight);
            leftSpawner.enabled = false;
            RightSpawner.enabled = true;

        }else{
            menuLeft.transform.position = RightController.transform.position + RightController.transform.TransformVector(positionOffsetRight);
            menuLeft.transform.rotation =  RightController.transform.rotation * Quaternion.Euler(rotationOffsetRight);

            //Menu a droite donc spray a gauche
            bottleSpray.transform.position = LeftController.transform.position + LeftController.transform.TransformVector(SprayrotationOffsetLeft);
            bottleSpray.transform.rotation = LeftController.transform.rotation * Quaternion.Euler(SprayrotationOffsetLeft);
            leftSpawner.enabled = true;
            RightSpawner.enabled = false;
            
        }
    }
}
