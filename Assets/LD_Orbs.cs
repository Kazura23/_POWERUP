using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Com.LuisPedroFonseca.ProCamera2D;

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
        ProCamera2D.Instance.Zoom(-2.2f, .5f);
        ControllerPlayer.Singleton.player.controllers.maps.SetMapsEnabled(false, 0);
        PlayerMovement.Singleton.body.constraints = RigidbodyConstraints2D.FreezeAll;
        //DOTween.PauseAll();
        UIManager.Singleton.powerUpScreen.GetComponent<RainbowColor>().enabled = true;

        DOVirtual.DelayedCall(3f, () => {

            UIManager.Singleton.powerUpScreen.GetComponent<RainbowColor>().enabled = false;

            ControllerPlayer.Singleton.player.controllers.maps.SetMapsEnabled(true, 0);
            PlayerMovement.Singleton.body.constraints = RigidbodyConstraints2D.None;
            ProCamera2D.Instance.Zoom(2.2f, .3f);
            DOTween.PlayAll();

        });


        if(orb == 1)
        {


        }

        //etc...
    }
}
