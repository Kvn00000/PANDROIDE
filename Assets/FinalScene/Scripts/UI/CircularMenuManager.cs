using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularMenuManager : MonoBehaviour{
    //Spawner Left and Right
    private SpawnBoidScript Spawner;


    //ObjectColor
    private string boidColorHex = "#d55e00";
    private string cubeColorHex = "#029e73";
    private string NothingColorHex = "#FFFFFF";


    private InitSceneScript initScript;

    //RightPoke on the left hand and LeftPoke on the Right hand

    public GameObject pokeInteractor;

    //For debug Wall
    public GameObject XROrigin;
    private ScenePlaneDetectController scenePlane;



    public GameObject DebugPanel;

    [Header("Spray")]
    public Material sprayMaterial;
    public GameObject SprayBottle;


    [Header("Controller")] // Left Menu has Right controller -- Right Menu has Left controller 
    public GameObject Controller;


    [Header("Left and Right Menus")]
    public GameObject MenuLeft;
    public GameObject MenuRight;


    [Header("Menus Page")]
    public GameObject MainMenu;
    public GameObject SettingsMenu;
    public GameObject MoreSettingsMenu;

    

    
    [Header("Main Menu Button")]
    public Button SettingsButton;
    public Button BoidButton;
    public Button CubeButton;
    public Button NothingButton;


    [Header("Settings Button")]
    public Button MoreButton;
    public Button BackSettingsButton;
    public Button ResetButton;
    public Button PersistanceButton;

    [Header("More Settings Button")]
    public Button LeftRightButton;
    public Button DebugWallButton;
    public Button DebugPanelButton;
    public Button BackMoreSettingsButton;


    void Awake(){
        scenePlane = XROrigin.GetComponent<ScenePlaneDetectController>();
        Spawner = Controller.GetComponent<SpawnBoidScript>();        
    }

    void OnEnable(){
        //To add listener only when the button is activated
        if(MoreSettingsMenu.activeSelf == true){
            addMoreSettingsListener();
        }else{
            addMainPageListener();
        }
    }
    void OnDisable(){
        removeMoreSettingsListener();
    }
    void Start(){
        //Delete all the invisible part of the buttons and add listener only for the MainMenu
        InitMainMenu();
        InitSettingsMenu();
        InitMoreSettingsMenu();
    }




    private void addMainPageListener(){
        if(SettingsButton){
            SettingsButton.onClick.AddListener(OnSettingsButtonClick);
        }
        if(BoidButton){
            BoidButton.onClick.AddListener(OnBoidButtonClick);
        }
        if(CubeButton){
            CubeButton.onClick.AddListener(OnCubeButtonClick);
        }
        if(NothingButton){
            NothingButton.onClick.AddListener(OnNothingButtonClick);
        }
    }

    private void removeMainPageListener(){
        if(SettingsButton){
            SettingsButton.onClick.RemoveListener(OnSettingsButtonClick);
        }
        if(BoidButton){
            BoidButton.onClick.RemoveListener(OnBoidButtonClick);
        }
        if(CubeButton){
            CubeButton.onClick.RemoveListener(OnCubeButtonClick);
        }
        if(NothingButton){
            NothingButton.onClick.RemoveListener(OnNothingButtonClick);
        }
    }

    private void addSettingsListener(){
        if(MoreButton){
            MoreButton.onClick.AddListener(OnMoreButtonClick);
        }
        if(BackSettingsButton){
            BackSettingsButton.onClick.AddListener(OnBackSettingsButtonClick);
        }
        if(ResetButton){
            ResetButton.onClick.AddListener(OnResetButtonClick);
        }
        if(PersistanceButton){
            PersistanceButton.onClick.AddListener(OnPersistanceButtonClick);
        }
    }

    private void removeSettingsListener(){
        if(MoreButton){
            MoreButton.onClick.RemoveListener(OnMoreButtonClick);
        }
        if(BackSettingsButton){
            BackSettingsButton.onClick.RemoveListener(OnBackSettingsButtonClick);
        }
        if(ResetButton){
            ResetButton.onClick.RemoveListener(OnResetButtonClick);
        }
        if(PersistanceButton){
            PersistanceButton.onClick.RemoveListener(OnPersistanceButtonClick);
        }
    }

    private void addMoreSettingsListener(){
        if(LeftRightButton){
            LeftRightButton.onClick.AddListener(OnLeftRightButtonClick);
        }
        if(DebugWallButton){
            DebugWallButton.onClick.AddListener(OnDebugWallButtonClick);
        }
        if(DebugPanelButton){
            DebugPanelButton.onClick.AddListener(OnDebugPanelButtonClick);
        }
        if(BackMoreSettingsButton){
            BackMoreSettingsButton.onClick.AddListener(OnBackMoreSettingsButtonClick);
        }
    }

    private void removeMoreSettingsListener(){
        if(LeftRightButton){
            LeftRightButton.onClick.RemoveListener(OnLeftRightButtonClick);
        }
        if(DebugWallButton){
            DebugWallButton.onClick.RemoveListener(OnDebugWallButtonClick);
        }
        if(DebugPanelButton){
            DebugPanelButton.onClick.RemoveListener(OnDebugPanelButtonClick);
        }
        if(BackMoreSettingsButton){
            BackMoreSettingsButton.onClick.RemoveListener(OnBackMoreSettingsButtonClick);
        }
    }


    private void InitMainMenu(){
        SettingsButton.transform.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        BoidButton.transform.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        CubeButton.transform.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        NothingButton.transform.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;

    }
    private void InitSettingsMenu(){
        BackSettingsButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        ResetButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        PersistanceButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        MoreButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

    private void InitMoreSettingsMenu(){
        LeftRightButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        DebugWallButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        BackMoreSettingsButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        DebugPanelButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;

    }



    //Main Menu Event -------------------------------------
    private void OnSettingsButtonClick(){

        //Switch to the Settings Menu
        SettingsMenu.SetActive(true);

        if(MainMenu.activeSelf == true){
            MainMenu.SetActive(false);
            addSettingsListener();
            removeMainPageListener();
        }


        if(SprayBottle.activeSelf == true){
            SprayBottle.SetActive(false);
        }

        if(pokeInteractor.activeSelf == false){
            pokeInteractor.SetActive(true);
        }

        //No spawn mode
        Spawner.toInstantiate = 0;
        Color color;
        ColorUtility.TryParseHtmlString(NothingColorHex, out color);
        sprayMaterial.SetColor("_BaseColor", color);

    }

    private void OnBoidButtonClick(){
        
        if(SprayBottle.activeSelf == false){
            SprayBottle.SetActive(true);
        }

        //spawn boid + change color
        Spawner.toInstantiate = 1;
        Color color;
        ColorUtility.TryParseHtmlString(boidColorHex, out color);
        sprayMaterial.SetColor("_BaseColor", color);
    }

    private void OnCubeButtonClick(){
        if(SprayBottle.activeSelf == false){
            SprayBottle.SetActive(true);
        }

        //Spawn cube + change color
        Spawner.toInstantiate = 2;
        Color color;
        ColorUtility.TryParseHtmlString(cubeColorHex, out color);
        sprayMaterial.SetColor("_BaseColor", color);
    }

    private void OnNothingButtonClick(){
        if(SprayBottle.activeSelf == true){
            SprayBottle.SetActive(false);
        }

        Spawner.toInstantiate = 0;
        Color color;
        ColorUtility.TryParseHtmlString(NothingColorHex, out color);
        sprayMaterial.SetColor("_BaseColor", color);
    }




    //Settings Menu Event -------------------------------------
    private void OnMoreButtonClick(){
        MoreSettingsMenu.SetActive(true);
        SettingsMenu.SetActive(false);

        addMoreSettingsListener();
        removeSettingsListener();
    }

    private void OnBackSettingsButtonClick(){
        MainMenu.SetActive(true);
        SettingsMenu.SetActive(false);

        addMainPageListener();
        removeSettingsListener();
    }

    private void OnResetButtonClick(){
        //Destroy all boids and cubes and spawn 16 boids 
        initScript.Thanos();
    }

    private void OnPersistanceButtonClick(){
        //Fade out or not
        scenePlane.changeFadeOutMod();
    }



    // More Settings Buttons -------------------------------------
    private void OnLeftRightButtonClick(){
        //Switch the menu on the left or right hand
        MenuLeft.SetActive(!MenuLeft.activeSelf);
        MenuRight.SetActive(!MenuRight.activeSelf);
    }

    private void OnDebugPanelButtonClick(){
        //Make the debug panel appears
        DebugPanel.SetActive(!DebugPanel.activeSelf);
    }

    private void OnDebugWallButtonClick(){
        //Make the plan appears
        scenePlane.OnTogglePlanesAction();
    }

    private void OnBackMoreSettingsButtonClick(){
        SettingsMenu.SetActive(true);
        MoreSettingsMenu.SetActive(false);

        addSettingsListener();
        removeMoreSettingsListener();
    }

    //For initSceneScript
    public void setScene(InitSceneScript sc)
    {
        this.initScript = sc;
    }
}