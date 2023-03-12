using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Ubiq.XR;
using UnityEngine;
using Ubiq.Spawning;

public class BallController : MonoBehaviour, IGraspable
{
    public float minHoleTime;
    public Text puttsCounter1;
    public Text puttsCounter2;
    public ClubController club;

    private Rigidbody ball;
    private Hand follow;
    private int putts;
    private float holeTime;
    private Vector3 lastPosition; // get ball back when out of bounce
    private Vector3 initialPosition; // put ball back after one round

    void Awake(){
        ball = GetComponent<Rigidbody>();
        ball.maxAngularVelocity = 1000;
        initialPosition = ball.position;        
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
            ball.isKinematic = true;
        }
        else
        {
            ball.isKinematic = false;
        }
    }

    private void Putt(Collision collision){
        if (collision.collider.tag == "Putt"){
            putts++;
            puttsCounter1.text = putts.ToString();
        }
    }

    private void OnTriggerStay(Collider other){
        if (other.tag == "Hole"){
            CountHoleTime();
        }
    }

    private void CountHoleTime(){
        holeTime += Time.deltaTime;
        // player has finished, move to the next player
        if (holeTime >= minHoleTime){
            Debug.Log("I'm in the hole and it took me " + putts + " putts to get in.");
            holeTime = 0;
            putts = 0;
            BackToInitialPosition(); // move ball back
            club.BackToInitialPosition(); // move club back
        }

    }

    private void OnTriggerExit(Collider other){
        if (other.tag == "Hole"){
            LeftHole();
        }
    }

    private void LeftHole(){
        holeTime = 0;
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.collider.tag == "Out Of Bounds"){
            transform.position = lastPosition;
            ball.velocity = Vector3.zero;
            ball.angularVelocity = Vector3.zero;
        }
    }

    private void BackToInitialPosition(){
        transform.position = initialPosition;
        ball.velocity = Vector3.zero;
        ball.angularVelocity = Vector3.zero;
    }


}