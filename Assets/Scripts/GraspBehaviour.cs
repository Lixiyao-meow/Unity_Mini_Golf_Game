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
    private bool owner = false; // new
    private bool lastBallOwner = false;

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
        Debug.Log("!!!! LOST OWNERSHIP!!!!!!! " + gameObject.name);
        owner = false;
        var data = msg.FromJson<Message>();
        transform.position = data.position;
        transform.rotation = data.rotation;
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

    
    public void Grasp(Hand controller)
    {
        hand = controller;
        if (!owner)
        {
            Debug.Log("Message sent from grasp change owner");
            context.SendJson(new Message(transform));
        }
        body.isKinematic = true;
        owner = true;
        Debug.Log("!!!! GAINED OWNERSHIP!!!!!!!! " + gameObject.name);
    }

    public void Release(Hand controller)
    {
        Debug.Log("RELEASED");
        hand = null;
        body.isKinematic = false;
        body.velocity = controller.velocity;
        //owner = false;
    }
    
    // Start is called before the first frame update
    internal void Awake()
    {
        context = NetworkScene.Register(this);
        Debug.Log("GraspBehaviour is awake!");
        body = GetComponent<Rigidbody>();
    }

    internal void UpdateOwnership(BallController ball)
    {
        if (owner == lastBallOwner) return;
        lastBallOwner = owner;
        if (owner)
        {
            ball.setOwner();
        } else
        {
            ball.removeOwner();
        }
    }

    // Update is called once per frame
    internal void LateUpdate()
    {

        if (hand)
        {
            transform.position = hand.transform.position;
            transform.rotation = hand.transform.rotation;
        }
    }

    private void FixedUpdate()
    {
        if (owner)
        {
            Debug.Log("I AM THE OWNER " + gameObject.name);
            context.SendJson(new Message(transform));
        }
    }
}
