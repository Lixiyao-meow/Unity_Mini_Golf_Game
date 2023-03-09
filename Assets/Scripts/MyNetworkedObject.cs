using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubiq.Messaging;
using Ubiq.Spawning;
using Ubiq.XR;

public class MyNetworkedObject : MonoBehaviour, IGraspable
{

    // Spawn with peer scope ???
    public NetworkId NetworkId { get; set; }
    private bool owner;
    private Hand controller;
    Vector3 lastPosition;
    NetworkContext context;

    private struct Message
    {
        public Vector3 position;

        public Message(Transform transform) {
            this.position = transform.position;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        context = NetworkScene.Register(this);
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        var data = message.FromJson<Message>();

        transform.position = data.position;

        lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (owner)
        {
            // 4. Send transform update messages if we are the current 'owner'
            context.SendJson(new Message(transform));
        }
    }

    private void LateUpdate() {
        if (controller)
        {
            transform.position = controller.transform.position;
            // transform.rotation = controller.transform.rotation;
        }
    }

    void IGraspable.Grasp(Hand controller)
    {
        owner = true;
        this.controller = controller;
    }

    void IGraspable.Release(Hand controller)
    {
        owner = false;
        this.controller = null;
    }
}
