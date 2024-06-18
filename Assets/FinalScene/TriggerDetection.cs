using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TriggerDetection : MonoBehaviour
{
    public InputActionAsset inputActions;
    private InputAction triggerAction;

    private void OnEnable()
    {
        // Récupérer l'action définie dans l'Input Action Asset
        var actionMap = inputActions.FindActionMap("XRI RightHand");
        triggerAction = actionMap.FindAction("Trigger");
        
        // Attacher les événements
        triggerAction.performed += OnTriggerPressed;
        
        // Activer l'action
        triggerAction.Enable();
    }

    private void OnDisable()
    {
        // Détacher les événements
        triggerAction.performed -= OnTriggerPressed;
        
        // Désactiver l'action
        triggerAction.Disable();
    }

    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        // Votre logique pour la détection de la gâchette
        Debug.Log("Trigger activated with value: " + context.ReadValue<float>());
        // Ajoutez ici votre logique pour la détection de la gâchette
    }
}
