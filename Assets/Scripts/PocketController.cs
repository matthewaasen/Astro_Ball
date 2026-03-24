using UnityEngine;

public class PocketController : MonoBehaviour
{
    public Material[] materials;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
            GameManager.Instance.BallSunk(trigger.gameObject.GetComponent<BallController>().ballColor, trigger.gameObject);
        }
        if (trigger.gameObject.CompareTag("CueBall"))
        {
            
        }

       
}
}
