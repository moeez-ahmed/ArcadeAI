using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShield : MonoBehaviour
{

    private PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void OnEnable()
    {
        playerHealth.isShielded = true; 
    }

    private void OnDisable()
    {
        playerHealth.isShielded = false;
    }
}
