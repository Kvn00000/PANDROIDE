using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallsScript : MonoBehaviour
{
    private FrontWall front;
    private LeftWall left;

    private RightWall right;

    private BackWall back;

    protected float wallsize;

    //public float size = 10F;
    // Start is called before the first frame update
    public void Init(float size)
    {
        front = GetComponentInChildren<FrontWall>();
        front.Init(size);

        left = GetComponentInChildren<LeftWall>();
        left.Init(size);

        right = GetComponentInChildren<RightWall>();
        right.Init(size);

        back = GetComponentInChildren<BackWall>();
        back.Init(size);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
