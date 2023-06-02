using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindDistanceTravelled : MonoBehaviour
{
    public GameObject startingPoint;
    public GameObject UIManager;
    public Text scoreCounter;
    public float distanceLength;
    public int roundedDistance;
    public int highestScore;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x > startingPoint.transform.position.x)
        {
            distanceLength = gameObject.transform.position.x - startingPoint.transform.position.x;
            roundedDistance = (int)distanceLength;

            if (roundedDistance > highestScore)
            {
                highestScore = roundedDistance;
                UIManager.GetComponent<UpdateUIElement>().UpdateElementInt(roundedDistance, scoreCounter);
            }
        }

        else
        {
            distanceLength = 0;
        }
    }
}
