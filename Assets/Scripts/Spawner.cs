using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubiq.Messaging;

public class Spawner : MonoBehaviour
{
    public GameObject itemPrefab;
    public GameObject cactus;
    public GameObject pumpkin;
    public GameObject red_flower;
    public GameObject yellow_flower;
    public GameObject purple_flower;
    public GameObject mushroom;
    public GameObject bamboo;
    public GameObject carrot;
    public GameObject turnip;
    public GameObject stone;


    NetworkContext context;

    private void Start()
    {
        context = NetworkScene.Register(this);
    }

    private struct Message
    {
        public bool spawned;
        public int item;
    }

    private GameObject ObjectFromId(int id)
    {
        if (id == 0) return cactus;
        if (id == 1) return pumpkin;
        if (id == 2) return red_flower;
        if (id == 3) return yellow_flower;
        if (id == 4) return purple_flower;
        if (id == 5) return mushroom;
        if (id == 6) return bamboo;
        if (id == 7) return carrot;
        if (id == 8) return turnip;
        if (id == 9) return stone;

        return null;
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        // Parse the message
        var m = message.FromJson<Message>();
        Debug.Log("GOT MESSAGE SPAWNING!");

        if (m.spawned)
        {
            Debug.Log("Spawning " + m.item);
            var obj = (GameObject)Instantiate(ObjectFromId(m.item), new Vector3(0, 0, 0), Quaternion.identity);
            obj.AddComponent<GraspBehaviour>();
        }
    }

    public void Spawn(int id)
    {
        var obj = (GameObject)Instantiate(ObjectFromId(id), new Vector3(0, 0, 0), Quaternion.identity);
        obj.AddComponent<GraspBehaviour>();

        Debug.Log("Sending SPAWN MESSAGE! " + id);
        context.SendJson(new Message()
        {
            spawned = true,
            item = id
        });
    }
}