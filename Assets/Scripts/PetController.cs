using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PetController : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 3.5f;
    [SerializeField] private float wanderSpeed = 1.5f;
    [SerializeField] private WorldArea wanderArea;
    [SerializeField] private float destinationBuffer;
    [SerializeField] private Transform platformTransformTarget;
    [SerializeField] private float idleTime = 15f;
    [SerializeField] private AudioSource munchAudio;
    
    private NavMeshAgent _agent;
    private VirtualPetState _virtualState;
    private float _idleTimer;
    private PetEffects _effects;
    private PetArduinoHandler _arduino;
    
    private VirtualPetState CurrentState
    {
        get => _virtualState;
        set
        {
            if(_virtualState == value)
                return;
            _virtualState = value;
            _petStateChanged = true;
        }
    }

    private bool _petStateChanged;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _effects = GetComponent<PetEffects>();
        CurrentState = VirtualPetState.Wander;
    }

    void Update()
    {
        switch (CurrentState)
        {
            case VirtualPetState.Wander:
                Wander();
                break;
            case VirtualPetState.Return:
                ReturnToPlatform();
                break;
            case VirtualPetState.Idle:
                Idle();
                break;
            default:
                return;
        }
    }

    private void ReturnToPlatform()
    {
        if (_petStateChanged)
        {
            _agent.speed = returnSpeed;
            _agent.SetDestination(platformTransformTarget.position);
            _petStateChanged = false;
        }
            
        
        if(_agent.remainingDistance > destinationBuffer)
            return;
        
        //temp
        transform.SetPositionAndRotation(platformTransformTarget.position, platformTransformTarget.rotation);
        CurrentState = VirtualPetState.Idle;
    }

    private bool overrideBoredom;
    private void Idle()
    {
        if (_petStateChanged)
        {
            _idleTimer = 0;
            _petStateChanged = false;
        }

        if (overrideBoredom)
        {
            _idleTimer = 0;
        }
            

        _idleTimer += Time.deltaTime;
        print(_idleTimer);
        if (_idleTimer >= idleTime)
            CurrentState = VirtualPetState.Wander;
    }

    private void Wander()
    {
        if (_petStateChanged)
        {
            _agent.speed = wanderSpeed;
            _agent.SetDestination(wanderArea.GetRandomPosition());
            _petStateChanged = false;
        }
        
        if(_agent.remainingDistance < destinationBuffer)
        {
            _agent.SetDestination(wanderArea.GetRandomPosition());
        }
    }

    [ContextMenu("Set Wander")]
    public void EditorSetWander() => CurrentState = VirtualPetState.Wander;
    [ContextMenu("Set Idle")]
    public void EditorSetIdle() => CurrentState = VirtualPetState.Idle;
    [ContextMenu("Set Return")]
    public void EditorSetReturn() => CurrentState = VirtualPetState.Return;

    private Coroutine _candyInterestRoutine;

    [ContextMenu("Start Candy Interest")]
    public void StartCandyInterest()
    {
        overrideBoredom = true;
        if (CurrentState == VirtualPetState.Wander)
            CurrentState = VirtualPetState.Return;

        if(_candyInterestRoutine != null)
            StopCoroutine(_candyInterestRoutine);
        
        _candyInterestRoutine = StartCoroutine(C_CandyInterest());
    }

    [ContextMenu("Stop Candy Interest")]
    public void StopCandyInterest()
    {
        overrideBoredom = false;
        _effects.StopSmileys();
        if(_candyInterestRoutine != null)
            StopCoroutine(C_CandyInterest());
    }

    private IEnumerator C_CandyInterest()
    {
        _effects.PlayStars();
        while (_effects.StarsActive)
            yield return null;
        
        _effects.PlaySmileys();
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Fox got something in its face");
        if (!other.TryGetComponent(out ObjectReturn objectReturn)) 
            return;
        
        print("it was food!");
        munchAudio.Play();
        objectReturn.ReturnToStart();
        StopCandyInterest();
    }
}

public enum VirtualPetState
{
    Wander,
    Return,
    Idle
}