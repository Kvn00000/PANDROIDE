using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnOfDebug : MonoBehaviour
{
    [SerializeField]
    private InputActionReference toggleDebugAction;
    private bool isOn = true;
    // Start is called before the first frame update
    void Start()
    {
        toggleDebugAction.action.performed += OntoggleDebugAction;
    }

    private void OntoggleDebugAction(InputAction.CallbackContext obj)
    {
        isOn = !isOn;
        this.gameObject.SetActive(isOn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
