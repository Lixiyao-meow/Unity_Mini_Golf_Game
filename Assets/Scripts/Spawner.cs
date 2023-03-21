using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubiq.Messaging;

public class Spawner : MonoBehaviour
{
    public GameObject itemPrefab;

    NetworkContext context;

    private void Start()
    {
        context = NetworkScene.Register(this);
    }

    private struct Message
    {
        public bool spawned;
        public string item;
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        // Parse the message
        var m = message.FromJson<Message>();
        Debug.Log("GOT MESSAGE!");

        if (m.spawned)
        {
            Debug.Log("Spawning " + m.item);
            var obj = (GameObject)Instantiate(Resources.Load("../Prefabs/" + m.item), new Vector3(0, 0, 0), Quaternion.identity);
            obj.AddComponent<GraspBehaviour>();
        }
    }

    public void Spawn(GameObject item)
    {
        var obj = (GameObject)Instantiate(item, new Vector3(0, 0, 0), Quaternion.identity);
        obj.AddComponent<GraspBehaviour>();

        Debug.Log("Sending SPAWN MESSAGE! " + item.name);
        context.SendJson(new Message()
        {
            spawned = true,
            item = item.name
        });
    }
}
