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

    NetworkContext context;
    Vector3 lastPosition;

    void Start()
    {
        context = NetworkScene.Register(this);
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
    }

    private void Update()
    {
        base.Update();
        if(lastPosition != transform.localPosition)
        {
            lastPosition = transform.localPosition;
            context.SendJson(new Message()
            {
                position = transform.localPosition
            });
        }
    }

    private struct Message
    {
        public Vector3 position;
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        // Parse the message
        var m = message.FromJson<Message>();

        // Use the message to update the Component
        transform.localPosition = m.position;

        // Make sure the logic in Update doesn't trigger as a result of this message
        lastPosition = transform.localPosition;
    }

    public void BackToInitialPosition(){
        club.MovePosition(initialPosition);
        club.velocity = Vector3.zero;
        club.angularVelocity = Vector3.zero;
        club.isKinematic = true;
    }
}
