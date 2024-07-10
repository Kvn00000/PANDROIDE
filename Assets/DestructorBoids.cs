using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructorBoids : MonoBehaviour
{
    private Transform parent;
    private int _layerBoid;
    /*
    0 --> Destroy direct 
    1 --> Destroy with FadeOut
    */
    private int _fadeOut = 0;

    // Start is called before the first frame update
    void Start()
    {
        parent = this.transform.parent;
        _layerBoid = LayerMask.NameToLayer("BOID");
        if (PlayerPrefs.HasKey("_FadeOutMode"))
        {
            _fadeOut = PlayerPrefs.GetInt("_FadeOutMode");
        }
    }
    private void Update()
    {
    }
    public void Thanos()
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.layer ==_layerBoid)
            {
                if (_fadeOut == 0)
                {
                    Destroy(child.gameObject);
                }
                else
                {
                    //ADD FADOUT CALL
                }
                
            }
        }
    }

    public void ChangeFadeOutMod()
    {
        if (_fadeOut == 0)
        {
            _fadeOut = 1;
        }
        else
        {
            _fadeOut = 0;
        }
    }

    /*
     Saves Functions
    */
    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            PlayerPrefs.SetInt("_FadeOutMode", _fadeOut);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            PlayerPrefs.SetInt("_FadeOutMode", _fadeOut);
        }
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("_FadeOutMode", _fadeOut);
    }
}
