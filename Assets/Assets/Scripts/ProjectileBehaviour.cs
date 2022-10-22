using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileBehaviour : MonoBehaviour
{
    public float maxVelocitySqueeze = 5f;
    [Range(0.0f, 0.99f)]
    public float maxSqueeze = 0.5f;
    public float timeToDestroy = 1f;

    public float angle;

    private Rigidbody2D rb;
    private Animator anim;
    private bool collided = false;

    public UnityEvent OnProjectileCollision;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!collided)
        {
            SquezeWithVelocity(rb.velocity);
            angle = Vector3.SignedAngle(Vector3.right, rb.velocity, Vector3.forward);
            gameObject.transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }

    private void SquezeWithVelocity(Vector2 velocity)
    {
        var squeezeFactor = Mathf.Min(maxVelocitySqueeze, Mathf.Abs(velocity.magnitude))/maxVelocitySqueeze;
        var squeezeVal = squeezeFactor * maxSqueeze;
        var xSqueeze = 1 + squeezeVal;
        var ySqueeze = 1 - squeezeVal;
        gameObject.transform.localScale = new Vector3(xSqueeze, ySqueeze, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collided = true;
        angle = Vector3.SignedAngle(Vector3.right, (Vector3)collision.GetContact(0).point - gameObject.transform.position, Vector3.forward);
        angle = GetQuadrant(angle);

        gameObject.transform.eulerAngles = new Vector3(0, 0, angle);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        rb.freezeRotation = true;

        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;

        anim.Play("Impact");
        OnProjectileCollision?.Invoke();
        Destroy(gameObject, timeToDestroy);
    }

    private float GetQuadrant(float newAngle)
    {
        if (newAngle >= 135 || newAngle < -135)
        {
            newAngle = -90;
        }
        else if (newAngle >= 45)
        {
            newAngle = 180;
        }
        else if (newAngle >= -45)
        {
            newAngle = 90;
        }
        else
        {
            newAngle = 0;
        }

        return newAngle;
    }
}
