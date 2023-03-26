using UnityEngine;

public class BallController : GraspBehaviour
{
    public float minHoleTime;
    public ScoreBoard scoreBoard;

    private Rigidbody ball;
    private float puttCoolDown = 1.0f;
    private float lastPuttTime = 0.0f;
    private float holeTime;
    private Vector3 lastPosition; // get ball back when out of bounce
    private Vector3 initialPosition; // put ball back after one roundVector3 lastNetworkedPosition;

    void Start()
    {
    }

    internal override void Awake(){
        base.Awake();
        ball = GetComponent<Rigidbody>();
        ball.maxAngularVelocity = 1000;
        initialPosition = ball.position;
        lastPosition = ball.position;   
    }

    internal override void Update()
    {
        base.Update();
        
        // put ball back while ghost collision
        if (ball.position.y < -1){
            BackToLastPosition();
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
                scoreBoard.AddPutt();
                lastPuttTime = Time.time;
                lastPosition = ball.position;
            }
        }
    }

    private void BackToInitialPosition(){
        transform.position = initialPosition;
        ball.velocity = Vector3.zero;
        ball.angularVelocity = Vector3.zero;
        lastPosition = initialPosition;
    }

    private void BackToLastPosition(){
        transform.position = lastPosition;
        ball.velocity = Vector3.zero;
        ball.angularVelocity = Vector3.zero;
    }

    private void StartAnotherRound(){
        holeTime = 0;
        scoreBoard.ResetPutts();
        BackToInitialPosition(); // move ball back
    }
}