using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossControl : MonoBehaviour
{
    public Image diamond;
    public Text informationText;

    private Transform playerTarget;
    private BossStateChecker bossStateChecker;
    private NavMeshAgent navAgent;
    private Animator anim;


    private bool finishedAttacking = true;
    private float currentAttackTime;
    private float waitAttackTime = 1f;


    // Start is called before the first frame update
    void Awake()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        bossStateChecker = GetComponent<BossStateChecker>();
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (finishedAttacking)
        {
            GetStateControl();
        }
        else
        {
            anim.SetInteger("Atk", 0);

            if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                finishedAttacking = true;
            }
        }
    }

    void GetStateControl()
    {
        if (bossStateChecker.BossState == Boss_State.DEATH)
        {
            navAgent.isStopped = true;
            anim.SetBool("Death", true);
            Destroy(gameObject, 3f);
            if (!diamond.gameObject.activeInHierarchy)
            {
                diamond.gameObject.SetActive(true);
                diamond.fillAmount = 1f;
                MainMenuController.diamondsAttained += 1;

                ChangeText(informationText, "Diamond Attained!", Color.green);

                StartCoroutine(WaitAndExecute());

                
            }
        }
        else
        {
            if (bossStateChecker.BossState == Boss_State.PAUSE)
            {
                navAgent.isStopped = false;
                anim.SetBool("Run", true);

                navAgent.SetDestination(playerTarget.position);
            }
            else if (bossStateChecker.BossState == Boss_State.ATTACK)
            {
                anim.SetBool("Run", false);

                Vector3 targetPosition = new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z);

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPosition - transform.position), 5f * Time.deltaTime);

                if (currentAttackTime >= waitAttackTime)
                {
                    int atkRange = Random.Range(1, 5);
                    anim.SetInteger("Atk", atkRange);

                    currentAttackTime = 0f;
                    finishedAttacking = false;
                }
                else
                {
                    anim.SetInteger("Atk", 0);
                    currentAttackTime += Time.deltaTime;
                }
            }
            else
            {
                anim.SetBool("Run", false);
                navAgent.isStopped = true;
            }
        }
    }
    Text ChangeText(Text informationText, string information, Color color)
    {
        informationText.text = information;
        informationText.color = color;

        return informationText;
    }

    IEnumerator WaitAndExecute()
    {
        yield return new WaitForSeconds(2f);
        print("Printed after wait time.");
        ChangeText(informationText, "Find the next stage!", Color.yellow);
    }
}
