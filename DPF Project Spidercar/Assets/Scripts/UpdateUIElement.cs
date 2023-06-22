using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUIElement : MonoBehaviour
{
    /* SCRIPT FUNCTION:
     * Updates UI elements. Most functions are called by other scripts
     * Notably, this holds the logic for the game over screen, including the personal best counter
     */

    public Text currentScoreCounter;
    public Text deathMessageText;
    public Text respawnMessageText;
    public GameObject personalBestGridLogic;
    public GameObject personalBestText;
    public GameObject finalScoreGridLogic;
    public GameObject finalScoreText;
    public GameObject deathBackground;
    private Animator personalBestAnimator;
    [HideInInspector] public bool respawnActive;
    private int personalBestInt;
    private string personalBestString;
    private string finalScoreString;
    private Vector3 personalBestStandaloneScale;

    private void Awake()
    {
        ResetUI();
        personalBestString = "Personal Best: ";
        personalBestInt = 0;
        finalScoreString = "Final Score: ";
        personalBestAnimator = personalBestText.GetComponent<Animator>();
        personalBestStandaloneScale = personalBestGridLogic.transform.localScale;
    }

    public void UpdateCounterInt(int number, string text, Text element)
    {
        element.text = text + number + "m";
    }

    public IEnumerator DisplayDeathElements(int score) //Called by PlayerAliveChecker on death
    {
        currentScoreCounter.enabled = false;
        deathMessageText.enabled = true;
        deathBackground.SetActive(true);

        yield return new WaitForSeconds(2f);

        if (personalBestInt < score) //If the personal best has been beat, do the following
        {
            personalBestGridLogic.SetActive(true);
            personalBestInt = score;
            UpdateCounterInt(score, personalBestString, personalBestText.GetComponent<Text>());
            personalBestText.GetComponent<Text>().color = Color.green;
            personalBestAnimator.Play("Personal Best"); //Makes it rotate left and right

            personalBestGridLogic.transform.localScale = personalBestStandaloneScale * 1.5f;

            yield return new WaitForSeconds(2f);

            respawnMessageText.enabled = true;
            respawnActive = true;
        }

        else //If the personal best has not been beat, do the following
        {
            UpdateCounterInt(score, finalScoreString, finalScoreText.GetComponent<Text>());
            personalBestGridLogic.transform.localScale = personalBestStandaloneScale;
            personalBestGridLogic.SetActive(true);
            finalScoreGridLogic.SetActive(true);
            finalScoreText.SetActive(true);
            personalBestText.SetActive(false);

            yield return new WaitForSeconds(1f);

            personalBestText.SetActive(true);
            personalBestText.GetComponent<Text>().color = Color.cyan;
            personalBestAnimator.Play("PB Text Default"); //Makes it stationary

            yield return new WaitForSeconds(1f);

            respawnMessageText.enabled = true;
            respawnActive = true;
        }
    }

    public void ResetUI() //Called by PlayerAliveChecker to reset UI on respawn
    {
        respawnActive = false;
        currentScoreCounter.enabled = true;
        currentScoreCounter.text = "Score: 0m";
        deathMessageText.enabled = false;
        respawnMessageText.enabled = false;
        deathBackground.SetActive(false);
        personalBestGridLogic.SetActive(false);
        finalScoreGridLogic.SetActive(false);
    }
}
