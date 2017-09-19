using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoThroughPlatform : MonoBehaviour {

    private GameObject Plateform;

	// Use this for initialization
	void Start () {
        Plateform = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            Plateform.GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine("Wait");
            PlayerMovement.Singleton.Reset();
            //Debug.Log("Player");
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(.05f);
        Plateform.GetComponent<BoxCollider2D>().enabled = true;
    }

}
