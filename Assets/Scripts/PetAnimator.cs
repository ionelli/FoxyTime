using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(
    typeof(NavMeshAgent), 
    typeof(Animator))
]
public class PetAnimator : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private Vector2 smoothDeltaPos;
    private Vector2 velocity;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        agent.updatePosition = false;
    }

    void Update()
    {
        Vector3 deltaPos = agent.nextPosition - transform.position;
        float dx = Vector3.Dot(transform.right, deltaPos);
        float dy = Vector3.Dot(transform.forward, deltaPos);

        float smooth = Mathf.Min(1f, Time.deltaTime / 0.15f);
        smoothDeltaPos = Vector2.Lerp(smoothDeltaPos, deltaPos, smooth);

        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPos / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;
        
        animator.SetBool("move", shouldMove);
        animator.SetFloat("velx", velocity.x);
        animator.SetFloat("vely", velocity.y);
    }

    private void OnAnimatorMove()
    {
        transform.position = agent.nextPosition;
    }
}
