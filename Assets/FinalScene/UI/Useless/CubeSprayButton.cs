using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CubeSprayButton : MonoBehaviour{
    public Button CubeButton;
    
    public GameObject RightController;
    public GameObject LeftController;
    public Material sprayMaterial;
    public GameObject SprayBottle;


    private SpawnBoidScript CubeSpawnerRight;
    private SpawnBoidScript CubeSpawnerLeft;
    private string cubeColorHex = "#029e73";

    public void Awake(){
        CubeSpawnerRight = RightController.GetComponent<SpawnBoidScript>();
        CubeSpawnerLeft = LeftController.GetComponent<SpawnBoidScript>();
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;


    }

    // Update is called once per frame
    void Start(){
        if(CubeButton){
            CubeButton.onClick.AddListener(OnButtonClick);
        }
    }

    public void OnButtonClick(){
        SprayBottle.SetActive(true);
        CubeSpawnerRight.toInstantiate = 2;
        CubeSpawnerLeft.toInstantiate = 2;
        Color color;
        ColorUtility.TryParseHtmlString(cubeColorHex, out color);
        sprayMaterial.SetColor("_BaseColor", color);
    }
}
