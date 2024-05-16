// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem.XR;

// public class IncreaseHeight : MonoBehaviour
// {
//     public XRController xrController; // Assurez-vous de référencer le contrôleur XR dans l'éditeur Unity
//     public float forceDeformation = 0.5f; // Force de déformation de l'objet

//     void Update()
//     {
//         // Vérifie si le contrôleur XR est en train d'interagir avec l'objet
//         if (xrController.IsInteracting(gameObject))
//         {
//             // Obtient la position du contrôleur XR
//             Vector3 positionController = xrController.GetPosition();

//             // Calcule la distance entre la position du contrôleur et la position de l'objet
//             float distance = Vector3.Distance(positionController, transform.position);

//             // Applique une déformation à l'objet en fonction de la distance
//             float deformation = forceDeformation / distance;
//             transform.localScale += new Vector3(deformation, deformation, deformation);
//         }
//     }
