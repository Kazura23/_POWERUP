using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Com.LuisPedroFonseca.ProCamera2D;

public class Enemy : MonoBehaviour
{


    public float Life = 100;

    public enum EnemyType
    {
        Sbire,
        Fly,
        Normal,
        Shooter
    }


    public enum SearchType
    {
        SearchLeft,
        SearchRight
    }

    public EnemyType enemyType;
    public SearchType searchType;
    public float Distance = 5;
    public float TimeDistance = 2;
    public Ease easeMove = Ease.Linear;
    public float SpeedRush = 1;
    public float MultiplierBack = 1;

    public float CheckPlayerDistanceX;
    public float CheckPlayerDistanceY;

    [Header("ATTACKS GENERAL")]
    public float AttackChargeTime;
    public float AttackDistance;
    public float AttackCooldownReload;
    public float PushbackDistance, PushbackDuration;
    public float PushbackCharacter = 3;

    [Header("ENEMY FLY")]
    public float FlyAttackTime;
    public float FlyBackTime;

    [Header("ENEMY SHOOT")]
    public GameObject bulletObject;
    public float bulletDuration = 4;

    public LayerMask layer;
    private bool Searching = true, AttackCharge;

    private Rigidbody2D body;
    private bool attacking;
    private Tween moveTw;
    private Tween rushTw;
    public Tween attackTw;
    private Tween pushbackTw;
    private Tween scaleTw;

    private Animator animator;

    private float Side;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();


        animator = GetComponent<Animator>();

