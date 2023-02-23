using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    public float maxPower;
    public float changeAngleSpeed;
    public float lineLength;
    public Slider powerSlider;
    public TextMeshProUGUI puttCountLaber;
    public float minHoleTime;

    private LineRenderer line;
    private Rigidbody ball;
    private float angle;
    private float powerUpTime;
    private float power;
    private int putts;
    private float holeTime;
    private Vector3 lastPosition;

    void Awake(){
        ball = GetComponent<Rigidbody>();
        line = GetComponent<LineRenderer>();
        ball.maxAngularVelocity = 1000;
    }

    void Update(){
        // if ball stops moving, actions below are allowed
        if (ball.velocity.magnitude < 0.01f){
            if (Input.GetKey(KeyCode.A)){
                angle -= changeAngleSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D)){
                angle += changeAngleSpeed * Time.deltaTime;
            }
            if (Input.GetKeyUp(KeyCode.Space)){
                Putt();
            }
            if (Input.GetKey(KeyCode.Space)){
                PowerUp();
            }
            UpdateLinePositions();
        }
        // if ball is still moving
        else{
            line.enabled = false;

        }
    }

    private void UpdateLinePositions(){
        if (holeTime == 0) {line.enabled = true;}

        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * lineLength);
    }

    private void Putt(){
        lastPosition = transform.position;
        ball.AddForce(Quaternion.Euler(0, angle, 0) * Vector3.forward * maxPower * power, ForceMode.Impulse);
        power = 0;
        powerSlider.value = 0;
        powerUpTime = 0;
        putts++;
        puttCountLaber.text = putts.ToString();
    }

    private void PowerUp(){
        powerUpTime += Time.deltaTime;
        power = Mathf.PingPong(powerUpTime, 1);
        powerSlider.value = power;
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
