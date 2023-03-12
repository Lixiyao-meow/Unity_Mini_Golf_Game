using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ubiq.XR;
using UnityEngine;
using Ubiq.Spawning;

public class ClubController : MonoBehaviour, IGraspable
{
    public GameObject ClubPrefab;

    private Quaternion relativeRotation;
    
    private Hand follow;
    private Rigidbody club;
    private Vector3 initialPosition;

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
        if (follow != null)
        {
            club.MovePosition(follow.transform.position);
            club.MoveRotation(follow.transform.rotation);
        }
    }

    public void BackToInitialPosition(){
        transform.position = initialPosition;
        club.velocity = Vector3.zero;
        club.angularVelocity = Vector3.zero;
    }
}
