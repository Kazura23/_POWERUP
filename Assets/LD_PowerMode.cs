using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using DG.Tweening;

public class LD_PowerMode : MonoBehaviour {

    public static LD_PowerMode Singleton;


    public GameObject powerMode;

    public GameObject[] rockObjects;

    private Transform rocks;

    public GameObject platformFirst;
    public Transform platformsContainer;
    public Transform platformGround;

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
        


        GameObject powerBg = powerMode.transform.GetChild(0).gameObject;
        powerBg.GetComponent<SpriteRenderer>().DOFade(0, 0);

        rocks = powerMode.transform.GetChild(5).GetComponent<Transform>();

        
        foreach (BoxCollider2D box in powerMode.GetComponentsInChildren<BoxCollider2D>())
        {
            box.enabled = false;
        }
        
        foreach (SpriteRenderer spritePlat in platformsContainer.GetComponentsInChildren<SpriteRenderer>())
        {
            spritePlat.DOFade(0, 0f);
        }
         

    }

    public void PowerModeStart()
    {
        
        foreach (BoxCollider2D box in powerMode.GetComponentsInChildren<BoxCollider2D>())
        {
            box.enabled = true;
        }

        GameObject powerBg = powerMode.transform.GetChild(0).gameObject;
        powerBg.GetComponent<SpriteRenderer>().DOFade(1, 3f);

        foreach (SpriteRenderer spritePlat in platformsContainer.GetComponentsInChildren<SpriteRenderer>())
        {
            spritePlat.DOFade(1, 4f);
        }

        MoveRock();

    }

    void MoveRock()
    {
        DOVirtual.DelayedCall(.6f, () => {

            var rock = Instantiate(rockObjects[UnityEngine.Random.Range(0, 4)], rocks.transform.localPosition, Quaternion.identity, rocks.transform);
            rock.transform.DOLocalMoveX(UnityEngine.Random.Range(-6.5f,6.5f), 0);
            rock.transform.DOLocalMoveY(GameManager.Singleton.player.transform.position.y + 15, 0);
            rock.transform.DOLocalMoveY(-5, 3).OnComplete(()=> {
                Destroy(rock);
            });

        }).SetLoops(-1,LoopType.Restart);

        /*foreach (Transform transrock in rocks)
        {
            transrock.DOLocalMoveY(100, 0);
            transrock.DOLocalMoveY(-8, 2).OnComplete(() =>
            {
                MoveRock();
            });
        }*/

    }

    // Update is called once per frame
    void Update () {
        /*
        var dist = (transform.position - Camera.main.transform.position).z;
        var downBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, -1, dist)).y;
        platformGround.DOMoveY(downBorder,0);*/
    }
}
