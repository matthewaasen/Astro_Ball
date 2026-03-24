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
            GameManager.Instance.BallSunk(trigger.gameObject.GetComponent<BallController>().ballColor, trigger.gameObject);
        }
        if (trigger.gameObject.CompareTag("CueBall"))
        {
            
        }
    }
}
