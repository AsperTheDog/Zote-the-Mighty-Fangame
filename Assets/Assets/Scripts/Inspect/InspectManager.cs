using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InspectManager: MonoBehaviour
{
    public Text inspectName;
    public Text inspectSubtitle;
    public Text inspectBody;

    public GameObject canvas;

    Queue<string> sentencesQ;

    bool _isInspecting;

    Coroutine typeSentenceRoutine;
    string currentSentence;

    Animator anim;

    public bool isInspecting { get => _isInspecting; }

    public float TypeSentenceTime = 0.018f;
    bool IsFirstSentence = true;

    private void Start()
    {
        sentencesQ = new Queue<string>();
        _isInspecting = false;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isInspecting)
        {
            if(Input.GetButtonDown("Attack") && inspectBody.text == currentSentence)
            {
                NextSentence();
            }

            if(Input.GetButton("Attack"))
            {
                TypeSentenceTime = 0.01f;
            }
            else
            {
                TypeSentenceTime = 0.018f;
            }
        }
    }

    public void StartDialogue(Dialogue dialogue, string[] sentences)
    {
        setInspecting(true);
        SetAnimation("FadeIn");
        inspectName.text = dialogue.name;
        inspectSubtitle.text = dialogue.subtitle;

        sentencesQ.Clear();

        foreach (string sentence in sentences)
        {
            sentencesQ.Enqueue(sentence);
        }
        IsFirstSentence = true;
        NextSentence();
    }

    void NextSentence()
    {
        StopAllCoroutines();
        if (typeSentenceRoutine != null)
        {
            inspectBody.text = currentSentence;
            typeSentenceRoutine = null;
            if (sentencesQ.Count != 0)
            {
                SetAnimation("NextSentence");
            }
            else
            {
                SetAnimation("FullStop");
            }
        }
        else
        {
            if (sentencesQ.Count == 0)
            {
                EndDialogue();
                return;
            }
            if(!IsFirstSentence)
                SetAnimation("NextSentenceOut");
            typeSentenceRoutine = StartCoroutine(TypeSentence(sentencesQ.Dequeue()));               

        }        
    }

    IEnumerator TypeSentence (string sentence)
    {
        currentSentence = sentence;
        inspectBody.text = "";
        if (IsFirstSentence)
        {
            yield return new WaitForSecondsRealtime(0.25f);
            IsFirstSentence = false;
        }

        foreach (var letter in sentence.ToCharArray())
        {
            inspectBody.text += letter;
            yield return new WaitForSecondsRealtime(TypeSentenceTime);
            if (inspectBody.text == sentence)
            {
                typeSentenceRoutine = null;
                if (sentencesQ.Count != 0)
                {
                    SetAnimation("NextSentence");
                }
                else
                {
                    SetAnimation("FullStop");
                }
            }
        }

        
    }

    void EndDialogue()
    {
        setInspecting(false);
        SetAnimation("FadeOut");
    }

    void setInspecting(bool value)
    {
        //canvas.SetActive(value);
        if (value)
        {
            FindObjectOfType<MovementController>().movementDisable();
            
        }
        else
        {
            FindObjectOfType<MovementController>().movementEnable();
        }        
        _isInspecting = value;        
    }

    void SetAnimation(string name)
    {
        anim.Play(name);
    }

}
