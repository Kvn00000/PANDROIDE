using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmitToBox : MonoBehaviour
{

    public float step = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Growingbox myParent = (Growingbox)transform.parent.GetComponent<Growingbox>();
        myParent.grow(step);
        //Vector3 myPos = this.transform.position;
        //Vector3 newPos = new Vector3(myPos.x,myPos.y+step,myPos.z);
        //this.transform.position = newPos;
        if (transform.position.y > 5)
        {
            step = -step;
        }
        if ((transform.position.y < 1)&&(step<0))
        {
            step = -step;
        }
    }
}
