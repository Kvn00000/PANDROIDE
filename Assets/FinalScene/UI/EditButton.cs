using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditButton : MonoBehaviour
{
    public Button editButton;
    public GameObject RightController;
    public GameObject LeftController;
    public Material sprayMaterial;
    public GameObject SprayBottle;

    private SpawnBoidScript BoidSpawnerRight;
    private SpawnBoidScript BoidSpawnerLeft;
    private string boidColorHex = "#FFFFFF";

    public void Awake(){
        BoidSpawnerRight = RightController.GetComponent<SpawnBoidScript>();
        BoidSpawnerLeft = LeftController.GetComponent<SpawnBoidScript>();

    }


    // Start is called before the first frame update
    void Start(){
        if(editButton){
            editButton.onClick.AddListener(OnButtonClick);
        }
    }

    // Update is called once per frame
    public void OnButtonClick(){
        SprayBottle.SetActive(false);
        BoidSpawnerRight.toInstantiate = 0;
        BoidSpawnerLeft.toInstantiate = 0;
        Color color;
        ColorUtility.TryParseHtmlString(boidColorHex, out color);
        sprayMaterial.SetColor("_BaseColor", color);
    }
}
