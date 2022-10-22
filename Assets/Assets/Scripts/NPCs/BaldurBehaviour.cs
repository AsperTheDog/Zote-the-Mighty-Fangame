using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaldurBehaviour : MonoBehaviour
{
    public PlayMakerFSM fsm;

    public Collider2D leftCollider;
    public Collider2D rightCollider;

    private void Start()
    {
        StartCoroutine(SendRandomValueToFSM());
    }

    private IEnumerator SendRandomValueToFSM()
    {
        while (true)
        {
            fsm.FsmVariables.GetVariable("randomStopVariable").RawValue = Random.Range(0f, 1f);
            yield return new WaitForSecondsRealtime(Random.Range(0.8f, 1.2f)); 
        }
    }

    void Update()
    {
        fsm.FsmVariables.GetVariable("missingGroundLeft").RawValue = !CheckGround(leftCollider);
        fsm.FsmVariables.GetVariable("missingGroundRight").RawValue = !CheckGround(rightCollider);        
    }

    private bool CheckGround(Collider2D coll)
    {
        List<Collider2D> collisions = new List<Collider2D>();
        coll.OverlapCollider(new ContactFilter2D() { }, collisions);

        foreach (var collision in collisions)
        {
            if (collision.tag == "Ground")
                return true;
        }

        return false;
    }

    public void SetFreezeRotation(bool value)
    {
        gameObject.GetComponent<Rigidbody2D>().freezeRotation = value;
    }
}