        if(enemyType == EnemyType.Sbire || enemyType == EnemyType.Fly || enemyType == EnemyType.Normal)
        {

            if (searchType == SearchType.SearchLeft)
                SearchLeft();
            if (searchType == SearchType.SearchRight)
                SearchRight();
        }
        
    }

    // Use this for initialization
    void Start()
    {
    }

    void SearchRight()
    {
        if (Searching)
        {
            animator.SetBool("Running", true);

            transform.DOScaleX(1f, 0f);
            moveTw = transform.DOMoveX(transform.position.x + Distance, TimeDistance).SetEase(easeMove).OnComplete(() =>
            {
                transform.DOScaleX(-1f, 0f);
                moveTw = transform.DOMoveX(transform.position.x - Distance, TimeDistance).SetEase(easeMove).OnComplete(() =>
                {

                    SearchRight();
                });
            });
        }

    }

    void SearchLeft()
    {
        if (Searching)
        {
            animator.SetBool("Running", true);

            transform.DOScaleX(-1, 0f);
            moveTw = transform.DOMoveX(transform.position.x - Distance, TimeDistance).SetEase(easeMove).OnComplete(() =>
            {
                transform.DOScaleX(1f, 0f);
                moveTw = transform.DOMoveX(transform.position.x + Distance, TimeDistance).SetEase(easeMove).OnComplete(() =>
                {

                    SearchLeft();
                });
            });
        }

    }

    void Rush()
    {
        if (Searching)
        {
            animator.SetBool("Running", true);

            AttackCharge = false;

            if (PlayerMovement.Singleton.transform.position.x < transform.position.x)
            {
                transform.DOScaleX(-1f, 0f);
            }
            else
            {
                transform.DOScaleX(1f, 0f);
            }
            Debug.Log("Rush");
            rushTw = transform.DOMoveX(PlayerMovement.Singleton.transform.position.x, 1 / SpeedRush).OnComplete(() =>
            {
                if (Searching)
                {
                    Rush();
                    Debug.Log("Rush");
                }
            });

        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Mathf.DeltaAngle(transform.eulerAngles.y, PlayerMovement.Singleton.transform.eulerAngles.y));
        //Debug.Log(Mathf.Abs(transform.position.x - PlayerMovement.Singleton.transform.position.x));

        //Debug.Log("Searching +" + Searching);
        //Debug.Log("Attack Charge +" + AttackCharge);
        if(enemyType == EnemyType.Normal)
            rushTw.Kill();

        if (Life <= 0)
        {
            if (LevelEvents.Singleton != null)
                LevelEvents.Singleton.zoneNumberEnemies += 1;

            string ShakeName = "ShakeKill";
            ProCamera2DShake.Instance.Shake(ShakeName);

            Destroy(transform.gameObject);
        }


        if (PlayerMovement.Singleton.transform.position.x > transform.position.x)
        {
            Side = 1f;
        }
        else if (PlayerMovement.Singleton.transform.position.x < transform.position.x)
        {
            Side = -1f;
        }
        /*
        if(!Searching)
            transform.DOScaleX(transform.localScale.x * Side, 0f);*/

        //Debug.Log("Side + " + Side);

        if((Mathf.Abs(transform.position.x - PlayerMovement.Singleton.transform.position.x) < CheckPlayerDistanceX)  && (Mathf.Abs(transform.position.y - PlayerMovement.Singleton.transform.position.y) < CheckPlayerDistanceY) && enemyType == EnemyType.Shooter){

            if (!AttackCharge && !PlayerMovement.Singleton.IsStuned)
            {
                AttackCharge = true;

                animator.SetTrigger("Attack");
                if (Side == 1)
                    transform.DOScaleX(1f, 0f);
                else
                    transform.DOScaleX(-1f, 0f);

                DOVirtual.DelayedCall(.5f, () =>
                {

                    var bullet = Instantiate(bulletObject, transform.GetChild(1).position, Quaternion.identity, transform.GetChild(1));
                    bullet.transform.localScale = Vector3.one;
                    bullet.transform.localPosition = Vector3.zero;

                    bullet.transform.DOMove(PlayerMovement.Singleton.GetComponentsInChildren<SpriteRenderer>()[0].transform.position, 1/bulletDuration).SetEase(Ease.Linear).OnComplete(()=> {
                        Destroy(bullet.gameObject);
                    });

                    AttackCharge = false;

                });
            }
        }
        


        if ((Mathf.Abs(transform.position.x - PlayerMovement.Singleton.transform.position.x) < CheckPlayerDistanceX/5 && enemyType == EnemyType.Normal) || (Mathf.Abs(transform.position.x - PlayerMovement.Singleton.transform.position.x) < CheckPlayerDistanceX) && enemyType == EnemyType.Fly)//(((Physics2D.Raycast(body.position, new Vector2(1 * Mathf.Sign(transform.localScale.x), -1), CheckPlayerDistance, layer) || (Physics2D.Raycast(body.position, new Vector2(0, -1), CheckPlayerDistance, layer))) && enemyType == EnemyType.Fly)))
        {
            if (!AttackCharge && Searching && !PlayerMovement.Singleton.IsStuned)
            {

                Searching = false;
                AttackCharge = true;
                Debug.Log("ATTACK");
                //transform.DOPunchScale(Vector3.one * .05f, AttackChargeTime, 10, 1);
                rushTw.Kill();
                moveTw.Kill();
                
                body.constraints = RigidbodyConstraints2D.FreezeAll;
                float tmpSide = Side;


                Vector2 playerPos = PlayerMovement.Singleton.transform.position;


                animator.SetTrigger("Attack");

                attackTw = DOVirtual.DelayedCall(AttackChargeTime, () =>
                {
                    //Attack();

                    attacking = true;

                    body.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;

                    if (enemyType == EnemyType.Normal)
                    {

                        attackTw = transform.DOMoveX(transform.position.x + AttackDistance * Mathf.Sign(tmpSide), .5f).OnComplete(() =>
                        {
                            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                            body.constraints = RigidbodyConstraints2D.FreezeAll;
                            attacking = false;

                            

                            animator.SetBool("Running", false);
                            //transform.GetChild(1).GetComponent<SpriteRenderer>().DOFade(0, 0);
                            attackTw = DOVirtual.DelayedCall(AttackCooldownReload, () =>
                            {

                                body.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
                                AttackCharge = false;
                                Searching = true;
                                SearchRight();
                            });
                        });
                    }
                    if (enemyType == EnemyType.Fly)
                    {

                        Vector2 stockedPos = transform.position;
                        attackTw = transform.DOMove(new Vector2(playerPos.x, playerPos.y - 3.5f), FlyAttackTime).OnComplete(() =>
                        {
                            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                            body.constraints = RigidbodyConstraints2D.FreezeAll;
                            attacking = false;
                            //transform.GetChild(1).GetComponent<SpriteRenderer>().DOFade(0, 0);
                            attackTw = transform.DOMove(new Vector2(stockedPos.x, stockedPos.y), FlyBackTime).OnComplete(() =>
                            {

                                body.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
                                AttackCharge = false;
                                Searching = true;
                                SearchRight();
                            });
                        });
                    }

                });

                
            }
        }

        if (attacking)
        {

            RaycastHit2D hit;

            if (enemyType == EnemyType.Normal)
            {

                hit = Physics2D.BoxCast(transform.position, Vector2.one * 1.4f, 90, Vector2.one, 1, layer);

                


                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        //Debug.Log("Player is STUN");
                        //hit.transform.DOLocalMoveX(PushbackCharacter * Mathf.Sign(Side), .0f).SetEase(Ease.InBounce);
                        hit.transform.GetComponent<PlayerMovement>().Stun();
                    }
                }
            }

            if (enemyType == EnemyType.Fly)
            {
                hit = Physics2D.BoxCast(transform.position, Vector2.one, 90, Vector2.down, 1, layer);

                if (hit.collider != null && !PlayerMovement.Singleton.IsStuned)
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        hit.transform.GetComponent<PlayerMovement>().Stun();
                        Debug.Log("Player is STUN");
                        //hit.transform.DOLocalMoveX(PushbackCharacter * Mathf.Sign(Side), .17f).SetEase(Ease.InBounce);
                    }
                }
            }
        }


        //if ((Physics2D.Raycast(body.position, new Vector2(1 * Mathf.Sign(transform.localScale.x), 0), CheckPlayerDistanceX, layer) || Physics2D.Raycast(body.position, new Vector2(1 * -Mathf.Sign(transform.localScale.x), 0), CheckPlayerDistanceX, layer)))
        if(Mathf.Abs(transform.position.x - PlayerMovement.Singleton.transform.position.x) < CheckPlayerDistanceX && !PlayerMovement.Singleton.IsStuned)
        {

            //Debug.Log("PlayerFound1");
            if (Searching && (enemyType == EnemyType.Normal || enemyType == EnemyType.Sbire))
            {
                Debug.Log("PlayerFound");
                moveTw.Kill();
                Rush();
            }

            
        }

       // Debug.DrawRay(body.position, new Vector2(Mathf.Sign(transform.localScale.x) * CheckPlayerDistanceX, 0), Color.green, .3f);
       // Debug.DrawRay(body.position, new Vector2(1 * -Mathf.Sign(transform.localScale.x) * CheckPlayerDistanceX / MultiplierBack, 0), Color.blue, .3f);
       // Debug.DrawRay(body.position, new Vector2(1 * -Mathf.Sign(transform.localScale.x) * CheckPlayerDistanceX, -6.5f), Color.green, .3f);
    }
    /*
    public void Attack()
    {
        RaycastHit2D hit;
        if(enemyType == EnemyType.Normal || enemyType == EnemyType.Big)
        {

            hit = Physics2D.BoxCast(transform.position, Vector2.one * 2, 90, Vector2.one, 1, layer);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    Debug.Log("Player is STUN");
                    hit.transform.GetComponent<PlayerMovement>().Stun();
                }
            }
        }

        if (enemyType == EnemyType.Fly)
        {

            hit = Physics2D.BoxCast(transform.position, Vector2.one * 2, 90, Vector2.down, 1, layer);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    Debug.Log("Player is STUN");
                    hit.transform.GetComponent<PlayerMovement>().Stun();
                }
            }
        }

        //Debug.Log(hit.transform.localScale);
        //transform.GetChild(1).DOScale(hit.transform.localScale,0);

        //Debug.Log(hit.transform.position);

       
    }*/

    public void TakeHit()
    {
        scaleTw.Kill(true);

        animator.SetBool("Running", false);

        scaleTw = transform.DOShakeScale(.2f, 3, 30, 90);

        if (PlayerMovement.Singleton.animator.GetBool("Mode_Normal"))
        {
            string ShakeName = "ShakeHit_Normal";
            ProCamera2DShake.Instance.Shake(ShakeName);
        }

        if (PlayerMovement.Singleton.animator.GetBool("Mode_Super"))
        {
            string ShakeName = "ShakeHit_Super";
            ProCamera2DShake.Instance.Shake(ShakeName);
        }
        
        
        pushbackTw.Kill();
        //Debug.Log("Pushback");
        moveTw.Kill();
        rushTw.Kill();
        rushTw = null;
        moveTw = null;
        attackTw.Kill();

        Searching = false;
        
        body.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        transform.DOLocalMoveX(body.transform.localPosition.x - PushbackDistance * Side, PushbackDuration).OnComplete(() =>
        {

            Searching = true;
            if (Side == 1)
                SearchRight();
            if (Side == -1)
                SearchLeft();
            //Rush();
        });

        //transform.DOPunchScale(Vector3.one * 1.4f, .2f, 15, 1);

        if (Life > 0)
        {
            GetComponentInChildren<SpriteRenderer>().color = new Color32(0xca, 0x23, 0x23, 0xff);
            ControllerPlayer.Singleton.player.controllers.maps.SetMapsEnabled(false, 0);
            //Time.timeScale = 0;
            DOTween.PauseAll();

            DOVirtual.DelayedCall(.1f, () => {

                ControllerPlayer.Singleton.player.controllers.maps.SetMapsEnabled(true, 0);
                GetComponentInChildren<SpriteRenderer>().color = Color.white;
                DOTween.PlayAll();
                //Time.timeScale = 1;
            });
        }
        
    }

    /*

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.collider.tag == "Wall")
        {
            moveTw.Kill();
            SearchLeft();
            //Debug.Log("Kill");
        }
    }*/
}
