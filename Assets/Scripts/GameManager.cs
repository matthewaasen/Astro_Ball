using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public enum GameState
    {
        Turn0, P1Turn, P1Motion, P2Turn, P2Motion, P1Wins, P2Wins, Menu
    }
    
public class GameManager : MonoBehaviour
{
    public Camera mainCamera;
    public Camera menuCamera;
    public CueBallController cueBallController;
    //allows other scripts to reference this one
    public static GameManager Instance;
    public GameState currentState;
    private Rigidbody[] ballRigidbodies;
    public float motionThreshold = 0.01f; //threshold for if a ball is considered moving
    public TextMeshProUGUI gameStateText;
    private AudioSource asource;
    public AudioClip turnChangeSound;
    public bool scratch;
    public TextMeshProUGUI turnText;
    //game mechanics
    private bool blueSunk;
    private bool redSunk;
    private string firstSunk;
    public string p1Color;
    private string p2Color;
    public string firstHit;
    public int p1Left;
    public int p2Left;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = GameState.Menu;
        p1Left = 7;
        p2Left = 7;
        menuCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        //Collects all Rigidbodies from balls to use for game states
        GameObject[] ballObjects = GameObject.FindGameObjectsWithTag("Ball");
        ballRigidbodies = new Rigidbody[ballObjects.Length];
        for (int i = 0; i < ballObjects.Length; i++)
        {
            ballRigidbodies[i] = ballObjects[i].GetComponent<Rigidbody>();
        }

