using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{

    public Transform head;
    public float distanceSpawn = 2.2f;

    public GameObject keyboard;
    public GameObject menu;
    public GameObject BoidMenu;

    public InputActionProperty showButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(showButton.action.WasPressedThisFrame()){
            menu.SetActive(!menu.activeSelf);
            BoidMenu.SetActive(false);
        }
        menu.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized*distanceSpawn;
        menu.transform.LookAt(new Vector3(head.position.x,menu.transform.position.y,head.position.z)) ;
        menu.transform.forward *= -1;

        
        BoidMenu.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized*distanceSpawn;   
        BoidMenu.transform.LookAt(new Vector3(head.position.x,BoidMenu.transform.position.y,head.position.z)) ;
        BoidMenu.transform.forward *= -1;
        

        // keyboard.transform.position = head.position + new Vector3(head.forward.x, -0.5f, head.forward.z).normalized*distanceSpawn;   
        // keyboard.transform.LookAt(new Vector3(head.position.x,keyboard.transform.position.y,head.position.z)) ;
        // keyboard.transform.forward *= -1;
    }

    
}
