using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ubiq.XR;
using UnityEngine;
using Ubiq.Spawning;
using Ubiq.Messaging;

public class ClubController : GraspBehaviour
{
    public Transform visual;
    private Quaternion relativeRotation;
    
    private Hand follow;
    private Rigidbody club;
    private Vector3 initialPosition;
    public BallController myBall;
    
    Vector3 lastPosition;

    void Start()
    {
    }

    internal override void Awake()
    {
        base.Awake();
        club = GetComponent<Rigidbody>();
        initialPosition = club.position;
    }

    public override void Grasp(Hand controller)
    {
        base.Grasp(controller);
        // Vector (A -> B) => (B - A)
        Vector3 relativePosition = transform.position - controller.transform.position;
        visual.position += relativePosition;
    }

    public override void Release(Hand controller)
     {
        base.Release(controller);
     }


    internal override void Update()
    {
        base.UpdateOwnership(myBall);
        base.Update();
    }

}
