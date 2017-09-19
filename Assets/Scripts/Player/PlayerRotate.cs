using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if(!GetComponent<PlayerMovement>().IsFlipped)
        {
            transform.eulerAngles = new Vector3(0, 1, 0); 
        }
        else if (GetComponent<PlayerMovement>().IsFlipped)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
    }
}
