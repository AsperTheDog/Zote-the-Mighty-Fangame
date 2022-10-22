using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneConnectionsGenerator;
using UnityEngine.SceneManagement;

public abstract class MovementController : MonoBehaviour
{
    [Header("Movement Parameteres")]
    [SerializeField] protected bool moveEnabled = true;
    [SerializeField] protected float runSpeed = 8.3f;

    [Header("Collision Parameteres")]
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected Collider2D groundCollider;
    [SerializeField] protected bool grounded = true;

    [Header("Input Parameters")]
    [SerializeField] protected float hInput = 0; 

    [Header("Animation Parameters")]
    [SerializeField] protected float lookDirection = 1;
    [SerializeField] protected float prevLookDirection = 1;
    [SerializeField] protected float actionDirection = 1;
    protected SpriteRenderer spriteRenderer;

    protected Rigidbody2D rb;
    public PlayMakerFSM fsm;
    protected Animator animator;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Health.OnDeath += () => PlayMakerFSM.BroadcastEvent("DeathGlobal");
        SetStartingPosition();
    }

    public virtual void Update()
    {
        detectIfGrounded();
        UpdatePreMove();
        if (moveEnabled)
        {
            movePlayer();
        }        
        animatePlayer();
    }

    protected virtual void UpdatePreMove() { }


    protected virtual void movePlayer()
    {
        hInput = Input.GetAxisRaw("Horizontal");

        float hFactor = 0; 
        if (hInput != 0)
        {
            hFactor = hInput > 0 ? 1 : -1;
            lookDirection = hFactor;
            actionDirection = lookDirection;

        }
        Vector2 move = new Vector2(runSpeed*hFactor, rb.velocity.y);

        fsm.FsmVariables.GetVariable("moving").RawValue = move[0]!=0;
        updDirection();

        rb.velocity = move;
    }

    protected virtual void updDirection()
    {
        
        if (prevLookDirection != lookDirection)
        {
            fsm.FsmVariables.GetVariable("turning").RawValue = true;
            prevLookDirection = lookDirection;
        }
        else
        {
            fsm.FsmVariables.GetVariable("turning").RawValue = false;
        }
        
        fsm.FsmVariables.GetVariable("actionDirection").RawValue = actionDirection;
    }

    protected virtual void animatePlayer()
    {
        spriteRenderer.flipX = lookDirection < 0;
    }

    protected void detectIfGrounded()
    {
        grounded = groundCollider.IsTouchingLayers(groundLayer);
        fsm.FsmVariables.GetVariable("grounded").RawValue = grounded;
    }


    public void movementDisableScript()
    {
        moveEnabled = false;
    }

    public void movementEnable()
    {
        moveEnabled = true;
        fsm.enabled = true;
    }

    public void movementDisable()
    {
        moveEnabled = false;
        fsm.enabled = false;
    }

    protected void setLookDirection()
    {
        lookDirection = actionDirection;
    }

    private void SetStartingPosition()
    {
        try
        {
            var spawn = FindObjectOfType<SceneConnectionManager>().spawn;

            var spawnObj = GameObject.Find(spawn);
            var newPos = spawnObj.transform.position;

            gameObject.transform.position = new Vector2(newPos.x, newPos.y);
        }
        catch (System.NullReferenceException)
        {
            Debug.LogWarning("Spawn point not found when loading scene...");
            return;
        }
    }
}
