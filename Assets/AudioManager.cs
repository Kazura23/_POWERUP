using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Singleton;

    //private AudioSource[] AudioList;

    void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(this);
        }
        

    }

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
