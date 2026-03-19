using UnityEngine;

public class BallController : MonoBehaviour
{

    public AudioClip ballSound;
    public AudioClip wallSound;
    private AudioSource asource;
    public GameObject FX_CollisionSparks;
    
    public string ballColor;

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
            float volume = c.relativeVelocity.magnitude * 0.2f;
                    if(volume > 0.8f)
                    {
                        volume = 0.8f;
                    }
                    if(volume < 0.1f)
                    {
                        volume = 0.1f;
                    }
            if (c.gameObject.CompareTag("Ball"))
        {
            //this makes it so only one of the balls plays the sound
            if (gameObject.GetInstanceID() < c.gameObject.GetInstanceID())
            {
                //relative velocity is how hard the collision is
                if(c.relativeVelocity.magnitude > 0.01f){
                    
                    asource.PlayOneShot(ballSound, volume);
                    if(volume > 0.1f)
                    {
                         GameObject sparks = Instantiate(FX_CollisionSparks, c.contacts[0].point, Quaternion.identity);
                         sparks.GetComponent<ParticleSystem>().Emit((int) (volume * 20));
                         }
                  }
            }
        }
            if (c.gameObject.CompareTag("Wall"))
            {
                if(c.relativeVelocity.magnitude > 0.01f){
                    asource.PlayOneShot(wallSound, volume);
                    if(volume > 0.1f)
                    {
                         GameObject sparks = Instantiate(FX_CollisionSparks, c.contacts[0].point, Quaternion.identity);
                         sparks.GetComponent<ParticleSystem>().Emit((int) (volume * 20));
                    }
                }
            }
        }
    } 
    
}

