﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{

    public float health = 100f;

    public Image healthImg;


    public void TakeDamage(float amount)
    {
        health -= amount;

        healthImg.fillAmount = health / 100f;

        print("Enemy took damage, the health is now : " + health);

        if (health <= 0)
        {

        }
    }
}