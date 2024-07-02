using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BackSettingsButton : MonoBehaviour{
    public Button _button;

    public GameObject MainMenu;
    public GameObject SettingsMenu;
    void Start(){
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        if(_button){
            _button.onClick.AddListener(OnButtonClick);
        }
    }


    public void OnButtonClick(){
        
        MainMenu.SetActive(true);
        SettingsMenu.SetActive(false);
        
    }
}
