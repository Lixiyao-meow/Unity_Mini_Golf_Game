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

    public GameObject patch0;
    public GameObject patch1;
    public GameObject patch2;
    public GameObject patch3;
    public GameObject patch4;
    public GameObject patch5;
    public GameObject patch6;
    public GameObject patch7;
    public GameObject patch8;
    public GameObject patch9;
    public GameObject patch10;
    public GameObject patch11;
    public GameObject patch12;
    public GameObject patch13;
    public GameObject patch14;
    public GameObject patch15;
    public GameObject patch16;
    public GameObject patch17;
    public GameObject patch18;


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
        if (id == 10) return patch0;
        if (id == 11) return patch1;
        if (id == 12) return patch2;
        if (id == 13) return patch3;
        if (id == 14) return patch4;
        if (id == 15) return patch5;
        if (id == 16) return patch6;
        if (id == 17) return patch7;
        if (id == 18) return patch8;
        if (id == 19) return patch9;
        if (id == 20) return patch10;
        if (id == 21) return patch11;
        if (id == 22) return patch12;
        if (id == 23) return patch13;
        if (id == 24) return patch14;
        if (id == 25) return patch15;
        if (id == 26) return patch16;
        if (id == 27) return patch17;
        if (id == 28) return patch18;

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