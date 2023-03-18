using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubiq.Messaging;
using Ubiq.Spawning;
using Ubiq.XR;

public class MyNetworkedObject : MonoBehaviour
{

    public NetworkId NetworkId { get; set; }
    private bool owner;
    Vector3 lastPosition;
    NetworkContext context;

    private struct Message
    {
        public Vector3 position;
        public Quaternion rotation;

        public Message(Transform transform) {
            this.position = transform.position;
            this.rotation = transform.rotation;
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
        transform.rotation = data.rotation;

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
}
