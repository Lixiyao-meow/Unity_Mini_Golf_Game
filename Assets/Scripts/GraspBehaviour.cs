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
        Debug.Log("!!!! LOST OWNERSHIP!!!!!!!!");
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
        body.isKinematic = true;
        context.SendJson(new Message(transform));
        owner = true;
        Debug.Log("!!!! GAINED OWNERSHIP!!!!!!!!");
    }

    public void Release(Hand controller)
    {
        Debug.Log("(((((((((((((((((((((()))))))))))))))))))))))))))");
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
        Debug.Log("______________________IN HERE _____________________", ball);
        if (owner)
        {
            ball.setOwner();
        } else
        {
            ball.removeOwner();
        }
    }

    // Update is called once per frame
    internal void Update()
    {
        if (owner)
        {
            context.SendJson(new Message(transform));
        }

        if (hand == null) return;

        body.MovePosition(hand.transform.position);
        body.MoveRotation(hand.transform.rotation);
    }
}
