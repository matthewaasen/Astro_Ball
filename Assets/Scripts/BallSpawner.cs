using System;
using Unity.VisualScripting;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject[] ballObjects = new GameObject[15];
    private int[] ballSetMaterials = new int[15];
    public Material[] ballMaterials = new Material[3];
    public Vector3[] ballPositions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Creates the balls
        for(int i = 0; i < 15; i++)
        {
            //position
            ballObjects[i] = Instantiate(ballPrefab, ballPositions[i]+ new Vector3(0, 0, 0.4f), Quaternion.identity, transform);
            ballObjects[i].transform.position = new Vector3(ballObjects[i].transform.position.x, ballObjects[i].transform.position.y, ballObjects[i].transform.position.z + 0.67f);
        }
        //Colors the balls
        int reds = 0;
        int blues = 0;
        for(int i = 0; i < 15; i++)
        {
            //randomly populates colors
            int randomMaterial = UnityEngine.Random.Range(0, 2);
            ballSetMaterials[i] = randomMaterial;
            //8 ball black-always ball 5
            if(i == 4)
            {
                ballSetMaterials[i] = 2;
            }
            //updates counts of balls
            if(ballSetMaterials[i] == 0 && i != 4)
            {
                blues++;
            }else if(ballSetMaterials[i] == 1 && i != 4)
            {
                reds++;
            }
        }
        //while balls aren't equal, randomly recolor one (from the color with less)
        while(reds != blues)
        {
            if(reds < blues)
            {
                int randomBlue = UnityEngine.Random.Range(0, 15);
                if(ballSetMaterials[randomBlue] == 0 && randomBlue != 4)
                {
                    ballSetMaterials[randomBlue] = 1;
                    reds++;
                    blues--;
                }
            }
            if(blues < reds)
            {
                int randomRed = UnityEngine.Random.Range(0, 15);
                if(ballSetMaterials[randomRed] == 1 && randomRed != 4)
                {
                    ballSetMaterials[randomRed] = 0;
                    blues++;
                    reds--;
                }
            }
        }
        //applies materials to balls
        for(int i = 0; i < 15; i++)
        {
            print(ballSetMaterials[i]);
            if(ballSetMaterials[i] == 0)
            {
                ballObjects[i].GetComponent<BallController>().ballColor = "Blue";
                ballObjects[i].GetComponent<Renderer>().material = ballMaterials[0];
            }else if(ballSetMaterials[i] == 1)
            {
                ballObjects[i].GetComponent<BallController>().ballColor = "Red";
                ballObjects[i].GetComponent<Renderer>().material = ballMaterials[1];
            }else if(ballSetMaterials[i] == 2)
            {
                ballObjects[i].GetComponent<BallController>().ballColor = "Eight";
                ballObjects[i].transform.tag = "8Ball";
                ballObjects[i].GetComponent<Renderer>().material = ballMaterials[2]; 
            }
        
    }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
