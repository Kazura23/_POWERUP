using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFairy : MonoBehaviour {

    public static PlayerFairy Singleton;
	// Use this for initialization
	void Awake () {
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
        
        
    }
}
