using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Ubiq.XR;

public class GraspBehaviour : MonoBehaviour, IGraspable
{
    private Hand hand;
    private Rigidbody body;
    public void Grasp(Hand controller)
    {
        hand = controller;
        body.isKinematic = true;
    }

    public void Release(Hand controller)
    {
        hand = null;
        body.isKinematic = false;
        body.velocity = controller.velocity;
    }
    
    // Start is called before the first frame update
    internal void Awake()
    {
        Debug.Log("GraspBehaviour is awake!");
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    internal void Update()
    {
        if (hand == null) return;

        body.MovePosition(hand.transform.position);
        body.MoveRotation(hand.transform.rotation);
    }
}
