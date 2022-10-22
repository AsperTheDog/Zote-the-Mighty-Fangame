using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ZoteflyController : MovementController
{
    public string[] rightAnims = new string[] {"Flying", "Idle", "Turn"};
    public float flySpeed = 5f;
    public float flyFalloff = -1f;
    public Vector2 spitVelocity;
    [SerializeField] bool landedAndNotMoving;
    [SerializeField] GameObject spitProjectile;

    protected override void animatePlayer()
    {
        string stateName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        spriteRenderer.flipX = !rightAnims.Contains(stateName) ? lookDirection < 0 : lookDirection > 0;
    }

    protected override void movePlayer()
    {
        hInput = Input.GetAxisRaw("Horizontal");

        float hFactor = 0;
        if (hInput != 0)
        {
            hFactor = hInput > 0 ? 1 : -1;
            lookDirection = hFactor;
            actionDirection = lookDirection;
        }     
        var yInput = Input.GetAxisRaw("Vertical");
        float yFactor = yInput == 0 ? 0: yInput > 0 ? 1 : -1;

        Vector2 move = new Vector2(runSpeed * hFactor, flySpeed * yFactor + flyFalloff);
        
        landedAndNotMoving = grounded && move[0] == 0;
        fsm.FsmVariables.GetVariable("landedAndNotMoving").RawValue = landedAndNotMoving;
        updDirection();
        rb.velocity = move;
    }

    public void AttackSpit()
    {
        var go = Instantiate(spitProjectile, gameObject.transform.position, Quaternion.identity);
        var additionalSpeedX = rb.velocity.x != 0 ? new Vector2(spitVelocity[0] * lookDirection, 0) : Vector2.zero;
        var additionalSpeedY = rb.velocity.y > 0  ? new Vector2(0, spitVelocity[1]) : Vector2.zero;
        go.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity + additionalSpeedX + additionalSpeedY;
    }
}
