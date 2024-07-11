# Simulation de mouvements collectifs en réalité virtuelle

## Description du projet

## Installation et utilisation

 1. Installer Unity Hub ainsi que la version 2022.3.17f1.
 2. Cloner le projet github sur votre machine.
 3. Ajouter le projet dans le Unity Hub en cliquant sur la flèche à droite de add (fleche bleue) puis cliquer sur "Add project from disk" (flèche  rouge), puis sélectionnez l'emplacement du projet.
 ![UnityHub](/image/UnityHub.png)
 4. Le projet nommé PANDROIDE apparait dans la liste, cliquer dessus pour l'ouvrir.
 5. Nous utilisons la scène FinalSceneMR qui se trouve dans le dossier Assets/FinalScene.

## Lancement sur un casque Oculus Quest3
 1. Assurer vous avant chaque build sur le casque VR, que le XR Device Simulator est désactivé dans la scene.
 <!-- Mettre screen du device -->
 ![DeviceSimulator](/image/DeviceSimulator.png)

 2. Aller dans la scene FinalSceneMR

 3. Cliquer en haut a gauche sur l'onglet fichier, puis Build Settings.

 4. Brancher votre casque à votre machine puis sélectionner la plateforme Android, sélectionner votre casque dans le "Run device", s'il n'y apparait pas appuyer sur le bouton "Refresh" et cliquer sur le bouton "Switch Plateform".

 5. Enfin, cliquer sur le bouton "Build and Run", et le projet pourra se lancer sur votre casque Oculus Quest3.


## Prise en main du projet

 - Tous les fichiers et scènes utilisés sont dans le dossier Assets/FinalScene
 - Les Scripts utilisés sont dans le dossier Script, rangé dans chaque dossier correspondant. De même pour les prefabs qui sont dans le dossier "Prefab".
 - Nous avons importé 3 prefab qui se trouvent dans le dossier "Imported Prefab" : il comporte le debug panel, le mesh de l'avion en papier, ainsi que le spray