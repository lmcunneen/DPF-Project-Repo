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
                StopAllCoroutines();
                highestScore = roundedDistance;
                UIManager.GetComponent<UpdateUIElement>().UpdateElementInt(roundedDistance, scoreCounter);
                StartCoroutine(ColourFade());
            }
        }

        else
        {
            distanceLength = 0;
        }
    }

    IEnumerator ColourFade()
    {
        scoreCounter.GetComponent<Text>().color = Color.red;
        yield return new WaitForSeconds(3f);
        scoreCounter.GetComponent<Text>().color = Color.grey;
    }
}
