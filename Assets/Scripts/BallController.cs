using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Ubiq.XR;
using Ubiq.Spawning;
using Ubiq.Messaging;

public class BallController : MonoBehaviour, IGraspable
{
    public float minHoleTime;
    public Text puttsCounter;
    public ClubController club;

    private Rigidbody ball;
    private Hand follow;
    private float puttCoolDown = 1.0f;
    private float lastPuttTime = 0.0f;
    private int putts;
    private float holeTime;
    private Vector3 lastPosition; // get ball back when out of bounce
    private Vector3 initialPosition; // put ball back after one round
    NetworkContext context;
    Vector3 lastNetworkedPosition;

    void Start()
    {
        context = NetworkScene.Register(this);
    }

    void Awake(){
        ball = GetComponent<Rigidbody>();
        ball.maxAngularVelocity = 1000;
        initialPosition = ball.position;
        lastPosition = ball.position;   
    }

    public void Grasp(Hand controller)
    {
        follow = controller;
        ball.isKinematic = true;
    }

    public void Release(Hand controller)
    {
        follow = null;
        ball.isKinematic = false;
        ball.velocity = controller.velocity;
    }

    private void Update()
    {
        if (lastNetworkedPosition != transform.localPosition)
        {
            lastNetworkedPosition = transform.localPosition;
            context.SendJson(new Message()
            {
                position = transform.localPosition
            });
        }
        if (follow != null)
        {
            ball.MovePosition(follow.transform.position);
            ball.MoveRotation(follow.transform.rotation);
            // transform.position = follow.transform.position;
            // transform.rotation = follow.transform.rotation;
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
        lastNetworkedPosition = transform.localPosition;
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
            StartAnotherRound();
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
        if(collision.impulse.magnitude <= Mathf.Epsilon) return;

        // if ball fall out of bounds
        if (collision.collider.tag == "Out Of Bounds"){
            BackToLastPosition();
        }

        // if club collide with the ball
        if (collision.collider.tag == "Putt"){
            if (Time.time > lastPuttTime + puttCoolDown){
                putts++;
                lastPuttTime = Time.time;
                lastPosition = ball.position;
                puttsCounter.text = putts.ToString();
            }
        }
    }

    private void BackToInitialPosition(){
        transform.position = initialPosition;
        ball.velocity = Vector3.zero;
        ball.angularVelocity = Vector3.zero;
    }

    private void BackToLastPosition(){
        transform.position = lastPosition;
        ball.velocity = Vector3.zero;
        ball.angularVelocity = Vector3.zero;
    }

    private void StartAnotherRound(){
        holeTime = 0;
        putts = 0;
        puttsCounter.text = putts.ToString();
        BackToInitialPosition(); // move ball back
        club.BackToInitialPosition(); // move club back
    }
}