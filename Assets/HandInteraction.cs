using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static OVRHand;

public class HandInteraction : MonoBehaviour
{
    [SerializeField, Range(0,1)] private float minPinchStrength = 0.5f;
    [SerializeField] private float handReachDistance = 1f;
    [SerializeField] private LayerMask holdableItemMask;

    private Rigidbody _heldItem;
    private OVRHand _hand;

    private OVRGrabber _grabber;

    private bool HasItem => _heldItem != null;

    private void Awake()
    {
        _hand = GetComponent<OVRHand>();
    }

    private void Update()
    {
        bool pinching = IsPinching();

        if (HasItem && !pinching)
        {
            _heldItem.isKinematic = false;
            _heldItem.transform.SetParent(null);
            Debug.Log($"Dropped {_heldItem.gameObject.name}");
            _heldItem = null;
            return;
        }

        if (!HasItem && pinching)
        {
            if(!_hand.IsPointerPoseValid)
                return;
            
            Vector3 pointDir = _hand.PointerPose.forward;
            Vector3 pointPos = _hand.PointerPose.position;

            if (Physics.Raycast(pointPos, pointDir,
                    out RaycastHit hit, handReachDistance, holdableItemMask))
            {
                if(!hit.collider.TryGetComponent(out _heldItem))
                    return;
                
                _heldItem.transform.SetParent(_hand.transform);
                _heldItem.transform.localPosition = Vector3.zero;
                Debug.Log($"Picked up {_heldItem.gameObject.name}");
            }
        }
    }

    private bool IsPinching()
    {
        bool validPinch = _hand.GetFingerIsPinching(HandFinger.Thumb) 
                          && _hand.GetFingerIsPinching(HandFinger.Index);

        if (!validPinch)
            return false;

        return !(_hand.GetFingerPinchStrength(HandFinger.Index) < minPinchStrength);
    }

    public Transform GetAttachTransform(IXRInteractable interactable)
    {
        throw new NotImplementedException();
    }

    public void GetValidTargets(List<IXRInteractable> targets)
    {
        throw new NotImplementedException();
    }

    public void OnRegistered(InteractorRegisteredEventArgs args)
    {
        throw new NotImplementedException();
    }

    public void OnUnregistered(InteractorUnregisteredEventArgs args)
    {
        throw new NotImplementedException();
    }

    public void PreprocessInteractor(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        throw new NotImplementedException();
    }

    public void ProcessInteractor(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        throw new NotImplementedException();
    }

    public InteractionLayerMask interactionLayers { get; }
    public event Action<InteractorRegisteredEventArgs> registered;
    public event Action<InteractorUnregisteredEventArgs> unregistered;
}
