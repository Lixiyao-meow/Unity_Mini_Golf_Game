using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    public float minHoleTime;

    private Rigidbody ball;
    private int putts;
    private float holeTime;
    private Vector3 lastPosition;

    void Awake(){
        ball = GetComponent<Rigidbody>();
        ball.maxAngularVelocity = 1000;
    }

    void Update(){

    }

    private void Putt(Collision collision){
        if (collision.collider.tag == "Putt"){
            putts++;
            Debug.Log("Putts: " + putts);
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


}