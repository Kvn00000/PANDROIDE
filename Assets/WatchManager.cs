using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;


public class WatchManager : MonoBehaviour
{   
    [Header("Menu and Positions")] 
    public GameObject menuRight;
    public GameObject menuLeft;
    public Vector3 positionOffsetLeft = new Vector3(0.52f, -0.1f, -0.24f); // Ajustez selon votre contrôleur
    public Vector3 rotationOffsetLeft = new Vector3(90, 0, 90);

    [Header("Controllers")] 

    public GameObject LeftController;
    public GameObject RightController;
    
    [Header("Buttons")]

    public Button boidButton;

    public Button cubeButton;

    public Button editButton;


    

    void Update()
    {

        menuLeft.transform.position = LeftController.transform.position + LeftController.transform.TransformVector(positionOffsetLeft);
        menuLeft.transform.rotation =  LeftController.transform.rotation * Quaternion.Euler(rotationOffsetLeft);
    
    }
}
