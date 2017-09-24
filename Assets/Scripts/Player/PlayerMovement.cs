using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Com.LuisPedroFonseca.ProCamera2D;

public class PlayerMovement : MonoBehaviour {


    [Header("SPEED")]
    public float SpeedIncrease;
    public float MaxSpeed;
    [Header("JUMP")]
    public int JumpMax;
    public float JumpHeight;
    public float JumpCooldown;
    public float JumpIncreaseSpeed = 12;
    private int JumpNumber;
    [Tooltip("Damages the enemies receive when Jumping on them")]
    public float JumpDamage;

    [Header("STUN")]
    public float StunDuration;

    [HideInInspector]
    public bool IsFlipped, IsStuned, quitWindow;
    private bool Grounded;
    [HideInInspector]
    public float Side;

    public static PlayerMovement Singleton;

    public Tween jumpTw;

    public LayerMask groundMask;

    [HideInInspector]
    public Rigidbody2D body;

    public Animator animator;

    private bool canJump = true, started;
    
    private float stockedSpeed;

    public void Reset()
    {
        jumpTw.Kill();
    }

	// Use this for initialization
	void Awake () {
        body = GetComponent<Rigidbody2D>();

        if(Singleton == null)
        {
            Singleton = this;
        }

        animator = GetComponent<Animator>();

        stockedSpeed = SpeedIncrease;
    }
	
    void Start()
    {
        DOVirtual.DelayedCall(.1f, () => {
            started = true;
        });
    }
    

    public void MoveRight()
    {
        IsFlipped = false;
        body.velocity = new Vector3(1f * SpeedIncrease, body.velocity.y);
        Side = 1f;
    }

    public void MoveLeft()
    {
        IsFlipped = true;
        body.velocity = new Vector3(-1f * SpeedIncrease, body.velocity.y);
        Side = -1f;
    }

    public void Stun()
    {
        body.constraints = RigidbodyConstraints2D.FreezePositionX;
        ControllerPlayer.Singleton.player.controllers.maps.SetMapsEnabled(false, 0);
        IsStuned = true;
        transform.GetComponent<BoxCollider2D>().enabled = false;
        transform.DOMoveY(0,.1f).OnComplete(() => {
            body.constraints = RigidbodyConstraints2D.FreezeAll;
        });
        
        DOVirtual.DelayedCall(StunDuration, () =>
        {
            IsStuned = false;
            transform.GetComponent<BoxCollider2D>().enabled = true;
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            ControllerPlayer.Singleton.player.controllers.maps.SetMapsEnabled(true, 0);
        });
    }

    public void Jump()
    {
        if ((JumpNumber < JumpMax && JumpNumber == 0 ) || (JumpNumber == 1  && JumpNumber < JumpMax))
        {

            
            canJump = false;
            SpeedIncrease = JumpIncreaseSpeed;
            Vector2 veloce = body.velocity;
            veloce.y = 0;
            body.velocity = veloce;
            JumpNumber += 1;
            //jumpTw = body.DOJump(new Vector2(body.transform.position.x + (5 * Mathf.Sign(Side)), transform.localPosition.y), 4, 0, .4f);
            body.AddForce(new Vector3(body.velocity.x, 10 * JumpHeight), ForceMode2D.Impulse);
        }
    }

    // Update is called once per frame
    void Update () {

        Debug.Log("canjump + " + canJump);

        if (body.velocity.x > MaxSpeed)
        {
            body.velocity = new Vector3(MaxSpeed, body.velocity.y);
            animator.SetBool("Moving", true);
        }

        if (body.velocity.x < -MaxSpeed)
        {
            body.velocity = new Vector3(-MaxSpeed, body.velocity.y);
            animator.SetBool("Moving", true);
        }
        
        if(body.velocity.x == 0)
        {
            animator.SetBool("Moving", false);
        }

        if (Physics2D.Raycast(body.position, Vector2.down, 4f, groundMask))
        {
            Grounded = true;
            JumpNumber = 0;
        }
        else
        {
            Grounded = false;
            //Debug.Log(Grounded);
        }

        if (ControllerPlayer.Singleton.player.GetButtonDown("Jump"))
        {
            Jump();
        }

        //Debug.Log(SpeedIncrease);

        //Pour ne pas sortir de l'écran

        var dist = (transform.position - Camera.main.transform.position).z;
        var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
        var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;

        if (!quitWindow)
        {
            Vector3 tmpPos = transform.position;
            tmpPos.x = Mathf.Clamp(transform.position.x, leftBorder, rightBorder);
            transform.position = tmpPos;
        }


        // Debug.Log("JumpMax " + JumpMax);
        // Debug.Log("JumpNumber " + JumpNumber);
    }


   

    void OnCollisionEnter2D(Collision2D col)
    {
        if (started)
        {
            if (col.transform.tag == "Ground")
            {
                /*
                DOVirtual.DelayedCall(JumpCooldown, () => {
                    canJump = true;
                });*/

                string ShakeName = "ShakeJump";
                ProCamera2DShake.Instance.Shake(ShakeName);

                Debug.Log(stockedSpeed);

                //float tmpSpeed;
                SpeedIncrease = DOVirtual.EasedValue(stockedSpeed, stockedSpeed / 12, .25f, Ease.Linear);
                DOVirtual.DelayedCall(.4f, () => {
                    SpeedIncrease = DOVirtual.EasedValue(stockedSpeed, stockedSpeed, .15f, Ease.Linear);
                });
            }
        }
    }
}
