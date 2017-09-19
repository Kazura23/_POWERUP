using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Camera mainCamera;
    public GameObject player;
    public string ScreenShake;
    public bool CanPlay;

    public static GameManager Singleton;
	// Use this for initialization
	void Awake() {
		if(Singleton == null)
        {
            Singleton = this;
        } else
        {
            Destroy(this);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (CanPlay && !PlayerMovement.Singleton.IsStuned)
        {
            ControllerPlayer.Singleton.player.controllers.maps.SetMapsEnabled(true, 0);
        }
        else
        {
            ControllerPlayer.Singleton.player.controllers.maps.SetMapsEnabled(false, 0);
        }
	}
}
