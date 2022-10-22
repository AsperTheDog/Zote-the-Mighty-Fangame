using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ZoteController : MovementController
{
    [Header("Movement Parameteres")]
    [SerializeField] protected bool wallSliding = false;

    [Header("Colliders")]
    [SerializeField] Collider2D hCollider;
    [SerializeField] Collider2D vCollider;
    [SerializeField] protected Collider2D wallColliderL;
    [SerializeField] protected Collider2D wallColliderR;

    [Header("Cooldowns")]
    [SerializeField] protected float diveCooldown = 0.95f;
    [SerializeField] private float diveTimer;

    [Header("Animation Parameters")]
    [SerializeField] protected float wallSlideLookDirection = 1;

    public string[] rightAnims = new string[] {"Run", "Fall", "Dive", "WallSlide", "Turn"};


    public override void Update()
    {
        base.Update();
        if (diveTimer > 0)
        {
            diveTimer -= Time.deltaTime; 
        }
        if (Input.GetButtonDown("Dive") && diveTimer <= 0)
        {
            fsm.SendEvent("DiveEventGlobal");
        }
    }

    protected override void UpdatePreMove()
    {
        detectIfHuggingWall();
    }

    protected void setDiveCooldown()
    {
        diveTimer = diveCooldown;
    }

    protected void setHorizontalCollider()
    {
        hCollider.enabled = true;
        vCollider.enabled = false;
    }

    protected void setVerticalCollider()
    {
        hCollider.enabled = false;
        vCollider.enabled = true;
    }

    protected override void updDirection()
    {
        if (wallSliding)
        {
            actionDirection = lookDirection*-1;    
        }

        base.updDirection();
    }
    
    protected void detectIfHuggingWall()
    {
        wallSliding = (wallColliderL.IsTouchingLayers(groundLayer) || wallColliderR.IsTouchingLayers(groundLayer))&&!grounded;
        wallSlideLookDirection = wallColliderR.IsTouchingLayers(groundLayer) ? 1 : -1;
         
        fsm.FsmVariables.GetVariable("wallSliding").RawValue = wallSliding;
    }

    protected override void animatePlayer()
    {
        string stateName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        spriteRenderer.flipX = rightAnims.Contains(stateName) ? lookDirection < 0: lookDirection > 0;
    }

    protected bool animatorOnTumbleState()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Tumble");
    }

    public void MoveToXPosition(float xPos)
    {
        Vector3 oldPos3D = gameObject.transform.position;
        Vector2 oldPos2D = new Vector2(oldPos3D.x, oldPos3D.y);
        Vector2 newPos = new Vector2(xPos, oldPos3D.y);
        Debug.Log(Vector2.Lerp(oldPos2D, newPos, 0.5f));         
    }

    public void GetHit()
    {
        fsm.SendEvent("GetHitGlobal");
    }
}
