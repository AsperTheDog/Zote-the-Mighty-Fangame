using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectChildren : MonoBehaviour
{
    public PlayMakerFSM fsm;
    Collider2D areaCollider;
    public int childrenInArea = 0;
    public int childrenToFind = 3;

    private void Start()
    {
        areaCollider = GetComponent<Collider2D>();
    }
    void Update()
    {
        childrenInArea = CountChildren();

        if (childrenInArea == childrenToFind)
            fsm.FsmVariables.GetVariable("missionCompleted").RawValue = true;
    }

    private int CountChildren()
    {
        int count = 0;
        List<Collider2D> collisions = new List<Collider2D>();
        areaCollider.OverlapCollider(new ContactFilter2D() { }, collisions);

        foreach (var collision in collisions)
        {
            if (collision.tag == "BaldurMissionObjective")
                count++;
        }

        return count;
    }
}
