using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LD_EndFade : MonoBehaviour {

    public GameObject FadeScreen;
    public float FadeDuration = 10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.transform.tag == "Player")
        {
            FadeScreen.transform.SetParent(FadeScreen.transform.parent.transform.parent);
            FadeScreen.GetComponent<Image>().DOColor(new Color(0, 0, 0), 0);
            FadeScreen.GetComponent<Image>().DOFade(0, 0);
            FadeScreen.GetComponent<Image>().DOFade(1, FadeDuration).OnComplete(()=> {
                DOVirtual.DelayedCall(3, () => {
                    SceneManager.LoadScene("Game", LoadSceneMode.Single);
                });
            });
            
            Destroy(this.gameObject);
        }
    }
}
