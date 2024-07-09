using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularMenuLeft : MonoBehaviour{
    //Spawner Left and Right
    private SpawnBoidScript SpawnerRight;
    private SpawnBoidScript SpawnerLeft;


    //ObjectColor
    private string boidColorHex = "#d55e00";
    private string cubeColorHex = "#029e73";
    private string NothingColorHex = "#FFFFFF";


    public GameObject initScene;
    //RightPoke on the left hand and LeftPoke on the Right hand
    public GameObject pokeInteractor;


    [Header("Debug button param")]

    public GameObject DebugPanel;
    public GameObject onTextLeft;
    public GameObject offTextLeft;
    public GameObject onTextRight;
    public GameObject offTextRight;



    [Header("Spray")]
    public Material sprayMaterial;
    public GameObject SprayBottle;


    [Header("Controllers")]
    public GameObject LeftController;
    public GameObject RightController;


    [Header("Left Right Menus")]
    public GameObject MenuLeft;
    public GameObject MenuRight;


    [Header("Menus Page")]
    public GameObject MainMenu;
    public GameObject SettingsMenu;
    public GameObject WallMenu;

    
    [Header("Main Menu Button")]
    public Button SettingsButton;
    public Button BoidButton;
    public Button CubeButton;
    public Button NothingButton;


    [Header("Settings Button")]
    public Button LeftRightButton;
    public Button BackSettingsButton;
    public Button WallButton;
    public Button DebugButton;


    [Header("WallMenu Button")]
    public Button InformationButton;
    public Button MinusButton;
    public Button PlusButton;
    public Button BackWallButton;






    void Start(){
        //Delete all the invisible part of the buttons and add listener only for the MainMenu
        InitMainMenu();
        InitSettingsMenu();
        InitWallMenu();

        SpawnerRight = RightController.GetComponent<SpawnBoidScript>();
        SpawnerLeft = LeftController.GetComponent<SpawnBoidScript>();
        
    }

    void OnEnable(){
        if(LeftRightButton){
            LeftRightButton.onClick.AddListener(OnLeftRightButtonClick);
        }
        if(BackSettingsButton){
            BackSettingsButton.onClick.AddListener(OnBackSettingsButtonClick);
        }
        if(WallButton){
            WallButton.onClick.AddListener(OnWallButtonClick);
        }
        if(DebugButton){
            DebugButton.onClick.AddListener(OnDebugButtonClick);
        }
    }

    void OnDisable(){
        if(LeftRightButton){
            LeftRightButton.onClick.RemoveListener(OnLeftRightButtonClick);
        }
        if(BackSettingsButton){
            BackSettingsButton.onClick.RemoveListener(OnBackSettingsButtonClick);
        }
        if(WallButton){
            WallButton.onClick.RemoveListener(OnWallButtonClick);
        }
        if(DebugButton){
            DebugButton.onClick.RemoveListener(OnDebugButtonClick);
        }
    
    }


    private void InitMainMenu(){
        SettingsButton.transform.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        BoidButton.transform.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        CubeButton.transform.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        NothingButton.transform.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;

        //Add Listener
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

    private void InitSettingsMenu(){
        LeftRightButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        BackSettingsButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        WallButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        DebugButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

    private void InitWallMenu(){
        InformationButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        MinusButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        PlusButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        BackWallButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;

        
    }


    //Main Menu Event
    public void OnSettingsButtonClick(){

        //Switch to the Settings Menu
        SettingsMenu.SetActive(true);

        if(MainMenu.activeSelf == true){
            MainMenu.SetActive(false);

            //Add SettingsMenu Listener
            if(LeftRightButton){
                LeftRightButton.onClick.AddListener(OnLeftRightButtonClick);
            }
            if(BackSettingsButton){
                BackSettingsButton.onClick.AddListener(OnBackSettingsButtonClick);
            }
            if(WallButton){
                WallButton.onClick.AddListener(OnWallButtonClick);
            }
            if(DebugButton){
                DebugButton.onClick.AddListener(OnDebugButtonClick);
            }

            //Remove MainMenu Listener
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


        if(SprayBottle.activeSelf == true){
            SprayBottle.SetActive(false);
        }

        if(pokeInteractor.activeSelf == false){
            pokeInteractor.SetActive(true);
        }

        //Les spray ne spawn rien + Couleur du spray = blanc
        SpawnerRight.toInstantiate = 0;
        SpawnerLeft.toInstantiate = 0;
        Color color;
        ColorUtility.TryParseHtmlString(NothingColorHex, out color);
        sprayMaterial.SetColor("_BaseColor", color);

    }

    public void OnBoidButtonClick(){
        
        if(SprayBottle.activeSelf == false){
            SprayBottle.SetActive(true);
        }

        if(pokeInteractor.activeSelf == true){
            pokeInteractor.SetActive(false);
        }
        // Le spray fait spawn des boids + changement de couleur du spray
        SpawnerRight.toInstantiate = 1;
        SpawnerLeft.toInstantiate = 1;
        Color color;
        ColorUtility.TryParseHtmlString(boidColorHex, out color);
        sprayMaterial.SetColor("_BaseColor", color);
    }

    public void OnCubeButtonClick(){
        if(SprayBottle.activeSelf == false){
            SprayBottle.SetActive(true);
        }

        if(pokeInteractor.activeSelf == true){
            pokeInteractor.SetActive(false);
        }

        //Le spray fait spawn des Cube + changemenr de couleur du spray
        SpawnerRight.toInstantiate = 2;
        SpawnerLeft.toInstantiate = 2;
        Color color;
        ColorUtility.TryParseHtmlString(cubeColorHex, out color);
        sprayMaterial.SetColor("_BaseColor", color);
    }

    public void OnNothingButtonClick(){
        if(SprayBottle.activeSelf == true){
            SprayBottle.SetActive(false);
        }

        if(pokeInteractor.activeSelf == false){
            pokeInteractor.SetActive(true);
        }
        SpawnerRight.toInstantiate = 0;
        SpawnerLeft.toInstantiate = 0;
        Color color;
        ColorUtility.TryParseHtmlString(NothingColorHex, out color);
        sprayMaterial.SetColor("_BaseColor", color);
    }

    //Settings Menu Event

    public void OnLeftRightButtonClick(){
        //Switch the menu on the left or right hand
        MenuLeft.SetActive(!MenuLeft.activeSelf);
        MenuRight.SetActive(!MenuRight.activeSelf);
    }

    public void OnDebugButtonClick(){
        //Turn on or off the debug panel
        DebugPanel.SetActive(!DebugPanel.activeSelf);

        onTextLeft.SetActive(!onTextLeft.activeSelf);
        offTextLeft.SetActive(!offTextLeft.activeSelf);

        onTextRight.SetActive(!onTextRight.activeSelf);
        offTextRight.SetActive(!offTextRight.activeSelf);
    }

    public void OnBackSettingsButtonClick(){
        MainMenu.SetActive(true);
        SettingsMenu.SetActive(false);

        //Add MainMenu Listener
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

        //Remove Settings Menu Listener
        if(LeftRightButton){
            LeftRightButton.onClick.RemoveListener(OnLeftRightButtonClick);
        }
        if(BackSettingsButton){
            BackSettingsButton.onClick.RemoveListener(OnBackSettingsButtonClick);
        }
        if(WallButton){
            WallButton.onClick.RemoveListener(OnWallButtonClick);
        }
        if(DebugButton){
            DebugButton.onClick.RemoveListener(OnDebugButtonClick);
        }

    }

    public void OnWallButtonClick(){
        WallMenu.SetActive(true);
        SettingsMenu.SetActive(false);

        //Add WallMenu Listener

        //Useless Maybe
        // if(InformationButton){
        //     InformationButton.onClick.AddListener();
        // }
        if(MinusButton){
            MinusButton.onClick.AddListener(OnMinusButtonClick);
        }
        if(PlusButton){
            PlusButton.onClick.AddListener(OnPlusButtonClick);
        }
        if(BackWallButton){
            BackWallButton.onClick.AddListener(OnBackWallButtonClick);
        }


        //Remove SettingsMenu Listener
        if(LeftRightButton){
            LeftRightButton.onClick.RemoveListener(OnLeftRightButtonClick);
        }
        if(BackSettingsButton){
            BackSettingsButton.onClick.RemoveListener(OnBackSettingsButtonClick);
        }
        if(WallButton){
            WallButton.onClick.RemoveListener(OnWallButtonClick);
        }
        if(DebugButton){
            DebugButton.onClick.RemoveListener(OnDebugButtonClick);
        }
    }


    //Wall Menu Event

    public void OnMinusButtonClick(){

    }
    public void OnPlusButtonClick(){

    }
    public void OnBackWallButtonClick(){
        SettingsMenu.SetActive(true);
        WallMenu.SetActive(false);



        //Add SettingsMenu Listener
        if(LeftRightButton){
            LeftRightButton.onClick.AddListener(OnLeftRightButtonClick);
        }
        if(BackSettingsButton){
            BackSettingsButton.onClick.AddListener(OnBackSettingsButtonClick);
        }
        if(WallButton){
            WallButton.onClick.AddListener(OnWallButtonClick);
        }
        if(DebugButton){
            DebugButton.onClick.AddListener(OnDebugButtonClick);
        }


        //Remove WallMenu Listener

        //Useless Maybe
        // if(InformationButton){
        //     InformationButton.onClick.RemoveListener();
        // }
        if(MinusButton){
            MinusButton.onClick.RemoveListener(OnMinusButtonClick);
        }
        if(PlusButton){
            PlusButton.onClick.RemoveListener(OnPlusButtonClick);
        }
        if(BackWallButton){
            BackWallButton.onClick.RemoveListener(OnBackWallButtonClick);
        }
    }
}