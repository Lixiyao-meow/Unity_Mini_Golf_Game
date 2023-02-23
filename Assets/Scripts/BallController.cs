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

    private LineRenderer line;
    private Rigidbody ball;
    private float angle;
    private float powerUpTime;
    private float power;
    private int putts;

    void Awake(){
        ball = GetComponent<Rigidbody>();
        line = GetComponent<LineRenderer>();
        ball.maxAngularVelocity = 1000;
    }

    void Update(){
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

    private void UpdateLinePositions(){
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * lineLength);
    }

    private void Putt(){
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

}
