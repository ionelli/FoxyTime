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
    private PetController controller;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<PetController>();
    }

    void Start()
    {
        agent.updatePosition = false;
    }

    void Update()
    {
        Vector3 worldDeltaPos = agent.nextPosition - transform.position;
        float dx = Vector3.Dot(transform.right, worldDeltaPos);
        float dy = Vector3.Dot(transform.forward, worldDeltaPos);
        Vector2 deltaPos = new Vector2(dx, dy);

        float smooth = Mathf.Min(1f, Time.deltaTime / 0.15f);
        smoothDeltaPos = Vector2.Lerp(smoothDeltaPos, deltaPos, smooth);

        if (Time.deltaTime > 1e-5f)
            velocity = deltaPos;// / Time.deltaTime;

        bool shouldMove = velocity.magnitude > .1f || agent.remainingDistance > agent.radius;
        
        animator.SetBool("move", shouldMove);
        animator.SetFloat("velx", velocity.x);
        animator.SetFloat("vely", velocity.y);

        // if (worldDeltaPos.magnitude > agent.radius)
        //     agent.nextPosition = transform.position + 0.9f * worldDeltaPos;
        
        if(controller.CurrentState == VirtualPetState.Idle)
            return;
        
        if (worldDeltaPos.magnitude > float.Epsilon)
            transform.position = agent.nextPosition - 0.9f*worldDeltaPos;
    }

    private void OnAnimatorMove()
    {
        //transform.position = agent.nextPosition;
        // Vector3 position = animator.rootPosition;
        // position.y = agent.nextPosition.y;
        // transform.position = position;
    }
}
