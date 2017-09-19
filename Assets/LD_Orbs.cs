using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LD_Orbs : MonoBehaviour {

    public int orbNumber;


	// Use this for initialization
	void Awake () {
        GetComponent<RainbowColor>().enabled = false;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            OnTakenOrb(orbNumber);

            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<RainbowScale>().enabled = false;
            transform.DOShakeScale(.45f, 2, 12, 90).SetEase(Ease.InBounce);
            GetComponent<RainbowColor>().enabled = true;
            DOVirtual.DelayedCall(.8f, () => {
                GetComponent<RainbowColor>().enabled = false;
                GetComponent<SpriteRenderer>().DOFade(0, .2f).OnComplete(()=> {
                    Destroy(this);
                });
            });
        }
    }


    void OnTakenOrb(int orb)
    {
        if(orb == 1)
        {


        }

        //etc...
    }
}
