using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LeftRightButton : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject watchManager;
    public Button _button;
    public GameObject menuLeft;
    public GameObject menuRight;
    public GameObject left;
    public GameObject right;

    private WatchManager watchManagerScript;
    // Update is called once per frame

    public void Awake(){
        watchManagerScript = watchManager.GetComponent<WatchManager>();

    }
    void Update()
    {
        if(_button){
            _button.onClick.AddListener(OnButtonClick);
        }
    }

    public void OnButtonClick(){
        left.SetActive(!left.activeSelf);
        right.SetActive(!right.activeSelf);
        // menuLeft.SetActive(!menuLeft.activeSelf);
        // if(left.activeSelf == true){
        //     watchManagerScript.positionOffsetLeft = new Vector3(0.52f, -0.1f, -0.24f);
        //     watchManagerScript.rotationOffsetLeft = new Vector3(90, 0, 0);
        // }else{
        //     watchManagerScript.positionOffsetLeft = new Vector3(-0.52f, -0.1f, 0.06f);
        //     watchManagerScript.rotationOffsetLeft = new Vector3(90, 180, 0);
        // }
        // menuRight.SetActive(!menuRight.activeSelf);

    }
}
