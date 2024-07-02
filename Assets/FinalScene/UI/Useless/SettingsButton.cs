using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingsButtton : MonoBehaviour{
    public Button _button;

    public GameObject SettingsMenu;
    public GameObject MainMenu;
    void Start(){
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        if(_button){
            _button.onClick.AddListener(OnButtonClick);
        }
    }


    public void OnButtonClick(){
        
        SettingsMenu.SetActive(true);
        MainMenu.SetActive(false);
        
    }
}
