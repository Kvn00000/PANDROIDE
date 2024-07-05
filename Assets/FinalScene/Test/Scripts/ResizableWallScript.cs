using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


public class ResizableWallScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 _centerInit;
    private Quaternion _tablerotation;
    private XRGrabInteractable grabInteractable;
    private List<XRBaseInteractor> interactors = new List<XRBaseInteractor>();
    private MeshFilter mesh;
    private XRBaseInteractor interactor2;
    private Vector3 StartControllerPos;
    private Vector3 previousPos;
    private bool isOn=true;

    private string surfaceDetected;


    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        mesh = GetComponent<MeshFilter>();
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
            // Debug.Log(args.interactor.transform.position);

            // Debug.Log(this.transform.forward);



            if (interactors.Count == 2)
            {
                interactor2 = args.interactor;
                StartControllerPos = interactor2.transform.position;

                previousPos = interactor2.transform.position;
                //grabInteractable.trackRotation = false;

                // Vector3 originalCoords = RotatePointAroundPivot(interactor2.transform.position,this.transform.position,Quaternion.Inverse(this.transform.rotation).eulerAngles);

                surfaceDetected = DetectGrabbedFace(args.interactor.transform.position);
            }
        }
    }


    private void OnRelease(SelectExitEventArgs args)
    {
        //Quand on relache l'objet on le supprime
        if (interactors.Contains(args.interactor))
        {
            interactors.Remove(args.interactor);

            if (interactors.Count < 2)
            {
                XRGrabInteractable gr = this.GetComponent<XRGrabInteractable>();
                gr.trackRotation = true;
                surfaceDetected = "";

            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        //Debug.Log(isInnerResize(new Vector3(0, 0.15f, 0)));
        if (interactors.Count == 2 && Vector3.Distance(interactor2.transform.position, previousPos) > 0.001)
        {
            float distance = Vector3.Distance(interactor2.transform.position, previousPos);
            Vector3 current = interactor2.transform.position;

            Vector3 test_prev = this.transform.InverseTransformPoint(previousPos);
            Vector3 test_cur = this.transform.InverseTransformPoint(current);

            // Vector3 distance = interactor2.transform.position - previousPos;
            // Debug.Log(distance);
            switch (surfaceDetected)
            {
                case "Front":
                    if (test_cur.z - test_prev.z < 0)
                    {

                        // if( Vector3.Distance(interactor2.transform.position ,StartControllerPos) > Vector3.Distance(previousPos ,StartControllerPos) ){
                        // Debug.Log(" Diff positive");
                        if (isInnerResize(current))
                        {
                            //resizeCircleInt(distance, "z", true);
                        }
                        else
                        {
                            resizeCube(distance, "z", true);
                        }
                    }
                    else
                    {
                        // Debug.Log(" Diff neg");

                        if (isInnerResize(current))
                        {
                            //resizeCircleInt(distance, "z", false);
                        }
                        else
                        {
                            resizeCube(distance, "z", false);
                        }
                    }
                    previousPos = interactor2.transform.position;

                    break;

                case "Left":
                    //Debug.Log("IS inner ? : " + isInnerResize(current));
                    if (test_cur.x - test_prev.x > 0)
                    {

                        // if( Vector3.Distance(interactor2.transform.position ,StartControllerPos) > Vector3.Distance(previousPos ,StartControllerPos) ){
                        // Debug.Log(" Diff positive");
                        if (isInnerResize(current))
                        {
                            //resizeCircleInt(distance, "x", true);
                        }
                        else
                        {
                            resizeCube(distance, "x", true);
                        }
                    }
                    else
                    {
                        // Debug.Log(" Diff neg");
                        if (isInnerResize(current))
                        {
                            //resizeCircleInt(distance, "x", false);
                        }
                        else
                        {
                            resizeCube(distance, "x", false);
                        }
                    }
                    previousPos = interactor2.transform.position;
                    break;

                case "Right":
                    //Debug.Log("IS inner ? : " + isInnerResize(current));
                    if (test_cur.x - test_prev.x < 0)
                    {

                        // if( Vector3.Distance(interactor2.transform.position ,StartControllerPos) > Vector3.Distance(previousPos ,StartControllerPos) ){
                        // Debug.Log(" Diff positive");
                        if (isInnerResize(current))
                        {
                            //resizeCircleInt(distance, "x", true);
                        }
                        else
                        {
                            resizeCube(distance, "x", true);
                        }
                    }
                    else
                    {
                        // Debug.Log(" Diff neg");
                        if (isInnerResize(current))
                        {
                            //resizeCircleInt(distance, "x", false);
                        }
                        else
                        {
                            resizeCube(distance, "x", false);
                        }
                    }
                    previousPos = interactor2.transform.position;

                    break;

                case "Back":
                    //Debug.Log("IS inner ? : " + isInnerResize(current));
                    // Debug.Log(interactor2.transform.position.z);
                    // Debug.Log(previousPos.z);

                    // Debug.Log("fjgklf");
                    // Debug.Log(Vector3.Distance(interactor2.transform.position ,StartControllerPos) == Vector3.Distance(previousPos ,StartControllerPos));

                    // Debug.Log(Vector3.Distance(interactor2.transform.position ,StartControllerPos));
                    // Debug.Log(Vector3.Distance(previousPos ,StartControllerPos));

                    // distance = interactor2.transform.position.z - StartControllerPos.z;
                    if (test_cur.z - test_prev.z > 0)
                    {

                        // if( Vector3.Distance(interactor2.transform.position ,StartControllerPos) > Vector3.Distance(previousPos ,StartControllerPos) ){
                        // Debug.Log(" Diff positive");
                        if (isInnerResize(current))
                        {
                            //resizeCircleInt(distance, "z", true);
                        }
                        else
                        {
                            resizeCube(distance, "z", true);
                        }
                    }
                    else
                    {
                        // Debug.Log(" Diff neg");
                        if (isInnerResize(current))
                        {
                            //resizeCircleInt(distance, "z", false);
                        }
                        else
                        {
                            resizeCube(distance, "z", false);
                        }
                    }
                    previousPos = current;
                    break;

                case "":
                    break;


            }

        }
    }



    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 centre, Vector3 angles)
    {
        Vector3 dir = point - centre; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        Vector3 nPoint = dir + centre;
        return nPoint; // return it
    }
    private bool isInnerResize(Vector3 posGrab) {

        // Parent Arene
        //           --> collider permettant le grab
        //           --> Top Arene
        //                   --> mur interieur
        //                   --> mur exterieur
        GameObject childInt = this.transform.GetChild(1).GetChild(0).gameObject;
        GameObject childExt = this.transform.GetChild(1).GetChild(1).gameObject;


        //Debug.DrawLine(posGrab, pointInt,Color.green);
        //Debug.DrawLine(posGrab, pointExt,Color.red) ;

        //Debug.DrawLine(posGrab, pointTest, Color.cyan);
        //Debug.DrawLine(posGrab, pointTest2, Color.yellow);
        //Debug.Log("/////////////////////////");
        //Debug.Log("POS Grab : "+ posGrab+",  point Int : "+ pointInt+ ", point Ext : "+ pointExt);
        //Debug.Log("Dist Int : "+distInt+" Dist Ext : "+distExt);
        //Debug.Log("/////////////////////////");
        float radiusInt = childInt.GetComponent<CircleWallScript>().getRadius();
        float distance = Vector3.Distance(posGrab,this.transform.position);
        float thickness = this.transform.GetChild(1).GetComponent<CircleWallScript>().getThickness() ;
        //Debug.DrawLine(posGrab, this.transform.position, Color.green);
        float toComp = radiusInt+(thickness*0.5f);
        //Debug.Log("Distance : " + distance+ "  toComp :"+toComp+" radiusInt : "+radiusInt);
        if (distance <toComp ) {
            return true;
        }
        else
        {
            return false;
        }


    }
    public void resizeCube(float amount, string axis, bool inverse)
    {
        //Debug.Log("Here");
        // amount = amount * 0.05f;
        switch (axis)
        {
            case "x":
                if (!inverse)
                {
                    //this.transform.position = new Vector3(this.transform.position.x+(amount/2), this.transform.position.y, this.transform.position.z);
                    this.transform.localScale = new Vector3(this.transform.localScale.x + amount, this.transform.localScale.y, this.transform.localScale.z);
                }
                else
                {

                    //this.transform.position = new Vector3(this.transform.position.x-(amount/2), this.transform.position.y, this.transform.position.z);
                    this.transform.localScale = new Vector3(this.transform.localScale.x - amount, this.transform.localScale.y, this.transform.localScale.z);
                }
                break;
            case "z":
                if (!inverse)
                {
                    //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + (amount / 2));
                    this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z + amount);
                }
                else
                {
                    //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z- (amount / 2));
                    this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z - amount);
                }
                break;
            default:
                break;

        }
        //Debug.Log(" DONE");
        this.transform.position = _centerInit;
        this.transform.rotation = _tablerotation;

    }
    public void resizeCircleInt(float amount, string axis, bool inverse)
    {
        //Debug.Log("Here");
        // amount = amount * 0.05f;
        GameObject child = this.transform.GetChild(1).GetChild(0).gameObject;
        switch (axis)
        {
            case "x":
                if (!inverse)
                {
                    //this.transform.position = new Vector3(this.transform.position.x+(amount/2), this.transform.position.y, this.transform.position.z);
                    child.transform.localScale = new Vector3(this.transform.localScale.x + amount, this.transform.localScale.y, this.transform.localScale.z+amount);
                }
                else
                {

                    //this.transform.position = new Vector3(this.transform.position.x-(amount/2), this.transform.position.y, this.transform.position.z);
                    child.transform.localScale = new Vector3(this.transform.localScale.x - amount, this.transform.localScale.y, this.transform.localScale.z-amount);
                }
                break;
            case "z":
                if (!inverse)
                {
                    //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + (amount / 2));
                    child.transform.localScale = new Vector3(this.transform.localScale.x+ amount, this.transform.localScale.y, this.transform.localScale.z + amount);
                }
                else
                {
                    //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z- (amount / 2));
                    child.transform.localScale = new Vector3(this.transform.localScale.x- amount, this.transform.localScale.y, this.transform.localScale.z - amount);
                }
                break;
            default:
                break;

        }

        //Debug.Log(" DONE");
        this.transform.position = _centerInit;
        this.transform.rotation = _tablerotation;
        //child.GetComponent<CircleWallScript>().updateTop();
    }
    private string DetectGrabbedFace(Vector3 contactPoint)
    {
        // Assuming the object is a cube, define the face normals
        Dictionary<string, Vector3> faceNormals = new Dictionary<string, Vector3>
        {
            { "Top", Vector3.up },
            { "Bottom", Vector3.down },
            { "Front", Vector3.forward },
            { "Back", Vector3.back },
            { "Left", Vector3.left },
            { "Right", Vector3.right }
        };

        // Find the closest face
        string closestFace = "";
        float maxDot = float.MinValue;

        foreach (var face in faceNormals)
        {
            Vector3 faceNormal = this.transform.TransformDirection(face.Value);
            float dot = Vector3.Dot((contactPoint - this.transform.position).normalized, faceNormal);

            if (dot > maxDot)
            {
                maxDot = dot;
                closestFace = face.Key;
            }
        }

        Debug.Log("Grabbed face: " + closestFace);

        return closestFace;
    }

    public void SetCenterInit(Vector3 pos) {
        this._centerInit = pos;
    }
    public void SetTableRotation(Quaternion rotation)
    {
        this._tablerotation = rotation;
    }
    public void enableModif() {
        if (isOn)
        {

            XRGrabInteractable grab= this.GetComponent<XRGrabInteractable>();
            int defaultLayer = InteractionLayerMask.NameToLayer("Default");
            grab.interactionLayers = InteractionLayerMask.GetMask(InteractionLayerMask.LayerToName(defaultLayer)) ;

        }
        else
        {
            XRGrabInteractable grab = this.GetComponent<XRGrabInteractable>();
            int nothingL = InteractionLayerMask.NameToLayer("Nothing");
            grab.interactionLayers = InteractionLayerMask.GetMask(InteractionLayerMask.LayerToName(nothingL));
        }
    }
}
