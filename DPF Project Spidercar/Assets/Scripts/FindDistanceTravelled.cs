using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindDistanceTravelled : MonoBehaviour
{
    /* SCRIPT FUNCTION:
     * Finds the distance between the vehicle and its origin point for score counting
     * Also contains coroutine that makes the colour fade on the score counter if not updated after x amount of seconds
     */

    public GameObject startingPoint;
    public GameObject UIManager;
    public Text scoreCounter;
    public float distanceLength; //Float that holds the precise distance of player from origin
    public int roundedDistance; //The current distance/score of the player rounded down to nearest whole number
    public int highestScore; //Saved score of player's highest score during this play session
    private string scoreString; //The string that is plugged into the UpdateCounterInt function on external script
    
    void Start()
    {
        scoreString = "Score: ";
    }

    void Update()
    {
        if (gameObject.transform.position.x > startingPoint.transform.position.x)
        {
            distanceLength = gameObject.transform.position.x - startingPoint.transform.position.x;
            roundedDistance = (int)distanceLength; //Rounds down distanceLength and stores that into roundedDistance for displaying

            if (roundedDistance > highestScore) //If the new score is higher than the previous displayed score, do the following
            {
                StopAllCoroutines(); //Stop any ColourFade coroutine that might be occurring
                highestScore = roundedDistance;
                UIManager.GetComponent<UpdateUIElement>().UpdateCounterInt(roundedDistance, scoreString, scoreCounter); //Update the score counter
                StartCoroutine(ColourFade()); //Start a new ColourFade coroutine
            }
        }

        else
        {
            distanceLength = 0;
        }
    }

    IEnumerator ColourFade() //Indicates when the counter has not been updated in some time
    {
        scoreCounter.GetComponent<Text>().color = Color.red; //Sets the colour to red
        yield return new WaitForSeconds(2f);
        scoreCounter.GetComponent<Text>().color = Color.grey; //Then after set amount of time, changes it to grey
    }
}
