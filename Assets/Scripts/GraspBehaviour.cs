using UnityEngine;
using Ubiq.XR;
using Ubiq.Messaging;

public class GraspBehaviour : MonoBehaviour, IGraspable
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
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage msg)
    {
        float currentTime = Time.time;
        if (currentTime - lastTime < 2f)
        {
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
        body.isKinematic = true;
        context.SendJson(new Message(transform));
        owner = true;
        lastTime = Time.time;
    }

    public virtual void Release(Hand controller)
    {
        hand = null;
        body.isKinematic = false;
        body.velocity = controller.velocity;
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
