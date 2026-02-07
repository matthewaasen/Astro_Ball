using UnityEngine;
using TMPro;

 public enum GameState
    {
        Turn0, P1Turn, P1Motion, P2Turn, P2Motion, GameOver
    }
    
public class GameManager : MonoBehaviour
{
    public CueBallController cueBallController;
    //allows other scripts to reference this one
    public static GameManager Instance;
    public GameState currentState;
    private Rigidbody[] ballRigidbodies;
    public float motionThreshold = 0.01f; //threshold for if a ball is considered moving
    public TextMeshProUGUI gameStateText;
    
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //Collects all Rigidbodies from balls to use for game states
        GameObject[] ballObjects = GameObject.FindGameObjectsWithTag("Ball");
        ballRigidbodies = new Rigidbody[ballObjects.Length];
        for (int i = 0; i < ballObjects.Length; i++)
        {
            ballRigidbodies[i] = ballObjects[i].GetComponent<Rigidbody>();
        }
        //sets up the laser guide
        cueBallController.PointToMiddle();
        cueBallController.lr.enabled = true;

        currentState = GameState.Turn0;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == GameState.Turn0)
        {
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
                cueBallController.PointToMiddle();
                cueBallController.lr.enabled = true;
                currentState = GameState.P2Turn;
            }
        }
        if(currentState == GameState.P2Motion)
        {
            if(!BallMoving())
            {
                cueBallController.PointToMiddle();
                cueBallController.lr.enabled = true;
                currentState = GameState.P1Turn;
            }
        }
        if(currentState == GameState.P1Turn)
        {
            if (BallMoving())
            {
                cueBallController.lr.enabled = false;
                currentState = GameState.P1Motion;
            }
        }
        if(currentState == GameState.P2Turn)
        {
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
        gameStateText.text = "Game State: " + currentState.ToString();
    }
    private bool BallMoving()
    {
        //checks each rigidbody to see if any ball is moving
        for(int i = 0; i < ballRigidbodies.Length; i++)
        {
            if(ballRigidbodies[i].linearVelocity.magnitude > motionThreshold)
            {
                return true;
            }
        }
        return false;
    }
}
