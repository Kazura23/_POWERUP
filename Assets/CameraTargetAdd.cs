using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

public class CameraTargetAdd : MonoBehaviour
{
    
    public GameObject nextActived;

    void Awake()
    {
        //TitleScreen = UIObject.transform.GetChild(0).gameObject
    }

    // Use this for initialization
    void Start()
    {

        //ProCamera2D.Instance.CameraTargets[0].TargetInfluenceH = 0;

    }
    
    void Update()
    {


    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            
            ProCamera2D.Instance.AddCameraTarget(transform);

            Destroy(this);
                //ProCamera2D.Instance.CameraTargets[0].TargetInfluenceH = 1;
            
        }


    }
}

