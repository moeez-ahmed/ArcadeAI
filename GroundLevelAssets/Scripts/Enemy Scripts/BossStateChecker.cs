using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Boss_State
{
    NONE,
    IDLE,
    PAUSE,
    ATTACK,
    DEATH
}

public class BossStateChecker : MonoBehaviour
{

    private Transform playerTarget;
    private Boss_State bossState = Boss_State.NONE;
    private float distanceToTarget;

    private EnemyHealth bossHealth;

    // Start is called before the first frame update
    void Awake()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        bossHealth = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        SetState();
    }

    void SetState()
    {
        distanceToTarget = Vector3.Distance(transform.position, playerTarget.position);

        if (bossState != Boss_State.DEATH)
        {
            if (distanceToTarget > 3 && distanceToTarget <= 15f)
            {
                bossState = Boss_State.PAUSE;
            }
            else if (distanceToTarget > 15f)
            {
                bossState = Boss_State.IDLE;
            }
            else if (distanceToTarget <= 3f)
            {
                bossState = Boss_State.ATTACK;
            }
            else
            {
                bossState = Boss_State.NONE;
            }

            if (bossHealth.health <= 0f)
            {
                bossState = Boss_State.DEATH;
            }
        }
    }

    public Boss_State BossState
    {
        get { return bossState; }
        set { bossState = value; }
    }
}
