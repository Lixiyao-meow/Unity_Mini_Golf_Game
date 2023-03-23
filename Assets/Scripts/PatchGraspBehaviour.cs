using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Ubiq.XR;
using Ubiq.Messaging;

public class PatchGraspBehaviour : MonoBehaviour, IGraspable
{
    private Hand hand;
    private Rigidbody body;
    private NetworkContext context; // new
    private bool owner = false; // new
    float lastTime;
    float lastTimeBall;

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
        // context = NetworkScene.Register(this);
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage msg)
    {
        float currentTime = Time.time;
        if (currentTime - lastTime < 2f)
        {
            //Debug.Log("Less than 2 seconds have passed since last time");
            return;
        }
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

    
    public virtual void Grasp(Hand controller)
    {
        hand = controller;
        context.SendJson(new Message(transform));
        owner = true;
        lastTime = Time.time;
    }

    public virtual void Release(Hand controller)
    {
        hand = null;
        body.velocity = controller.velocity;
        Vector3 pos = transform.position;
        pos.y = 0f;
        transform.position = pos;
    }
    
    // Start is called before the first frame update
    internal virtual void Awake()
    {
        context = NetworkScene.Register(this);
        body = GetComponent<Rigidbody>();
    }

    internal void UpdateOwnership(BallController ball)
    {
        float currentTime = Time.time;
        if (currentTime - lastTimeBall < 2f)
        {
            //Debug.Log("Less than 2 seconds have passed since last time for the ball");
            return;
        }
        lastTimeBall = currentTime;
        if (owner)
        {
            ball.setOwner();
        } else
        {
            ball.removeOwner();
        }
    }

    // Update is called once per frame
    internal virtual void Update()
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
