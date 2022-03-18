using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static OVRHand;

public class HandInteraction : OVRGrabber
{
    [SerializeField, Range(0,1)] private float minPinchStrength = 0.5f;
    [SerializeField] private OVRHand _hand;

    private Rigidbody _heldItem;
    

    private OVRGrabber _grabber;

    private bool HasItem => _heldItem != null;

    public override void Update()
    {
        base.Update();

        bool pinching = IsPinching();

        if (!m_grabbedObj && pinching && m_grabCandidates.Count > 0)
            GrabBegin();
        else if (m_grabbedObj && !pinching)
            GrabEnd();
    }

    private bool IsPinching()
    {
        bool validPinch = _hand.GetFingerIsPinching(HandFinger.Index);

        if (!validPinch)
            return false;

        return _hand.GetFingerPinchStrength(HandFinger.Index) > grabBegin;
    }
}
