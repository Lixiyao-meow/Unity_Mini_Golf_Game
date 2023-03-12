using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ubiq.XR;
using UnityEngine;
using Ubiq.Spawning;

public class ClubController : MonoBehaviour, IGraspable
{
    public GameObject ClubPrefab;

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
    }

    public void Release(Hand controller)
    {
        follow = null;
    }

    private void Update()
    {
        if (follow != null)
        {
            transform.position = follow.transform.position;
            transform.rotation = follow.transform.rotation;
            club.isKinematic = true;
        }
        else
        {
            club.isKinematic = false;
        }
    }

    public void BackToInitialPosition(){
        transform.position = initialPosition;
        club.velocity = Vector3.zero;
        club.angularVelocity = Vector3.zero;
    }
}
