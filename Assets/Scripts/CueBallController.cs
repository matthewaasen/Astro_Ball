using UnityEngine;
using UnityEngine.InputSystem;

public class CueBallController : MonoBehaviour
{
    Rigidbody rb;
    Transform tf;
    Keyboard k;
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
            rb.AddForce(tf.forward, ForceMode.Impulse); //ForceMode.Impulse applies force instantly (simulating cue stick hit)
        }
    }
}
