using UnityEngine;
using UnityEngine.Events;

public class PlayerEventManager : MonoBehaviour
{
    public UnityEvent PlayerInTrigger;
    public UnityEvent OnPlayerTriggerEnter;
    public UnityEvent OnPlayerTriggerExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            OnPlayerTriggerEnter?.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            OnPlayerTriggerExit?.Invoke();
        }        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerInTrigger?.Invoke();
        }        
    }

}
