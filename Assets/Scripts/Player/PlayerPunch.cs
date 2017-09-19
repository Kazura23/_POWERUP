using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Com.LuisPedroFonseca.ProCamera2D;

public class PlayerPunch : MonoBehaviour
{

    private enum HitType
    {
        Attack1,
        Attack2,
        Attack3
    }

    private HitType HitState = HitType.Attack1;

    public float[] HitlvlDamage;
    public float[] HitlvlSpeedLow;
    public float[] HitMoveDistance;

    private float HitDamage;

    private Tween speedTw;

    //public int HitState = 1;
    public float TimeComboBreak;
    public float TimeBetweenEachHit;
    public float TimeComboCooldown;

    public static PlayerPunch Singleton;

    public LayerMask enemyMask;

    public RaycastHit2D hit;

    private float stockedSpeed;

    // Use this for initialization
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


        stockedSpeed = PlayerMovement.Singleton.SpeedIncrease;
    }

    public void Attack()
    {

        if (m_canHit == true)
        {

            //RaycastHit2D hit;

            if (HitState == HitType.Attack1)
            {
                //Debug.DrawLine()
                Hit1();
                StopCoroutine("ComboBreak");
                StartCoroutine("ComboBreak");
                StartCoroutine("CooldownBetweenHits");
                m_canHit = false;
            }
            else if (HitState == HitType.Attack2)
            {
                Hit2();
                StopCoroutine("ComboBreak");
                StartCoroutine("ComboBreak");
                StartCoroutine("CooldownBetweenHits");
                m_canHit = false;
            }
            else if (HitState == HitType.Attack3)
            {
                Hit3();
                StopCoroutine("ComboBreak");
                StartCoroutine("ComboBreak");
                StartCoroutine("ComboCooldown");
                m_canHit = false;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
    }

    void Hit1()
    {
        //speedTw.Kill();

        HitState = HitType.Attack2;
        HitDamage = HitlvlDamage[0];


        PlayerMovement.Singleton.transform.DOMoveX(PlayerMovement.Singleton.transform.position.x + HitMoveDistance[0] * Mathf.Sign(PlayerMovement.Singleton.transform.rotation.y) ,.1f);

        PlayerMovement.Singleton.SpeedIncrease /= HitlvlSpeedLow[0];
        speedTw = DOVirtual.DelayedCall(.5f, () => {
            PlayerMovement.Singleton.SpeedIncrease = stockedSpeed;
        });

        GetComponent<SpriteRenderer>().color = Color.green;
        Color color = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0.6f);

        hit = Physics2D.BoxCast(transform.position, Vector2.one, 90, Vector2.one, 10, enemyMask);
        

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {

                hit.transform.GetComponent<Enemy>().Life -= HitDamage;
                hit.transform.GetComponent<Enemy>().TakeHit();
            }
        }

        //Debug.Log(PlayerMovement.Singleton.SpeedIncrease);
    }

    void Hit2()
    {
        //speedTw.Kill();

        HitState = HitType.Attack3;
        HitDamage = HitlvlDamage[1];

        PlayerMovement.Singleton.transform.DOMoveX(PlayerMovement.Singleton.transform.position.x + HitMoveDistance[1] * Mathf.Sign(PlayerMovement.Singleton.transform.rotation.y) , .1f);

        PlayerMovement.Singleton.SpeedIncrease /= HitlvlSpeedLow[1];
        speedTw = DOVirtual.DelayedCall(.5f, () => {
            PlayerMovement.Singleton.SpeedIncrease = stockedSpeed;
        });

        GetComponent<SpriteRenderer>().color = Color.yellow;
        Color color = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0.6f);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {

                hit.transform.GetComponent<Enemy>().Life -= HitDamage;
                hit.transform.GetComponent<Enemy>().TakeHit();
            }
        }
    }

    void Hit3()
    {
        //speedTw.Kill();

        HitState = HitType.Attack1;
        HitDamage = HitlvlDamage[2];

        PlayerMovement.Singleton.transform.DOMoveX(PlayerMovement.Singleton.transform.position.x + HitMoveDistance[2] * Mathf.Sign(PlayerMovement.Singleton.transform.rotation.y), .1f);

        PlayerMovement.Singleton.SpeedIncrease /= HitlvlSpeedLow[2];
        speedTw = DOVirtual.DelayedCall(.5f, () => {
            PlayerMovement.Singleton.SpeedIncrease = stockedSpeed;
        });

        GetComponent<SpriteRenderer>().color = Color.red;
        Color color = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0.6f);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                hit.transform.GetComponent<Enemy>().Life -= HitDamage;
                hit.transform.GetComponent<Enemy>().TakeHit();
            }
        }
    }


    IEnumerator ComboBreak()
    {
        yield return new WaitForSeconds(TimeComboBreak);
        HitState = HitType.Attack1;
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
    }

    IEnumerator CooldownBetweenHits()
    {
        yield return new WaitForSeconds(TimeBetweenEachHit);
        m_canHit = true;
    }

    IEnumerator ComboCooldown()
    {
        yield return new WaitForSeconds(TimeComboCooldown);
        m_canHit = true;
    }

    bool m_canHit = true;
}
