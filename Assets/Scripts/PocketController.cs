using UnityEngine;

public class PocketController : MonoBehaviour
{
    public Material[] materials;
    private AudioSource asource;
    public AudioClip pocketSound;
    public AudioClip otherPocketSound;
    public AudioClip scratchSound;
    public AudioClip EightBallPocketSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        asource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
         //adjusts color
        if (GameManager.Instance.currentState == GameState.Turn0 || GameManager.Instance.currentState == GameState.Menu)
        {
                //sets to Material 0 and the light to white
                GetComponent<MeshRenderer>().material = materials[0];
                GetComponent<Light>().color = Color.white;
        }else if (GameManager.Instance.currentState == GameState.P1Turn || GameManager.Instance.currentState == GameState.P1Motion)
        {
                //sets to Material 1 and the light to blue
                if(GameManager.Instance.p1Color == "Red")
                {
                    GetComponent<MeshRenderer>().material = materials[2];
                    GetComponent<Light>().color = Color.red;
                }else if(GameManager.Instance.p1Color == "Blue")
                {
                    GetComponent<MeshRenderer>().material = materials[1];
                    GetComponent<Light>().color = Color.cyan;
                }
        }else if (GameManager.Instance.currentState == GameState.P2Turn || GameManager.Instance.currentState == GameState.P2Motion)
        {       if(GameManager.Instance.p1Color == "Red")
                {
                    GetComponent<MeshRenderer>().material = materials[1];
                    GetComponent<Light>().color = Color.cyan;
                }else if(GameManager.Instance.p1Color == "Blue")                
                {
                    GetComponent<MeshRenderer>().material = materials[2];
                    GetComponent<Light>().color = Color.red;
                }   
    }
    }

    private void OnTriggerEnter (Collider trigger)
    {
        if (trigger.gameObject.CompareTag("Ball"))
        {
            //NEEDS TO BE FIXED
            //
            //
            //
            if(trigger.gameObject.GetComponent<BallController>().ballColor == "Red")
            {
                GameManager.Instance.p2Left--;
            }else if(trigger.gameObject.GetComponent<BallController>().ballColor == "Blue")
            {
                GameManager.Instance.p1Left--;
            }
            GameManager.Instance.BallSunk(trigger.gameObject.GetComponent<BallController>().ballColor, trigger.gameObject);
        }
        if (trigger.gameObject.CompareTag("CueBall"))
        {
            GameManager.Instance.scratch = true;
            GameManager.Instance.BallSunk(trigger.gameObject.GetComponent<BallController>().ballColor, trigger.gameObject);
            asource.PlayOneShot(scratchSound, 1.0f);
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
