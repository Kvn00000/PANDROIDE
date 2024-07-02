using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;  

public class Selector : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject RightController;
    public GameObject BoidPrefab;
    public GameObject BoidMenu;

    public InputActionProperty inputActionProperty;  


    private SpawnBoidScript BoidSpawner;

    public void Awake(){
        BoidSpawner = RightController.GetComponent<SpawnBoidScript>();

    }

    public void SetSpawnerFromIndex(int index){
        // if (index == 0){
        //     BoidSpawner.enabled = false;
        // }else if(index == 1 ){
            
        //     BoidSpawner.enabled = true;
            
        //     // BoidSpawner.inputAction = inputActionProperty;
        //     // BoidSpawner.BoidPrefab = BoidPrefab;
        // }

        if (index == 0){
            BoidSpawner.toInstantiate = 0;
            BoidMenu.SetActive(false);
        }else if(index == 1 ){
            BoidSpawner.toInstantiate = 1;
            BoidMenu.SetActive(true);
        }else if(index == 2 ){
            BoidSpawner.toInstantiate = 2;
        }

    }

}
