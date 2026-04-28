using UnityEngine;

public class PocketController : MonoBehaviour
{
    public Material[] materials;
    private AudioSource asource;
    public AudioClip pocketSound;
    public AudioClip otherPocketSound;
    public AudioClip scratchSound;
    public AudioClip EightBallPocketSound;
    private MeshRenderer mr;
    private Light light;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        asource = GetComponent<AudioSource>();
        mr = GetComponent<MeshRenderer>();
        light = GetComponent<Light>();

    }

    // Update is called once per frame
    void Update()
    {
         //adjusts color (light object and material) based on each player's color (white for unassigned)
        if (GameManager.Instance.currentState == GameState.Turn0 || GameManager.Instance.currentState == GameState.Menu)
        {
            
                mr.material = materials[0];
                light.color = Color.white;
        }
        if (GameManager.Instance.currentState == GameState.P1Turn || GameManager.Instance.currentState == GameState.P1Motion)
        {
                if(GameManager.Instance.p1Color == "Red")
                {
                    mr.material = materials[2];
                    light.color = Color.red;
                }else if(GameManager.Instance.p1Color == "Blue")
                {
                    mr.material = materials[1];
                    light.color = Color.cyan;
                }
        }else if (GameManager.Instance.currentState == GameState.P2Turn || GameManager.Instance.currentState == GameState.P2Motion)
        {       if(GameManager.Instance.p1Color == "Red")
                {
                    mr.material = materials[1];
                    light.color = Color.cyan;
                }else if(GameManager.Instance.p1Color == "Blue")                
                {
                    mr.material = materials[2];
                    light.color = Color.red;
                }   
    }
    }

    private void OnTriggerEnter (Collider trigger)
    {   
        if (trigger.gameObject.CompareTag("Ball"))
        {
            string BallColor = trigger.gameObject.GetComponent<BallController>().ballColor;
            GameManager.Instance.BallSunk(trigger.gameObject.GetComponent<BallController>().ballColor, trigger.gameObject);
        
            if(trigger.gameObject.GetComponent<BallController>().ballColor == GameManager.Instance.p1Color)
            {
                asource.PlayOneShot(pocketSound, 0.8f);
                GameManager.Instance.p1Left--;
            }else if(trigger.gameObject.GetComponent<BallController>().ballColor == GameManager.Instance.p2Color)
            {
                asource.PlayOneShot(otherPocketSound, 0.8f);
                GameManager.Instance.p2Left--;
            }
            }
        if (trigger.gameObject.CompareTag("CueBall"))
        {
            GameManager.Instance.scratch = true;
            GameManager.Instance.BallSunk(trigger.gameObject.GetComponent<BallController>().ballColor, trigger.gameObject);
            asource.PlayOneShot(scratchSound, 0.8f);
        }
        if (trigger.gameObject.CompareTag("8Ball"))
        {
            asource.PlayOneShot(EightBallPocketSound, 1.0f);
            //P1 Sinks 8Ball
            if(GameManager.Instance.currentState == GameState.P1Motion)
            {
                if(GameManager.Instance.p1Left == 0)
                {
                    GameManager.Instance.BallSunk(trigger.gameObject.GetComponent<BallController>().ballColor, trigger.gameObject);
                    GameManager.Instance.currentState = GameState.P1Wins;
                }else
                {                   
                    GameManager.Instance.BallSunk(trigger.gameObject.GetComponent<BallController>().ballColor, trigger.gameObject);
                    GameManager.Instance.currentState = GameState.P2Wins;
                }
            
            }
            //P1 Sinks 8Ball
            if(GameManager.Instance.currentState == GameState.P2Motion)
            {
                if(GameManager.Instance.p2Left == 0)
                {
                    GameManager.Instance.BallSunk(trigger.gameObject.GetComponent<BallController>().ballColor, trigger.gameObject);
                    GameManager.Instance.currentState = GameState.P2Wins;
                }else
                {                   
                    GameManager.Instance.BallSunk(trigger.gameObject.GetComponent<BallController>().ballColor, trigger.gameObject);
                    GameManager.Instance.currentState = GameState.P1Wins;
                }
            
            }
        }
}
}
