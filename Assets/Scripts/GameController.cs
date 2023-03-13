using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubiq.Rooms;
using Ubiq.Messaging;

public class GameController : MonoBehaviour
{
    public BallController ball;

    private List<string> PlayerList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        PlayerList.Add("Player 1");
        Debug.Log("Start: " + PlayerList.Count);
    }

    public void OnPeerUpdated(IPeer peer)
    {
        PlayerList.Add("Player 2");
        Debug.Log("update : " + PlayerList.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
