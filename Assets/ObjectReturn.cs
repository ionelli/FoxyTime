using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectReturn : MonoBehaviour
{
    [SerializeField] private float startDistanceBuffer = 1f;
    [SerializeField] private float returnWaitTime;

    private bool _hasMoved;
    private Vector3 _startPos;
    private float _returnWait;
    private Rigidbody _body;
    private OVRGrabbable _grabbable;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        _grabbable = GetComponent<OVRGrabbable>();
        _startPos = transform.position;
    }

    void Update()
    {
        if(transform.position.y < -5f)
            ReturnToStart();
        
        if (_hasMoved)
        {
            if (_grabbable.isGrabbed)
            {
                _returnWait = returnWaitTime;
                return;
            }
            
            _returnWait -= Time.deltaTime;
            if (_returnWait <= 0)
                ReturnToStart();
            return;
        }

        if (Vector3.Distance(transform.position, _startPos) < startDistanceBuffer) 
            return;

        if (_grabbable.isGrabbed)
            return;
        
        _hasMoved = true;
        _returnWait = returnWaitTime;
    }

    public void ReturnToStart()
    {
        transform.position = _startPos;
        _hasMoved = false;
        _body.velocity = Vector3.zero;
    }
}