        asource = GetComponent<AudioSource>();
        asource.playOnAwake = false;
        //sets up the laser guide
        cueBallController.PointToMiddle();
        cueBallController.lr.enabled = true;
        blueSunk = false;
        redSunk = false;
        scratch = false;
        firstSunk = "None";
        firstHit = "None";
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState != GameState.Menu)
        {
            turnText.gameObject.SetActive(false);
            if (Keyboard.current.cKey.isPressed){
            menuCamera.gameObject.SetActive(true);
            mainCamera.gameObject.SetActive(false);

            }else
            {
                mainCamera.gameObject.SetActive(true);
                menuCamera.gameObject.SetActive(false);
            }
        }
        

        if(currentState == GameState.Turn0)
        {
            p1Color = "None";
            p2Color = "None";
            if (BallMoving())
            {
                cueBallController.lr.enabled = false;
                currentState = GameState.P1Motion;
            }
        }
        if(currentState == GameState.P1Motion)
        {
            cueBallController.lr.enabled = false;
            //transition between P1 motion and P2 turn
            if(!BallMoving())
            {
                if (scratch)
                {
                    currentState = GameState.P2Turn;
                    return;
                }
                //p1 Foul
                if(p1Color != "None" && p1Color != firstHit)
                {
                    cueBallController.PointToMiddle();
                    cueBallController.lr.enabled = true;
                    currentState = GameState.P2Turn;
                    asource.PlayOneShot(turnChangeSound, 0.5f);
                    return;
                }
                //p1 go again
                if(p1Color == "Red" && redSunk || p1Color == "Blue" && blueSunk)
                {
                    cueBallController.PointToMiddle();
                    cueBallController.lr.enabled = true;
                    currentState = GameState.P1Turn;
                    return;
                }else{ //p1 turn over, switch to p2
                    cueBallController.PointToMiddle();
                    cueBallController.lr.enabled = true;
                    currentState = GameState.P2Turn;
                    asource.PlayOneShot(turnChangeSound, 0.5f);
                    FreezeBalls();
                }
                
            }
        }
        if(currentState == GameState.P2Motion)
        {
            cueBallController.lr.enabled = false;
            if(!BallMoving())
            {
                if (scratch)
                {
                    currentState = GameState.P1Turn;
                    return;
                }
                //p2 Foul
                if(p2Color != "None" && p2Color != firstHit)
                {
                    cueBallController.PointToMiddle();
                    cueBallController.lr.enabled = true;
                    currentState = GameState.P1Turn;
                    asource.PlayOneShot(turnChangeSound, 0.5f);
                    return;
                }
                //p2 go again
                if(p2Color == "Red" && redSunk || p2Color == "Blue" && blueSunk)
                {
                    cueBallController.PointToMiddle();
                    cueBallController.lr.enabled = true;
                    currentState = GameState.P2Turn;
                    return;
                }else{ //p2 turn over, switch to p1
                    cueBallController.PointToMiddle();
                    cueBallController.lr.enabled = true;
                    currentState = GameState.P1Turn;
                    asource.PlayOneShot(turnChangeSound, 0.5f);
                    FreezeBalls();
                }

            }
        }
        if(currentState == GameState.P1Turn)
        {
            
            //if cue ball was hit in by previous player
            if (scratch)
            {
                cueBallController.Scratch();
                scratch = false;
            }
            //resets sunk status
            blueSunk = false;
            redSunk = false;
            firstSunk = "None";
            firstHit = "None";

            if (BallMoving())
            {
                cueBallController.lr.enabled = false;
                currentState = GameState.P1Motion;
            }
        }
        if(currentState == GameState.P2Turn)
        {
            //if cue ball was hit in by previous player
            if (scratch)
            {
                cueBallController.Scratch();
                scratch = false;
            }
            //resets sunk status
            blueSunk = false;
            redSunk = false;
            firstSunk = "None";
            firstHit = "None";
            if (BallMoving())
            {
                cueBallController.lr.enabled = false;
                currentState = GameState.P2Motion;
            }
        }

        updateGameStateText();
    }


    //ensures only one instance of this script runs (singleton)
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void FreezeBalls()
    {
        for(int i = 0; i < ballRigidbodies.Length; i++)
        {
            if(ballRigidbodies[i] == null)
            {
                continue; //skip if ball has been destroyed
            } 
            ballRigidbodies[i].linearVelocity = Vector3.zero;
            ballRigidbodies[i].angularVelocity = Vector3.zero;
        }
    }
    private void updateGameStateText()
    {
        gameStateText.text = "Game State: " + currentState.ToString() + " P1 Color: " + p1Color + " P2 Color: " + p2Color;
        if(currentState == GameState.Menu)
        {
            turnText.gameObject.SetActive(false);
        }
        else
        {
            turnText.gameObject.SetActive(true);
        }
        if(currentState == GameState.P1Motion || currentState == GameState.P1Turn || currentState == GameState.Turn0)
        {
            turnText.text = "Player 1's Turn";
        }
        else
        {
            turnText.text = "Player 2's Turn";
        }
    }
    private bool BallMoving()
    {
        //checks each rigidbody to see if any ball is moving
        for(int i = 0; i < ballRigidbodies.Length; i++)
        {
            if(ballRigidbodies[i] == null)
            {
                continue; //skip if ball has been destroyed
            } 
            if(ballRigidbodies[i].linearVelocity.magnitude > motionThreshold)
            {
                return true;
            }
        }
        return false;
    }

    public void StartGame()
    {
        menuCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        currentState = GameState.Turn0;
    }

    public void BallSunk(string color, GameObject ballObject)
    {
        ballObject.GetComponent<BallController>().SinkBall();
        
        if(firstSunk == "None")
        {
            firstSunk = color;
        }
        //sets redSunk and blueSunk
        if(color == "Red")
        {
            redSunk = true;
        }else if(color == "Blue")
        {           
            blueSunk = true;
        }
        //assigns colors to players if not already assigned
        if(p1Color == "None" && currentState == GameState.P1Motion)
        { 
            p1Color = color;
            if(color == "Blue")
            {
                p2Color = "Red";
            }else{
                p2Color = "Blue";
            }    
        }else if(p1Color == "None" && currentState == GameState.P2Motion)
        {
            p2Color = color;
            if(color == "Blue")
            {
                p1Color = "Red";
            }else{
                p1Color = "Blue";
            }    
        }
    }
}
