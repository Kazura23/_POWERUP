using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using DG.Tweening;

public class ControllerPlayer : MonoBehaviour {

    public Player player;

    //public float Direction;

    public static ControllerPlayer Singleton;


    void Awake()
    {
        player = ReInput.players.GetPlayer(0);

        if(Singleton == null)
        {
            Singleton = this;
        }

    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void LateUpdate () {

        //Direction = player.GetAxis("Horizontal");

        

        if (player.GetButton("Left"))
        {
            PlayerMovement.Singleton.MoveRight();
        }
            
        else if (player.GetButton("Right"))
        {
            PlayerMovement.Singleton.MoveLeft();
        }
        

        if (player.GetButtonDown("Attack"))
        {
            PlayerPunch.Singleton.Attack();
        }
        //Debug.Log(Direction);
    }
}
