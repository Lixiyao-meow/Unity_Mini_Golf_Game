using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ubiq.XR;
using UnityEngine;
using Ubiq.Spawning;
using Ubiq.Messaging;

public class ClubController : MonoBehaviour, IGraspable
{
    public GameObject ClubPrefab;

    private Quaternion relativeRotation;
    
    private Hand follow;
    private Rigidbody club;
    private Vector3 initialPosition;

    NetworkContext context;
    Vector3 lastPosition;

    void Start()
    {
        context = NetworkScene.Register(this);
    }

    private void Awake()
    {
        club = GetComponent<Rigidbody>();
        initialPosition = club.position;
    }

    public void Grasp(Hand controller)
    {
        follow = controller;
        club.isKinematic = true;
        // Vector (A -> B) => (B - A)
        relativeRotation =  transform.rotation;
    }

    public void Release(Hand controller)
    {
        follow = null;
        club.isKinematic = false;
        club.velocity = controller.velocity; // Set the club's velocity to the VR hand
    }

    private void Update()
    {
        if(lastPosition != transform.localPosition)
        {
            lastPosition = transform.localPosition;
            context.SendJson(new Message()
            {
                position = transform.localPosition
            });
        }

        if (follow != null)
        {
            club.MovePosition(follow.transform.position);
            club.MoveRotation(follow.transform.rotation);
        }
    }

    private struct Message
    {
        public Vector3 position;
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        // Parse the message
        var m = message.FromJson<Message>();

        // Use the message to update the Component
        transform.localPosition = m.position;

        // Make sure the logic in Update doesn't trigger as a result of this message
        lastPosition = transform.localPosition;
    }

    public void BackToInitialPosition(){
        transform.position = initialPosition;
        club.velocity = Vector3.zero;
        club.angularVelocity = Vector3.zero;
    }
}
