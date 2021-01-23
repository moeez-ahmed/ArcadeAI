using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;

    public bool isShielded;

    private Animator anim;

    private Image healthImg;

    public bool sheilded
    {
        get { return isShielded; }
        set { isShielded = value; }
    }

    void Awake()
    {
        anim = GetComponent<Animator>();

        healthImg = GameObject.Find("Health Icon").GetComponent<Image>();
    }

    void Update()
    {
        MainMenuController.currentTime += 1 * Time.deltaTime;
        //print(MainMenuController.currentTime);
    }

    public void TakeDamage(float amount)
    {
        if (!isShielded)
        {
            health -= amount;

            healthImg.fillAmount = health / 100f;

            print("Player took damage : " + health);

            if (health <= 0f)
            {
                anim.SetBool("Death", true);

                if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
                {
                    //Player has Died xD
                    Destroy(gameObject, 2f);
                    SceneManager.LoadScene("FinishScreen");
                }
            }
        }
    }

    public void HealPlayer(float healAmount)
    {
        health += healAmount;

        if (health >= 100)
        {
            health = 100f;
        }

        healthImg.fillAmount = health / 100f;
    }
}
