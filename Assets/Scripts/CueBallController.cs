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

    public AudioClip cueSound;
    private AudioSource asource;
    public ParticleSystem cueSparks;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tf = GetComponent<Transform>();
        k = Keyboard.current;
        lr = GetComponent<LineRenderer>();
        asource = GetComponent<AudioSource>();
        asource.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Ensures that GameManager exists
        if (GameManager.Instance == null) return;
        //disables shooting while balls are in motion
        if(GameManager.Instance.currentState == GameState.P1Turn || GameManager.Instance.currentState == GameState.P2Turn || GameManager.Instance.currentState == GameState.Turn0)
        {
            canShoot = true;
            lr.enabled = true;
        }else
        {
            canShoot = false;
            lr.enabled = false;
        }

    
        //adds force all at once (ForceMode.Impulse) when space key is released
        if (k.spaceKey.wasReleasedThisFrame && canShoot)
        {
            asource.PlayOneShot(cueSound, 1.0f);
            rb.AddForce(tf.forward * forceAmount, ForceMode.Impulse); 
            cueSparks.Emit(10); 
            forceAmount = 0;
            
            
        }

        if (canShoot)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), Camera.main.transform.position.y - tf.position.y));
            Vector3 direction = mousePos - tf.position;
            transform.rotation = Quaternion.LookRotation(direction);
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

    public void Scratch()
    {
        tf.position = new Vector3(0, tf.position.y, -0.7f);
        //disables the ball's physics and makes it invisible
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            GameManager.Instance.firstHit = collision.gameObject.GetComponent<BallController>().ballColor;
        }
    }
}
