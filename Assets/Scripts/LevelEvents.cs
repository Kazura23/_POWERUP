using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;


public class LevelEvents : MonoBehaviour {

    public Transform[] cameraTargets;

    

    public float characterXinfluence = .18f;

    public int cameraEvent = 1;

    public int zoneNumberEnemies = 0;
    public int zoneMaxEnemies = 5;


    private Transform nextFightZone;
    private bool canTrigger = true;


    public static LevelEvents Singleton;

	// Use this for initialization
	void Awake () {
        if(Singleton == null)
        {
            Singleton = this;
        }

        nextFightZone = transform.parent.transform.GetChild(cameraEvent + 1);
    }
	
	// Update is called once per frame
	void Update () {
        if (zoneNumberEnemies >= zoneMaxEnemies)
        {
            ProCamera2D.Instance.RemoveCameraTarget(cameraTargets[cameraEvent].transform);

            nextFightZone.gameObject.SetActive(true);
            ProCamera2D.Instance.CameraTargets[0].TargetInfluenceH = 1;
            Destroy(this);
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Player" && canTrigger)
        {
            ProCamera2D.Instance.AddCameraTarget(cameraTargets[cameraEvent].transform);
            canTrigger = false;
            ProCamera2D.Instance.CameraTargets[0].TargetInfluenceH = characterXinfluence;
        }
    }

    
}
