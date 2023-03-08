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
    private Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
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
            body.isKinematic = true;
        }
        else
        {
            body.isKinematic = false;
        }
    }
}
