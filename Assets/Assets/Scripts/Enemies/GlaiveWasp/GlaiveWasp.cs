using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GlaiveWasp : MonoBehaviour
{
    public PlayMakerFSM fsm;

    private GameObject player;

    public Vector3 playerPosition;
    private Vector3 waspPosition;

    public bool facingRight = false;
    public float angle;

    private bool groundCollided = false;

    private LayerMask mask;


    private void Start()
    {
        player = GameObject.Find("PlayerFly");
        playerPosition = player.transform.position;
        waspPosition = transform.position;

        if(transform.localScale.x == -1)
        {
            facingRight = true;
        }

        fsm = gameObject.GetComponentInParent<PlayMakerFSM>();

        mask = LayerMask.GetMask("Ground");

        float patroll = NextFloat(2, 4);
        fsm.FsmVariables.GetVariable("patrollDistance").RawValue = patroll;
    }

    private void Update()
    {
        waspPosition = transform.position;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            return;
        }
        getPlayerPosition();
        RaycastHit2D hit = Physics2D.CircleCast(waspPosition, 0.6f, playerPosition - waspPosition, Vector3.Distance(waspPosition, playerPosition), mask);
        if (hit.collider == null)
        {
            fsm.FsmVariables.GetVariable("insideTrigger").RawValue = true;
        }
        else
        {
            fsm.FsmVariables.GetVariable("insideTrigger").RawValue = false;
        }
    }

    public void TriggerEnter2D()
    {
        getPlayerPosition();
        RaycastHit2D hit = Physics2D.CircleCast(waspPosition, 0.6f, playerPosition - waspPosition, Vector3.Distance(waspPosition, playerPosition), mask);
        if (hit.collider == null)
        {
            fsm.FsmVariables.GetVariable("insideTrigger").RawValue = true;
        }
    }

    public void TriggerExit2D()
    {
        fsm.FsmVariables.GetVariable("insideTrigger").RawValue = false;
    }

    public void getDirection()
    {
        getPlayerPosition();
        if (playerPosition.x > waspPosition.x && facingRight == false || playerPosition.x < waspPosition.x && facingRight)
        {
            fsm.FsmVariables.GetVariable("turn").RawValue = true;
        }
        else
        {
            fsm.FsmVariables.GetVariable("turn").RawValue = false;
        }
    }

    public void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Rotate()
    {
        if (facingRight)
        {
            angle = Vector3.SignedAngle(Vector3.right, playerPosition - waspPosition, transform.forward);
        }
        else
        {
            angle = Vector3.SignedAngle(Vector3.left, playerPosition - waspPosition, transform.forward);
        }
        gameObject.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    public void getVector()
    {
        if (!facingRight)
        {
            angle += 180;
        }
        float radian = (angle) * Mathf.Deg2Rad;
        Vector2 res = new Vector2(15*(Mathf.Cos(radian)), 15*(Mathf.Sin(radian)));
        fsm.FsmVariables.GetVariable("attackVector").RawValue = res;
    }

    public void checkAttackEnd()
    {
        if (transform.position == playerPosition || groundCollided)
        {
            groundCollided = false;
            Rest();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (fsm.ActiveStateName == "Attack")
            {
                groundCollided = true;
            }
        }
        else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            GetHit(collision);
        }
    }

    public void Rest()
    {
        fsm.FsmVariables.GetVariable("rest").RawValue = true;
    }

    public void getPlayerPosition()
    {
        playerPosition = player.transform.position;
    }

    public void Patroll()
    {
        if (facingRight)
        {
            fsm.FsmVariables.GetVariable("patrollVector").RawValue = new Vector2(1.5f, 0);
        }
        else
        {
            fsm.FsmVariables.GetVariable("patrollVector").RawValue = new Vector2(-1.5f, 0);
        }
    }

    public void PatrollTurn()
    {
        fsm.FsmVariables.GetVariable("patrollTurn").RawValue = true;
    }

    public void Reset()
    {
        if (facingRight)
        {
            Vector3 theScale = transform.localScale;
            theScale = new Vector3(-1, 1, 1);
            transform.localScale = theScale;
        }
        else
        {
            Vector3 theScale = transform.localScale;
            theScale = new Vector3(1, 1, 1);
            transform.localScale = theScale;
        }

        Quaternion theRotation = transform.localRotation;
        theRotation = new Quaternion(0, 0, 0, 0);
        transform.localRotation = theRotation;
    }

    public void GetHit(Collision2D collision)
    {
        if (collision.GetContact(0).point.x > waspPosition.x)
        {
            fsm.FsmVariables.GetVariable("deathVector").RawValue = new Vector2(-1f, 10f);
        }
        else
        {
            fsm.FsmVariables.GetVariable("deathVector").RawValue = new Vector2(1f, 10f);
        }
        fsm.SendEvent("Death");
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
    }

    static float NextFloat(float min, float max)
    {
        System.Random random = new System.Random();
        double val = (random.NextDouble() * (max - min) + min);
        return (float)val;
    }
}
