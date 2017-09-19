using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Com.LuisPedroFonseca.ProCamera2D;

public class LD_VeryBeginning : MonoBehaviour {

	// Use this for initialization
	void Start () {

        GameManager.Singleton.CanPlay = false;

        PlayerMovement.Singleton.MoveRight();
        PlayerMovement.Singleton.transform.DOMoveY(12.5f,0);
        PlayerMovement.Singleton.transform.DOMoveY(0, 1.5f).SetEase(Ease.InElastic, -1, .5f);
        PlayerMovement.Singleton.transform.DOMoveX(-15, 0);
        PlayerMovement.Singleton.transform.DOMoveX(PlayerMovement.Singleton.transform.position.x + 3f, 1.5f).SetEase(Ease.InElastic, - 1, .5f).OnComplete(() => {
            string ShakeName = "ShakeKill";
            ProCamera2DShake.Instance.Shake(ShakeName);
            DOVirtual.DelayedCall(1, () => {
                GameManager.Singleton.CanPlay = true;
            });
        }); 
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
