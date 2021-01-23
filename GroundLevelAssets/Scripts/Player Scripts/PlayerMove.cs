﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Animator anim;
    private CharacterController charController;
    private CollisionFlags collisionFlags = CollisionFlags.None;

    private float moveSpeed = 5f;
    private bool canMove;
    private bool finishedMovement = true;

    private Vector3 targetPos = Vector3.zero;
    private Vector3 playerMove = Vector3.zero;

    private float playerToPointDistance;

    private float gravity = 9.8f;
    private float height;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateHeight();
        CheckIfFinishedMovement();
    }

    bool IsGrounded()
    {
        return collisionFlags == CollisionFlags.CollidedBelow ? true : false;
        //if (collisionFlags == CollisionFlags.CollidedBelow)
        //{
        //    return true;
        //}
        //return false;
    }

    void CalculateHeight()
    {
        if (IsGrounded())
        {
            height = 0f;
        }
        else
        {
            height -= gravity * Time.deltaTime;
        }
    }

    void CheckIfFinishedMovement()
    {
        if (!finishedMovement)
        {
            if (!anim.IsInTransition(0) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Stand") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
            {
                finishedMovement = true;
            }
        }
        else
        {
            MoveThePlayer();
            playerMove.y = height * Time.deltaTime;
            collisionFlags = charController.Move(playerMove);
        }
    }

    void MoveThePlayer()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast (ray, out hit))
            {
                if (hit.collider is TerrainCollider)
                {
                    playerToPointDistance = Vector3.Distance(transform.position, hit.point);

                    if (playerToPointDistance >= 1.0f)
                    {
                        canMove = true;
                        targetPos = hit.point;
                    }
                }
            }
        }
        if (canMove)
        {
            anim.SetFloat("Walk", 1.0f);

            Vector3 targetTemp = new Vector3(targetPos.x, transform.position.y, targetPos.z);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetTemp - transform.position), 15.0f * Time.deltaTime);

            playerMove = transform.forward * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, targetPos) <= 0.5f)
            {
                canMove = false;
            }
        }
        else
        {
            playerMove.Set(0f, 0f, 0f);
            anim.SetFloat("Walk", 0f);
        }
    }

    public bool FinishedMovement
    {
        get
        {
            return finishedMovement;
        }
        set
        {
            finishedMovement = value;
        }
    }

    public Vector3 TargetPosition
    {
        get
        {
            return targetPos;
        }
        set
        {
            targetPos = value;
        }
    }

}