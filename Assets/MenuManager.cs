using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public Transform head;
    public float distanceSpawn = 2.2f;
    public GameObject menu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        menu.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized*distanceSpawn;
        menu.transform.LookAt(new Vector3(head.position.x,menu.transform.position.y,head.position.z)) ;
        menu.transform.forward *= -1;
    }
}
