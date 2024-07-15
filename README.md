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
 ![BuildSettings](/image/Build%20Settings.png){width = 80; height = 80}

 
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
- **Imported prefab**
- **Materials**
- **Prefabs**
- **Scripts**


