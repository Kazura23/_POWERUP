using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

public class DebutLevelCamera : MonoBehaviour
{
    public GameObject UIObject;
    private GameObject TitleScreen;

    public bool lockCam = false;


    public GameObject nextActived;

    void Awake()
    {
        //TitleScreen = UIObject.transform.GetChild(0).gameObject
    }

    // Use this for initialization
    void Start()
    {

        ProCamera2D.Instance.CameraTargets[0].TargetInfluenceH = 0;

    }

    // Update is called once per frame
    void Update()
    {


    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {

            //gameObject.SetActive(false);

            //ProCamera2D.Instance.AddCameraTarget(transform);
            //nextActived.SetActive(true);
            
            

            if (lockCam)
            {


                ProCamera2D.Instance.CameraTargets[0].TargetInfluenceH = 0;

            }

            else
            {
                

                //ProCamera2D.Instance.RemoveCameraTarget(transform);
                //ProCamera2D.Instance.CameraTargets.RemoveRange(1, 99999);
                ProCamera2D.Instance.CameraTargets[0].TargetInfluenceH = 1;
            }
        }


    }
}

