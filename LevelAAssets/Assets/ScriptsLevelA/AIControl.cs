using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour {

	GameObject[] goalLocations;
	UnityEngine.AI.NavMeshAgent agent;
	Animator anim;

	private bool setveryfast = false;
	// Use this for initialization
	void Start () {
		goalLocations = GameObject.FindGameObjectsWithTag("goal");
		agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
		anim = this.GetComponent<Animator>();
		agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);

		anim.SetFloat("wOffset", Random.Range(0, 1));
		anim.SetTrigger("isWalking");
		
		float sm = Random.Range(0.5f, 1.5f);
		anim.SetFloat("speedMult", sm);
		agent.speed *= sm;
	}
	
	// Update is called once per frame
	void Update () {
		if (agent.remainingDistance < 1)
		{
			agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
		}
	}

	public void walkSlow()
	{
		float sm = 0.1f;
		anim.SetFloat("speedMult", sm);
		agent.speed *= sm;
	}

	public void setFast()
	{
		setveryfast = true;
		Debug.Log(setveryfast);
	}
}
