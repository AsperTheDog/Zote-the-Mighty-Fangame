using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;

[RequireComponent(typeof(DialogueFSM))]
public class InspectElem : MonoBehaviour
{
    public LayerMask layer;
    [HideInInspector]public DialogueFSM diagFSM;
    public Dialogue dialogue;

    public PlayMakerFSM inspectDisplay;
    private PlayMakerFSM playerFSM;

    bool inArea = false;

    Collider2D coll;
    InspectManager inspManager;

    private void Start()
    {
        if(dialogue == null)
        {
            Debug.LogWarning("Dialogue file not set for object" + gameObject.name);
        }
        else
        {
            coll = gameObject.GetComponent<Collider2D>();
            inspManager = FindObjectOfType<InspectManager>();
            diagFSM = GetComponent<DialogueFSM>();
            foreach (var fsm in FindObjectOfType<MovementController>().gameObject.GetComponents<PlayMakerFSM>())
            {
                if (fsm.FsmName == "MainController")
                {
                    playerFSM = fsm;
                    break;
                }
            }
            if(playerFSM is null)
            {
                Debug.LogError("Player movement controller fsm not found!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {        
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            inArea = true;
            inspectDisplay.FsmVariables.GetFsmBool("insideTrigger").RawValue = true;
        }    
    }

    private void Update()
    {
        if (inArea)
        {
            if (coll.IsTouchingLayers(layer) && !inspManager.isInspecting && playerFSM.Fsm.ActiveStateName == "Idle")
            {
                if (Input.GetButtonDown("Action"))
                {
                    var sentences = diagFSM.GetSentences(dialogue);
                    if (sentences != null)
                        inspManager.StartDialogue(dialogue, sentences);
                }
            }
            inspectDisplay.FsmVariables.GetFsmBool("isInspecting").RawValue = inspManager.isInspecting;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            inArea = false;
            inspectDisplay.FsmVariables.GetFsmBool("insideTrigger").RawValue = false;
        }
    }
}
