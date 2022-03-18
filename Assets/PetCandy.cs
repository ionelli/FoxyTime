using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetCandy : OVRGrabbable
{
    [SerializeField] private PetController pet;
    private ObjectReturn _objectReturn;

    protected override void Start()
    {
        base.Start();
        _objectReturn = GetComponent<ObjectReturn>();
    }

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        base.GrabBegin(hand, grabPoint);
        pet.StartCandyInterest();
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(linearVelocity, angularVelocity);
        pet.StopCandyInterest();
        _objectReturn.ReturnToStart();
    }
}
