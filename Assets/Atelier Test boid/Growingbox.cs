using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growingbox : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void grow(float step)
    {
        Vector3 lscale = this.transform.localScale;
        Vector3 nScale = new Vector3(lscale.x,lscale.y+step,lscale.z);
        Vector3 p = Vector3.up;
        this.transform.localScale = nScale;
        //transform.parent.Translate(p * step);
    }
}
