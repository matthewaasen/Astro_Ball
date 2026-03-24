using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public enum GameState
    {
        Turn0, P1Turn, P1Motion, P2Turn, P2Motion, GameOver, Menu
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

    //game mechanics
    private bool blueSunk;
    private bool redSunk;
    public string p1Color;
    private string p2Color;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = GameState.Menu;
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
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState != GameState.Menu)
        {
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
            //transition between P1 motion and P2 turn
            if(!BallMoving())
            {
                //check if P1 goes again (sunk correct color and not other color)
                if(p1Color == "Blue" && blueSunk && !redSunk || p1Color == "Red" && redSunk && !blueSunk)
                {
                    cueBallController.PointToMiddle();
                    cueBallController.lr.enabled = true;
                    currentState = GameState.P1Turn;
                    return;
                }
                else
                {
                    cueBallController.PointToMiddle();
                    cueBallController.lr.enabled = true;
                    currentState = GameState.P2Turn;
                    asource.PlayOneShot(turnChangeSound, 0.5f);
                }
                
            }
        }
        if(currentState == GameState.P2Motion)
        {
            if(!BallMoving())
            {
                if(p2Color == "Blue" && blueSunk && !redSunk || p2Color == "Red" && redSunk && !blueSunk)
                {
                    cueBallController.PointToMiddle();
                    cueBallController.lr.enabled = true;
                    currentState = GameState.P2Turn;
                    return;
                }
                else
                {
                    cueBallController.PointToMiddle();
                    cueBallController.lr.enabled = true;
                    currentState = GameState.P1Turn;
                    asource.PlayOneShot(turnChangeSound, 0.5f);
                }
            }
        }
        if(currentState == GameState.P1Turn)
        {
            //resets sunk status
            blueSunk = false;
            redSunk = false;
            if (BallMoving())
            {
                cueBallController.lr.enabled = false;
                currentState = GameState.P1Motion;
            }
        }
        if(currentState == GameState.P2Turn)
        {
            //resets sunk status
            blueSunk = false;
            redSunk = false;
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

    private void updateGameStateText()
    {
        gameStateText.text = "Game State: " + currentState.ToString() + " P1 Color: " + p1Color + " P2 Color: " + p2Color;
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
        
        if (color == "Blue")
        {
            blueSunk = true;
        }
        else if (color == "Red")
        {
            redSunk = true;
        }
        
        //sets player color if it's not yet set
        if((currentState == GameState.P1Motion) || (currentState == GameState.P2Motion))
        {
            if(currentState == GameState.P1Motion)
            {
            if(p1Color == "None")
            {
                if(blueSunk)
                {
                    p1Color = "Blue";
                    p2Color = "Red";
                }
                else if(redSunk)
                {
                    p1Color = "Red";
                    p2Color = "Blue";
                }
            }
            }
            if(currentState == GameState.P2Motion)
            {
            if(p1Color == "None")
            {
                if(blueSunk)
                {
                    p2Color = "Blue";
                    p1Color = "Red";
                }
                else if(redSunk)
                {
                    p2Color = "Red";
                    p1Color = "Blue";
                }
            }
            }
        }
        else if(currentState == GameState.P2Motion || currentState == GameState.P2Turn)
        {
        if(p1Color == "None")
        {
            if(blueSunk)
            {
                p1Color = "Blue";
                p2Color = "Red";
            }
            else if(redSunk)
            {
                p1Color = "Red";
                p2Color = "Blue";
            }
        }
        

    }
}
}
