using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Ubiq.XR;
using Ubiq.Messaging;

public class GraspBehaviour : MonoBehaviour, IGraspable
{
    private Hand hand;
    private Rigidbody body;
    private NetworkContext context; // new
    private bool owner; // new

    private struct Message
    {
        public Vector3 position;
        public Quaternion rotation;

        public Message(Transform transform)
        {
            this.position = transform.position;
            this.rotation = transform.rotation;
        }
    }

    private void Start()
    {
        context = NetworkScene.Register(this);
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage msg)
    {
        var data = msg.FromJson<Message>();
        transform.position = data.position;
        transform.rotation = data.rotation;
    }

    private void FixedUpdate()
    {
        if (owner)
        {
            context.SendJson(new Message(transform));
        }
    }

    internal void setOwner()
    {
        owner = true;
    }

    internal void removeOwner()
    {
        owner = false;
    }

    internal void setBallOwner(BallController ball)
    {
        ball.setOwner();
    }

    internal void removeBallOwner(BallController ball)
    {
        ball.removeOwner();
    }

    private void LateUpdate()
    {
        if (hand)
        {
            transform.position = hand.transform.position;
            transform.rotation = hand.transform.rotation;
        }
    }

    public void Grasp(Hand controller)
    {
        hand = controller;
        body.isKinematic = true;
        owner = true;
    }

    public void Release(Hand controller)
    {
        hand = null;
        body.isKinematic = false;
        body.velocity = controller.velocity;
        owner = false;
    }
    
    // Start is called before the first frame update
    internal void Awake()
    {
        Debug.Log("GraspBehaviour is awake!");
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    internal void Update()
    {
        if (hand == null) return;

        body.MovePosition(hand.transform.position);
        body.MoveRotation(hand.transform.rotation);
    }
}
