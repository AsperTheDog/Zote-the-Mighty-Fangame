using UnityEngine;

public class ElderBaldurBehaviour : MonoBehaviour
{
    public PlayMakerFSM fsm;

    private void Start()
    {
        fsm = gameObject.GetComponentInParent<PlayMakerFSM>();
    }

    public void TriggerEnter2D()
    {
        fsm.FsmVariables.GetVariable("insideTrigger").RawValue = true;
    }

    public void TriggerExit2D()
    {
        fsm.FsmVariables.GetVariable("insideTrigger").RawValue = false;
    }

}
