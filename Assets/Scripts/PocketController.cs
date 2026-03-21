using UnityEngine;

public class PocketController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter (Collider trigger)
    {
        if (trigger.gameObject.CompareTag("Ball"))
        {
            //destroys the ball that collides with the pocket
            Destroy(trigger.gameObject);
        }
    }
}
