using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueFSM : MonoBehaviour
{
    PlayMakerFSM fsm;
    // Start is called before the first frame update
    void Start()
    {
        fsm = GetComponent<PlayMakerFSM>();
    }

    private void SetAsStartState()
    {
        fsm.Fsm.StartState = fsm.ActiveStateName;
    }

    public void NextState()
    {
        fsm.FsmVariables.GetFsmBool("finished").RawValue = true;
    }

    public string[] GetSentences(Dialogue dialogue)
    {
        var sentences = dialogue.GetSentences(fsm.ActiveStateName);
        if (sentences != null)
        {
            fsm.FsmVariables.GetFsmBool("changeState").RawValue = true;
            return sentences;
        }
        else
        {
            Debug.LogWarning("Could not find text for current state: " + fsm.ActiveStateName);
            return null;
        }
    }

}
