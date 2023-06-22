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
    public float distanceFromOriginLength;
    public int roundedDistanceFromOrigin;
    public int highestSessionScore;
    private string scoreMessageString; 
    
    void Start()
    {
        scoreMessageString = "Score: ";
    }

    void Update()
    {
        if (gameObject.transform.position.x > startingPoint.transform.position.x)
        {
            distanceFromOriginLength = gameObject.transform.position.x - startingPoint.transform.position.x;
            roundedDistanceFromOrigin = (int)distanceFromOriginLength; //Rounds down distanceLength and stores that into roundedDistance for displaying

            if (roundedDistanceFromOrigin > highestSessionScore) //If the new score is higher than the previous displayed score, do the following
            {
                StopAllCoroutines(); //Stop any ColourFade coroutine that might be occurring
                highestSessionScore = roundedDistanceFromOrigin;
                UIManager.GetComponent<UpdateUIElement>().UpdateCounterInt(roundedDistanceFromOrigin, scoreMessageString, scoreCounter); //Update the score counter
                StartCoroutine(ColourFade()); //Start a new ColourFade coroutine
            }
        }

        else
        {
            distanceFromOriginLength = 0;
        }
    }

    IEnumerator ColourFade() //Indicates when the counter has not been updated in some time
    {
        scoreCounter.GetComponent<Text>().color = Color.red;
        yield return new WaitForSeconds(2f);
        scoreCounter.GetComponent<Text>().color = Color.grey;
    }
}
