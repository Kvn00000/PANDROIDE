# Particube


## Table of content
 - [Project Description](#project-description)
 - [Installation ](#installation)
 - [Launch on an Oculus Quest3 headset](#launch-on-an-oculus-quest3-headset)
 - [Users Guide](#users-guide)
 - [Developper Part](#developper-part)
    - [Project Architecture](#project-architecture)
    - [Atelier Test Boid](#atelier-test-boid)
    - [Final Scene Folder](#final-scene-folder)
    - [Prefabs Folder](#prefabs-folder)
    - [Scripts Folder](#scripts-folder)
        - [Boids Script folder ](#boids-script-folder)
        - [Cubes scripts folder ](#cubes-scripts-folder)
        - [Destruction scripts folder ](#destruction-scripts-folder)
        - [Ground_Arena scripts folder ](#ground_arena-scripts-folder)
        - [Interraction scripts folder ](#interraction-scripts-folder)
        - [Initialisation scene scripts folder ](#initialisation-scene-scripts-folder)
        - [UI scripts folder ](#ui-scripts-folder)
        - [Wall scripts](#wall-scripts-folder)
    - [TestScene Folder](#testscene-folder)
    - [Final Scene MR Intro](#final-scene-mr-intro)

## Project Description
This project demonstrates a simulation of collective movements in virtual reality, where users can interact with cubes and boids represented as paper planes. An arena will spawn on a table and users can interact by grabbing objects or resizing cubes.

## Installation

 1. Install Unity Hub with version 2022.3.17f1.
 2. Clone the repository to your laptop.
 3. Add the project to Unity Hub by clicking the arrow (blue arrow) and selecting "Add project from disk" (red arrow), then choose the project.
 ![UnityHub](/image/UnityHub.png)
 4. The project Particube appears in the list, click to open.
 5. The main scene we are using is "FinalSceneMR", located in the Assets/FinalScene folder.

## Launch on an Oculus Quest3 headset
 1. Before each build on the headset, ensure that the "XR Device Simulator is disabled in the scene.
 ![DeviceSimulator](/image/DeviceSimulator.png)

 2. Navigate to the "FinalSceneMR".

 3. Click on the "File" menu in the top left corner, and select Build Settings.

 4. Connect your headset to your computer, select the Android plateform. Choose your headset from the "Run device" dropdown. If it does not appear, click the "Refresh" button (blue arrow) and then click on the "Switch Platform" button (red arrow).


<!-- <img src="./image/Build%20Settings.png" width="60%" style="display: block; margin: 0 auto;"> -->

<div style="text-align: center;">
    <img src="./image/Build%20Settings.png" width="60%" style="display: inline-block;">
</div>

 
 5. Finally, click on the "Build and Run" button, and the project will launch in your Oculus Quest3 headset.

## Users Guide
 When the project is launched on the headset, press the Unity icon to start. You will see an arena on the table with PaperPlanes spawning inside.

 - Grab Interaction: When the small sphere is near a paper plane or a cube, you will feel a slight vibration. Press the trigger with your middle finger to grab an object.

 - Poke Interaction: A watch is available on the left hand. By tapping buttons, you can switch between different modes. If you are left-handed, you can switch the watch to the right side by tapping the "..." button twice and then the "Right" button once.

 - Spawning object: On the first page of the watch, you can choose to spawn either a paper plane or a cube. When a spawn mode is selected, a spray will appear. Pull the trigger with your index finger to spawn an object.

 - Resizing a Cube: To resize a cube, grab it with one hand, then use your other hand to grab a different side of the cube. The color of the side will change, indicating that you can resize it.

## Developper part

### Project architecture

 In the **Asset** Folder  you will find all the files related to the project.
 
 Inside you will find a lot of folder. The main folders are the **Atelier Test Boid** and the **FinalScene**.

### Atelier Test Boid 

 In the **Atelier Test Boid** you will find previous iterations of the boids code. They are not all working but shows that they are diffrent ways to implements these behaviours in Unity. There you also have the **_BoidWorkshop_** scene where you will be able to test boids prefab with differents arenas and obstacles. It is recommended to use this scene if you need to change the boids behaviour to tune before using them in the real scene.

### Final Scene folder
In this folder you will find the most recent version of the code. This code is functionnal with the following architecture :

- **Imported prefab** contains as the name implies the imported prefabs like the spray  and the paper plane asset. 
- **Materials** contains the materials of all prefabs. There are transparent variants needed for the fadeOut.
- **Prefabs** contains all the prefabs not imported. 
- **Scripts** contains all the scripts used in the scene
- **TestScene** contains the scenes used for testing.

In this folder you will also find _FinalSceneMR_ scene where the final version of the project is implemented.

### Prefabs folder

The Prefabs folder have the following object :
* Boid --> The prefab of the boid
* Ground --> The folder containing the prefabs for the plane and the ground of the arena
* ARPlaneColored --> a prefab used in the visualization of the plan in the debug.
* ParentArena --> The parent arena that was used in resize of the arena feature. This is disabled in this version of the project
* PlanDensity --> a prefab that act as a bounding box for the table to avoid the problem where boid dive through the plan.
* ResizeCube --> the prefab of the cube used in the project.
* Wall --> The prefab of wall of the arena

###  Scripts Folder
Each script is sorted according to the feature of the script.
The folder follows this architecture :
* AR Colorization --> contain the script used to color the planes
* Boids --> scripts related to the boids 
* Cubes --> scripts related to the cubes
* Destruction --> scripts related to the destruction of the objects
* Ground_Arena --> scripts related the plane of the arena
* Interraction --> scripts related to the MR interractions
* initialisation scene --> scripts related to the creation of the arena and the plane detection
* UI --> scripts related to the User Interface
* Wall --> scripts related to the creation of the walls of the arena

#### Boids Script folder 
The folder contains :
* **_boidTuning.cs_** --> The main code of the boids
* **_CounterG2.cs_** --> The code that handle the ground collisions of the boids

#### Cubes scripts folder
The folder contains :
* Coloration --> A folder with the scripts to color each face of the cube if the face is selected
* **_CubeScale.cs_** --> Script used to resize the cube in each dimension
* **_ResizeCubeGravity.cs_** -->  change the mass of the cube if the cube is on the ground. Prevent boids to push the cube

#### Destruction scripts folder
The folder contains :
* **_DestroyGroundScript.cs_** --> Script used to destroy the cubes on the ground and boids if persistance is disabled
* **_FadeOutScript.cs_** -->  Script containing the fadeOut executed by the cubes and boids before destruction. If the object is grabbed cancel the fadeOut.

#### Ground_Arena scripts folder
The folder contains :
* Scripts called **_BackGround.cs_** and others that create the tile of the tiled version of the project. Not used because of poor performances.
* **_plane.cs_** --> Create the plane of the arena.
#### Interraction scripts folder
The folder contains :
* **_pull.cs_** --> Handle the resize for a tile of the tiled version of the project. Not used.
* **_SpawnBoidScript.cs_** --> Allow the user to spawn boids or cube in the limits decided by the dev. Limits are now 100 Boids and 100 cubes. Higher values results in lower FPS.

#### Initialisation scene scripts folder
The folder contains :
* **_InitSceneScript.cs_** --> One of the most important script. Initialize the arena and modify the parameters of spawned boids. Also impact the number of sides of the arena.
* **_ScenePlaneDetectController.cs_** --> On of the most important script. Detect the AR planes and spawn the arena using **_InitSceneScript.cs_**. Handle the save of parameters like the persistance Arena dimensions and spawn to rebuild the scene by pressing meta button twice if the user go out of bounds.

#### UI scripts folder
The folder contains :
* Image --> a folder that contains the images of the circular menu
* **_CircularMenu.cs_** --> Handle the interraction and navigation of the circular menu. 
* **_HandleMenuAffordance.asset_** --> is used to make the circular menu appear.
#### Wall scripts folder
The folder contains :
* **_CircleWallScript.cs_** --> Create a sided wall. if the number of side is high, create a circular wall. Also contains functions to draw the top of a wall to allow wall with a thickness.
* **_ResizableWallScript.cs_**  --> Allow the user to resize the arena. Feature disabled in the lastest build because not that usefull a source of problem with the On/Off of this mod.

### TestScene folder
This folder contains differents scene used for testing feature in isolation. Very usefull if you want to keep unity debug information and if you know how to use the XR device simulator.

### Final Scene MR Intro

the final scene contain different game objects. The most important game object are :
* the XR Hands Information Setup  --> Contains most of the MR device with the XR Origin
* the Init Scene --> This is where you can change the infos easily to propagate toward boids and spawner
* the XR device simulator -->  enable this if you want to test a scene without a headset on. 
* The debug panel --> not active because debug is disabled at start.

If you want to modify the spawner and menus you will find them in the in the XR Origin in the left and right controller.

![FinalSceneMR](/image/finalSceneMRImage.PNG)


