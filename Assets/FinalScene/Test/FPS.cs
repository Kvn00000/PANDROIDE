using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{

    private float fps;
    public TMPro.TextMeshProUGUI FPSCounterText;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("GetFPS",1 ,1);
    }

    void GetFPS (){
        fps = (int)(1f/ Time.unscaledDeltaTime);
        // fps = (9.0f * fps + 1.0f / Time.deltaTime) / 10.0f;
        FPSCounterText.text = "FPS : " + fps.ToString();
    }
}
