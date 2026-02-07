using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;



public class CueBallController : MonoBehaviour
{

    Rigidbody rb;
    Transform tf;
    Keyboard k;

    public TextMeshProUGUI debugText;
    public float forceIncrement;
    public float maxForce;
    private float forceAmount;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tf = GetComponent<Transform>();
        k = Keyboard.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (k.spaceKey.wasReleasedThisFrame)
        {
            rb.AddForce(tf.forward * forceAmount, ForceMode.Impulse); //ForceMode.Impulse applies force instantly (simulating cue stick hit)
            forceAmount = 0;
        }

        if (k.spaceKey.isPressed)
        {
            if(forceAmount < maxForce)
            {
                forceAmount += forceIncrement * Time.deltaTime;
            }
        }
        
        UpdateText();
    }

    void UpdateText()
    {
        debugText.text = "Force: " + Math.Round(forceAmount, 2).ToString();
    }
}
