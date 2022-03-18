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
    
    private NavMeshAgent _agent;
    private PetState _currentState;
    private float _idleTimer;
    
    private PetState CurrentState
    {
        get => _currentState;
        set
        {
            if(_currentState == value)
                return;
            _currentState = value;
            _petStateChanged = true;
        }
    }

    private bool _petStateChanged;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        CurrentState = PetState.Wander;
    }

    void Update()
    {
        switch (CurrentState)
        {
            case PetState.Wander:
                Wander();
                break;
            case PetState.Return:
                ReturnToPlatform();
                break;
            case PetState.Idle:
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
        }
            
        
        if(_agent.remainingDistance > destinationBuffer)
            return;
        
        //temp
        transform.SetPositionAndRotation(platformTransformTarget.position, platformTransformTarget.rotation);
        CurrentState = PetState.Idle;
    }

    private void Idle()
    {
        if (_petStateChanged)
            _idleTimer = 0;

        _idleTimer += Time.deltaTime;
        if (_idleTimer >= idleTime)
            CurrentState = PetState.Wander;
    }

    private void Wander()
    {
        if (_petStateChanged || _agent.remainingDistance < destinationBuffer)
        {
            _agent.speed = wanderSpeed;
            _agent.SetDestination(wanderArea.GetRandomPosition());
        }
    }

    [ContextMenu("Set Wander")]
    public void EditorSetWander() => CurrentState = PetState.Wander;
    [ContextMenu("Set Idle")]
    public void EditorSetIdle() => CurrentState = PetState.Idle;
    [ContextMenu("Set Return")]
    public void EditorSetReturn() => CurrentState = PetState.Return;
}

public enum PetState
{
    Wander,
    Return,
    Idle
}