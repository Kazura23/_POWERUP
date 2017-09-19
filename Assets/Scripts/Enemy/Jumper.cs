using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

public class Jumper : MonoBehaviour
{
    public string RepelTag;
    public float Force = 30;
    public BoxCollider2D reference;

    private bool cooldown = false;

    void OnCollisionEnter2D(Collision2D col)
    {
        Vector2 diff = PlayerMovement.Singleton.transform.position - transform.position;
        //Debug.Log(diff.y);
        if (Mathf.Abs(diff.y) > 3f)
        {
            
            GameObject other = col.gameObject;
            if (other.CompareTag(RepelTag) && reference.isActiveAndEnabled && !cooldown)
            {
                other.GetComponent<Rigidbody2D>().velocity = (Vector2.up * Force + new Vector2(PlayerMovement.Singleton.Side, 0) * Force * 2);
                transform.parent.GetComponent<Enemy>().Life -= PlayerMovement.Singleton.JumpDamage;

                string ShakeName = "ShakeHit";
                ProCamera2DShake.Instance.Shake(ShakeName);

                StopCoroutine(StopCooldown());
                StartCoroutine(StopCooldown());
            }
        }

        
    }
    
    void OnCollisionStay2D(Collision2D col)
    {

        Vector2 diff = PlayerMovement.Singleton.transform.position - transform.position;

        if (Mathf.Abs(diff.y) > 3f)
        {

            GameObject other = col.gameObject;
            if (other.CompareTag(RepelTag) && reference.isActiveAndEnabled && !cooldown)
            {
                other.GetComponent<Rigidbody2D>().velocity = (Vector2.up * Force + new Vector2(PlayerMovement.Singleton.Side, 0) * Force * 2);
                transform.parent.GetComponent<Enemy>().Life -= PlayerMovement.Singleton.JumpDamage;

                string ShakeName = "ShakeHit";
                ProCamera2DShake.Instance.Shake(ShakeName);

                StopCoroutine(StopCooldown());
                StartCoroutine(StopCooldown());
            }
        }
    }

    void Update()
    {
        //Vector2 diff = PlayerMovement.Singleton.transform.position - transform.position;
        //Debug.Log(Mathf.Abs(diff.y));

        GetComponent<BoxCollider2D>().enabled = reference.isActiveAndEnabled;
    }

    IEnumerator StopCooldown()
    {
        cooldown = true;
        yield return new WaitForSeconds(.3f);
        cooldown = false;
    }
}