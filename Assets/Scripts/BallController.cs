using UnityEngine;

public class BallController : MonoBehaviour
{

    public AudioClip collisionSound;
    private AudioSource asource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        asource = GetComponent<AudioSource>();
        asource.playOnAwake = false;
        asource.spatialBlend = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision c)
    {
        //prevents sound from occuring before first shot
        if(GameManager.Instance.currentState != GameState.Turn0)
        {
            if (c.gameObject.CompareTag("Ball"))
        {
            //this makes it so only one of the balls plays the sound
            if (gameObject.name.CompareTo(c.gameObject.name) < 0)
            {
                //relative velocity is how hard the collision is
                if(c.relativeVelocity.magnitude > 0.01f){
                    asource.PlayOneShot(collisionSound, 1.0f);
                }
            
            }
        }
    } 
        }
       
}
