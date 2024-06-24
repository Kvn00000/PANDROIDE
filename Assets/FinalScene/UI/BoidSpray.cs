using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class BoidSpray : MonoBehaviour{
    public Button BoidButton;
    
    public GameObject RightController;
    public GameObject LeftController;
    public Material sprayMaterial;
    public GameObject SprayBottle;


    private SpawnBoidScript BoidSpawnerRight;
    private SpawnBoidScript BoidSpawnerLeft;
    private string boidColorHex = "#d55e00";

    public void Awake(){
        BoidSpawnerRight = RightController.GetComponent<SpawnBoidScript>();
        BoidSpawnerLeft = LeftController.GetComponent<SpawnBoidScript>();

    }

    // Update is called once per frame
    void Start(){
        if(BoidButton){
            BoidButton.onClick.AddListener(OnButtonClick);
        }
    }

    public void OnButtonClick(){
        SprayBottle.SetActive(true);
        BoidSpawnerRight.toInstantiate = 1;
        BoidSpawnerLeft.toInstantiate = 1;
        Color color;
        ColorUtility.TryParseHtmlString(boidColorHex, out color);
        sprayMaterial.SetColor("_BaseColor", color);
    }
}
