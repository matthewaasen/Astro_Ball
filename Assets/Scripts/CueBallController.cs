using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;



public class CueBallController : MonoBehaviour
{

    Rigidbody rb;
    Transform tf;
    Keyboard k;
    public LineRenderer lr;
    private RaycastHit hit;
    private Vector3 hitPoint;
    
    public TextMeshProUGUI debugText;
    public float forceIncrement;
    public float maxForce;
    private float forceAmount;
    public bool canShoot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tf = GetComponent<Transform>();
        k = Keyboard.current;
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Ensures that GameManager exists
        if (GameManager.Instance == null) return;
        //disables shooting while balls are in motion
        if(GameManager.Instance.currentState == GameState.P1Motion || GameManager.Instance.currentState == GameState.P2Motion)
        {
            canShoot = false;
        }else
        {
            canShoot = true;
        }

    
        //adds force all at once (ForceMode.Impulse) when space key is released
        if (k.spaceKey.wasReleasedThisFrame)
        {
            rb.AddForce(tf.forward * forceAmount, ForceMode.Impulse); 
            forceAmount = 0;
        }

        if (k.spaceKey.isPressed && canShoot)
        {
            if(forceAmount < maxForce)
            {
                forceAmount += forceIncrement * Time.deltaTime;
            }
        }

        UpdateLaser();
        UpdateText();
    }

    void UpdateText()
    {
        debugText.text = "Force: " + Math.Round(forceAmount, 2).ToString();
    }

    void UpdateLaser()
    {
        lr.SetPosition(0,tf.position);
        if(Physics.Raycast(tf.position, tf.forward, out hit))
        {
            hitPoint = hit.point;
        }
        float xDifference = hitPoint.x - tf.position.x;
        float zDifference = hitPoint.z - tf.position.z;
        //animates the laser by changing its end point over time
        for(int i = 0; i < 10; i++){
            lr.SetPosition(1, tf.position + new Vector3(xDifference * (i/10f), 0, zDifference * (i/10f)));
        }
    }

    public void PointToMiddle()
    {
        //points to origin and keeps current y position
        tf.LookAt(new Vector3(0, tf.position.y, 0));
    }
}
