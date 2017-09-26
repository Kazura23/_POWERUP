using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine.UI;

public class LD_Orbs : MonoBehaviour {

    public int orbNumber;

    public bool canZoom;

    public GameObject powerMode;


    public Tween zoomTw;

    public Transform CameraTarget;

    [Header("ORB EFFECT")]
    public float fairyPosition;
    public float zoomTime = .2f;
    public float colorSwitchTime = .2f;
    public float PowerUpDuration = 5;

    [Header("CHARACTER PROPERTIES CHANGES")]
    public float damageMultiplier;
    public float SpeedMultiplier;
    //public float newJumpSpeed;
    public int newJumpNumberMax;

    [HideInInspector]
    public bool quitWindow;

	// Use this for initialization
	void Awake () {
        GetComponent<RainbowColor>().enabled = false;
    }

    void Start()
    {
        Destroy(GameObject.Find("PC2DPanTarget"));

        
    }
	
	// Update is called once per frame
	void Update () {
        //DEV COMMANDS

        if ((Input.GetKeyDown("a") && orbNumber == 1) || (Input.GetKeyDown("z") && orbNumber == 2) || (Input.GetKeyDown("e") && orbNumber == 3) || (Input.GetKeyDown("r") && orbNumber == 4))
        {
                PlayerMovement.Singleton.quitWindow = true;

                DOVirtual.DelayedCall(.1f, () => {

                    Vector2 tmpPos = transform.position;
                    GameManager.Singleton.player.transform.position = tmpPos;
                    DOVirtual.DelayedCall(.1f, () => {
                        PlayerMovement.Singleton.quitWindow = false;
                    });
                });
            
        }
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

    void Zoom()
    {
        if (canZoom)
            GameManager.Singleton.mainCamera.DOOrthoSize(3.5f - .2f, zoomTime);


            zoomTw = DOVirtual.DelayedCall(zoomTime, () => {
                if (canZoom) { 
                    GameManager.Singleton.mainCamera.DOOrthoSize(3.5f + .2f, zoomTime);
                zoomTw = DOVirtual.DelayedCall(zoomTime, () => {
                if(canZoom)
                Zoom();

                    });
                }
            });

    }
    void OnTakenOrb(int orb)
    {
        zoomTw.Kill(false);
        //ProCamera2D.Instance.Zoom(-2.2f, .5f);
        GameManager.Singleton.mainCamera.DOOrthoSize(3.5f, .5f).SetEase(Ease.Linear);

        ProCamera2D.Instance.RemoveCameraTarget(CameraTarget, 0);

        GameManager.Singleton.CanPlay = false;
        //ControllerPlayer.Singleton.player.controllers.maps.SetMapsEnabled(false, 0);
        PlayerMovement.Singleton.body.constraints = RigidbodyConstraints2D.FreezeAll;
        //DOTween.PauseAll();
        UIManager.Singleton.powerUpScreen.GetComponent<RainbowColor>().enabled = true;
        UIManager.Singleton.powerUpScreen.GetComponent<RainbowColor>().time = colorSwitchTime;
        DOVirtual.DelayedCall(.5f, () => {
            Debug.Log("Zoom");
            canZoom = true;
            Zoom();
        });

        float stockedWalkSpeed = PlayerMovement.Singleton.SpeedMultiplier;
        int stockedJumpMax = PlayerMovement.Singleton.JumpMax;

        //PLAYER GETS NEW PROPERTIES

        PlayerMovement.Singleton.SpeedMultiplier = SpeedMultiplier;
        PlayerMovement.Singleton.JumpMax = newJumpNumberMax;
        PlayerPunch.Singleton.HitlvlDamage[0] *= damageMultiplier;
        PlayerPunch.Singleton.HitlvlDamage[1] *= damageMultiplier;
        PlayerPunch.Singleton.HitlvlDamage[2] *= damageMultiplier;


        PlayerFairy.Singleton.transform.DOLocalMoveX(fairyPosition, 2);



        DOVirtual.DelayedCall(3f, () => {

            canZoom = false;
            UIManager.Singleton.powerUpScreen.GetComponent<RainbowColor>().enabled = false;
            UIManager.Singleton.powerUpScreen.DOFade(0, colorSwitchTime * 5f);
            
            GameManager.Singleton.mainCamera.DOOrthoSize(5.76f, .5f).SetEase(Ease.InElastic).OnComplete(()=> {
                //ControllerPlayer.Singleton.player.controllers.maps.SetMapsEnabled(true, 0);
                GameManager.Singleton.CanPlay = true;
                PlayerMovement.Singleton.body.constraints = RigidbodyConstraints2D.FreezeRotation;
                DOTween.PlayAll();

                UIManager.Singleton.QuoteCharacterStart();
                UIManager.Singleton.CharacterQuote.GetComponent<Text>().text = "BADASS!!";

                DOVirtual.DelayedCall(PowerUpDuration, () =>
                {

                    //PLAYER GETS OLD PROPERTIES

                    PlayerMovement.Singleton.SpeedMultiplier = stockedWalkSpeed;
                    PlayerMovement.Singleton.JumpMax = stockedJumpMax;
                    PlayerPunch.Singleton.HitlvlDamage[0] /= damageMultiplier;
                    PlayerPunch.Singleton.HitlvlDamage[1] /= damageMultiplier;
                    PlayerPunch.Singleton.HitlvlDamage[2] /= damageMultiplier;
                    });
                });
            

        });


        if(orb == 1)
        {
            PlayerMovement.Singleton.animator.SetTrigger("Transfo_NormalToStrong");

        }

        if(orb == 3)
        {

            //PLAYER PROPERTIES
            PlayerMovement.Singleton.JumpMax = 3;
            //APlayerMovement.Singleton.SpeedIncrease =



            GameObject powerBg = powerMode.transform.GetChild(0).gameObject;
            powerBg.GetComponent<SpriteRenderer>().DOFade(1, 3f);



            LD_PowerMode.Singleton.PowerModeStart();

            DOVirtual.DelayedCall(3.05f, () =>
            {
                ProCamera2D.Instance.RemoveAllCameraTargets();
                GameManager.Singleton.mainCamera.DOOrthoSize(7, 1f).SetEase(Ease.InBounce);
                ProCamera2D.Instance.AddCameraTarget(powerMode.transform.GetChild(0).transform);
                ProCamera2D.Instance.AddCameraTarget(GameManager.Singleton.player.transform);
                ProCamera2D.Instance.AdjustCameraTargetInfluence(powerMode.transform.GetChild(0).transform, 1, 0);
                ProCamera2D.Instance.AdjustCameraTargetInfluence(GameManager.Singleton.player.transform, 0, 1);
                ProCamera2D.Instance.FollowVertical = true;

                powerBg.transform.DOLocalMoveY(159.8f, 2.5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    powerBg.transform.DOLocalMoveY(132.26f, 0).SetEase(Ease.Linear);
                }).SetLoops(-1, LoopType.Restart);

                
            });


        }

        //etc...
    }
}
