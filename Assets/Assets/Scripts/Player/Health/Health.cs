using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int health;
    public int maxHealth;
    private int idx;
    private bool died = false;

    [SerializeField] GameObject testHeart;

    private Animator animator;

    public delegate void DeathEvent();
    public static event DeathEvent OnDeath;

    void Start()
    {
        resetHealth();
    }


    //testing
    private void Update()
    {
        updateHealth(false);
    }

    public void updateHealth(bool incMaxHealth)
    {
        if (incMaxHealth)
        {
            GameObject heart = Instantiate(testHeart, transform.position, Quaternion.identity);
            heart.transform.SetParent(gameObject.transform);
        }

        idx = 0;
        foreach (Transform child in transform)
        {
            animator = child.GetComponent<Animator>();
            if (idx < health)
            {
                animator.SetBool("HealthDown", false);
            }
            else if (idx >= health)
            {
                animator.SetBool("HealthDown", true);
            }
            idx++;
        }

        if (health == 0 & !died)
        {
            OnDeath?.Invoke();
            died = true;
        }
    }

    public void increaseHealth()
    {
        health++;
        updateHealth(false);
    }

    public void decreaseHealth()
    {
        health--;
        updateHealth(false);
    }

    public void increaseMaxHealth()
    {
        maxHealth++;
        health++;
        updateHealth(true);
    }

    public void resetHealth()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < maxHealth; i++)
        {
            GameObject heart = Instantiate(testHeart, transform.position, Quaternion.identity);
            heart.transform.SetParent(gameObject.transform);

            if (i >= health)
            {
                animator = heart.GetComponent<Animator>();
                animator.SetBool("HealthDown", true);
            }
            idx++;
        }
    }
}

