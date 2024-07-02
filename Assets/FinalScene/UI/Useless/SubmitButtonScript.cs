using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubmitButtonScript : MonoBehaviour
{

    public Button _button;
    public TMP_InputField SpeedInput;
    public TMP_InputField WallRayInput;
    public TMP_InputField AvoidRayInput;
    public TMP_InputField CohesionRayInput;
    public TMP_InputField AttractionRayInput;
    public TMP_InputField FilterInput;

    public GameObject RightController;


    public void Start(){
        if(_button){
            _button.onClick.AddListener(OnButtonClick);
        }
    }
    public void OnButtonClick(){
        //Debug.Log("j'ai changé les params normalement");
        float SpeedText;
        float WallRayText;
        float AvoidRayText; 
        float CohesionRayText; 
        float AttractionRayText; 
        float FilterText; 

        float.TryParse(SpeedInput.text,out SpeedText);
        float.TryParse(WallRayInput.text,out WallRayText);
        float.TryParse(AvoidRayInput.text,out AvoidRayText);
        float.TryParse(CohesionRayInput.text,out CohesionRayText);
        float.TryParse(AttractionRayInput.text,out AttractionRayText);
        float.TryParse(FilterInput.text,out FilterText);


        SpawnBoidScript SpawnBoidComponent = RightController.GetComponent<SpawnBoidScript>();

        SpawnBoidComponent.speed = SpeedText;
        SpawnBoidComponent.wallRay = WallRayText;
        SpawnBoidComponent.avoidRay = AvoidRayText;
        SpawnBoidComponent.cohesionRay = CohesionRayText;
        SpawnBoidComponent.attractionRay = AttractionRayText;
        SpawnBoidComponent.filter = FilterText;
    }




}
