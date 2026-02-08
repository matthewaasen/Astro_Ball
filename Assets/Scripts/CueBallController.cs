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
    RaycastHit hit;
    
    public TextMeshProUGUI debugText;
    public float forceIncrement;
    public float maxForce;
    private float forceAmount;
    public bool canShoot;
    public float angleIncrement;

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

        if (k.leftArrowKey.isPressed && canShoot)
        {
            tf.Rotate(0, -angleIncrement * Time.deltaTime, 0);
        }   

        if (k.rightArrowKey.isPressed && canShoot)
        {
            tf.Rotate(0, angleIncrement * Time.deltaTime, 0);
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
            lr.SetPosition(1, hit.point);
        }
    }

    public void PointToMiddle()
    {
        //points to origin and keeps current y position
        tf.LookAt(new Vector3(0, tf.position.y, 0));
    }
}
