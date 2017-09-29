using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using DG.Tweening;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine.UI;

public class LD_PowerMode : MonoBehaviour {

    public static LD_PowerMode Singleton;


    public GameObject powerMode;

    public GameObject[] rockObjects;

    private Transform rocks;

    public GameObject platformFirst;
    public Transform platformsContainer;
    public Transform platformGround;
    public Transform rocksContainer;
    public BoxCollider2D endPowerMode;

    private Tween rocksTw;

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

    public void PowerModeEnd()
    {
        GameManager.Singleton.mainCamera.DOOrthoSize(3.5f, 1.5f);
        ProCamera2D.Instance.FollowHorizontal = true;
        ProCamera2D.Instance.RemoveCameraTarget(powerMode.transform.GetChild(0).transform);
        ProCamera2D.Instance.AdjustCameraTargetInfluence(GameManager.Singleton.player.transform, 1, 1);
        ProCamera2D.Instance.OffsetY = -.6f;
        GameManager.Singleton.CanPlay = false;
        PlayerMovement.Singleton.GetComponentInChildren<RainbowRotate>().enabled = true;
        rocksTw.Kill();

        foreach(Transform rock in rocksContainer)
        {
            Destroy(rock.gameObject);
        }

        DOVirtual.DelayedCall(3f, () =>
        {
            GameManager.Singleton.mainCamera.DOOrthoSize(5.76f, .9f).SetEase(Ease.InElastic);
            ProCamera2D.Instance.OffsetY = 0;
            DOVirtual.DelayedCall(.2f, () => {
                GameManager.Singleton.CanPlay = true;
                ProCamera2D.Instance.FollowVertical = false;
            });
            PlayerMovement.Singleton.GetComponentInChildren<RainbowRotate>().enabled = false;
            PlayerMovement.Singleton.GetComponentsInChildren<SpriteRenderer>()[0].transform.DOLocalRotate(Vector3.zero, 0);

            UIManager.Singleton.quoteText = "...";
            UIManager.Singleton.QuoteCharacterStart(4);

        });

        foreach (BoxCollider2D box in powerMode.GetComponentsInChildren<BoxCollider2D>())
        {
            box.enabled = false;
        }

        GameObject powerBg = powerMode.transform.GetChild(0).gameObject;
        powerBg.GetComponent<SpriteRenderer>().DOFade(0, 3f);

        foreach (SpriteRenderer spritePlat in platformsContainer.GetComponentsInChildren<SpriteRenderer>())
        {
            spritePlat.DOFade(0, 2f);
        }


    }



    void MoveRock()
    {
        rocksTw = DOVirtual.DelayedCall(.6f, () => {

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
