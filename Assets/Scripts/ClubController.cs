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

    private void Awake()
    {
        base.Awake();
        club = GetComponent<Rigidbody>();
        initialPosition = club.position;
    }

    public void Grasp(Hand controller)
    {
        // Vector (A -> B) => (B - A)
        Vector3 relativePosition = transform.position - controller.transform.position;
        visual.position += relativePosition;
        Debug.Log("ahahahaha");
        //base.setBallOwner(myBall);
    }


    private void LateUpdate()
    {
        base.UpdateOwnership(myBall);
        base.LateUpdate();
    }

    public void BackToInitialPosition(){
        club.MovePosition(initialPosition);
        club.velocity = Vector3.zero;
        club.angularVelocity = Vector3.zero;
        club.isKinematic = true;
    }
}
