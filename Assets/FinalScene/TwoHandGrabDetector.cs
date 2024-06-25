using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandGrabDetector : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private List<XRBaseInteractor> interactors = new List<XRBaseInteractor>();

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (!interactors.Contains(args.interactor))
        {
            interactors.Add(args.interactor);
        }

        if (interactors.Count == 2)
        {
            Debug.Log("Object grabbed with both hands.");
            // Your logic when the object is grabbed with both hands
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (interactors.Contains(args.interactor))
        {
            interactors.Remove(args.interactor);
        }

        if (interactors.Count < 2)
        {
            Debug.Log("Object is no longer grabbed with both hands.");
            // Your logic when the object is no longer grabbed with both hands
        }
    }
}
