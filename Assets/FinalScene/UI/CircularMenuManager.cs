using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularMenuManager : MonoBehaviour
{
    public Material sprayMaterial;
    public GameObject SprayBottle;
    public GameObject LeftController;
    public GameObject RightController;

    public GameObject pokeInteractor;


    public GameObject MenuLeft;
    public GameObject MenuRight;

    //Spawner Left and Right
    private SpawnBoidScript SpawnerRight;
    private SpawnBoidScript SpawnerLeft;

    //ObjectColor
    private string boidColorHex = "#d55e00";
    private string cubeColorHex = "#029e73";
    private string NothingColorHex = "#FFFFFF";


    

    [Header("Menus")]
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
    public Button IDKButton;


    [Header("WallMenu Button")]
    public Button InformationButton;
    public Button MinusButton;
    public Button PlusButton;
    public Button BackWallButton;






    void Start(){
        //Delete all the invisible part of the buttons and add the listener for each button
        InitMainMenu();
        InitSettingsMenu();
        InitWallMenu();

        SpawnerRight = RightController.GetComponent<SpawnBoidScript>();
        SpawnerLeft = LeftController.GetComponent<SpawnBoidScript>();
        
    }


    private void InitMainMenu(){
        SettingsButton.transform.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        BoidButton.transform.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        CubeButton.transform.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        NothingButton.transform.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;

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
        IDKButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;


        if(LeftRightButton){
            LeftRightButton.onClick.AddListener(OnLeftRightButtonClick);
        }
        if(BackSettingsButton){
            BackSettingsButton.onClick.AddListener(OnBackSettingsButtonClick);
        }
        if(WallButton){
            WallButton.onClick.AddListener(OnWallButtonClick);
        }

        //Useless for now

        // if(IDKButton){
        //     IDKButton.onClick.AddListener();
        // }
    }

    private void InitWallMenu(){
        InformationButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        MinusButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        PlusButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        BackWallButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;

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
    }


    //Main Menu Event
    public void OnSettingsButtonClick(){
        SettingsMenu.SetActive(true);
        
        
        if(MainMenu.activeSelf == true){
            MainMenu.SetActive(false);
        }


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

    public void OnBoidButtonClick(){
        if(SprayBottle.activeSelf == false){
            SprayBottle.SetActive(true);
        }


        if(pokeInteractor.activeSelf == true){
            pokeInteractor.SetActive(false);
        }

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
        MenuLeft.SetActive(!MenuLeft.activeSelf);
        MenuRight.SetActive(!MenuRight.activeSelf);
    }

    public void OnBackSettingsButtonClick(){
        MainMenu.SetActive(true);
        SettingsMenu.SetActive(false);
    }

    public void OnWallButtonClick(){
        WallMenu.SetActive(true);
        SettingsMenu.SetActive(false);
    }


    //Wall Menu Event

    public void OnMinusButtonClick(){

    }
    public void OnPlusButtonClick(){

    }
    public void OnBackWallButtonClick(){
        SettingsMenu.SetActive(true);
        WallMenu.SetActive(false);
    }
}
