using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubiq.Messaging;
using Ubiq.Spawning;
using Ubiq.XR;

public class MyNetworkedObject : MonoBehaviour, INetworkSpawnable, IGraspable
{

    // Spawn with peer scope ???
    public NetworkId NetworkId { get; set; }

    private Hand controller;

    NetworkContext context;

    // Start is called before the first frame update
    void Start()
    {
        context = NetworkScene.Register(this);
    }

    Vector3 lastPosition;

    // Update is called once per frame
    void Update()
    {
        if (lastPosition != transform.localPosition)
        {
            lastPosition = transform.localPosition;
            context.SendJson(new Message { position = transform.localPosition });
        }
    }

    private struct Message
    {
        public Vector3 position;
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        var m = message.FromJson<Message>();

        transform.localPosition = m.position;

        lastPosition = transform.localPosition;
    }

    private void LateUpdate() {
        if (controller)
        {
            transform.position = controller.transform.position;
            transform.rotation = controller.transform.rotation;
        }
    }

    void IGraspable.Grasp(Hand controller)
    {
        this.controller = controller;
    }

    void IGraspable.Release(Hand controller)
    {
        this.controller = null;
    }
}
