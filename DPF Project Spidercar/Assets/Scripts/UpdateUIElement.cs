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

    public Text scoreCounter; //Text that displays current score during gameplay
    public Text deathText; //Text that states the player has died
    public Text respawnText; //Text indicating that the player can respawn
    public GameObject personalBestParent; //Parent object of PB Text, used for scaling and grid logic
    public GameObject personalBestText; //Child object that holds the actual text for Personal Best
    public GameObject finalScoreParent; //Parent object of the final score text, used for grid logic
    public GameObject finalScoreText; //Child object that holds the actual text for Final Score
    public GameObject deathBackground;
    private Animator personalBestAnimator; //Animator for the PB Text
    public bool respawnActive; //Bool that activates the respawn state when true
    private int personalBestInt;
    private string personalBestString;
    private string finalScoreString;
    private Vector3 personalBestScale; //Used for scaling to make PB Text more prominent when final score isn't present

    private void Awake()
    {
        ResetUI();
        personalBestString = "Personal Best: ";
        personalBestInt = 0;
        finalScoreString = "Final Score: ";
        personalBestAnimator = personalBestText.GetComponent<Animator>();
        personalBestScale = personalBestParent.transform.localScale;
    }

    public void UpdateCounterInt(int number, string text, Text element)
    {
        element.text = text + number + "m";
    }

    public IEnumerator DisplayDeathElements(int score) //Called by PlayerAliveChecker on death
    {
        scoreCounter.enabled = false;
        deathText.enabled = true;
        deathBackground.SetActive(true);

        yield return new WaitForSeconds(2f);

        if (personalBestInt < score) //If the personal best has been beat, do the following
        {
            personalBestParent.SetActive(true);
            personalBestInt = score;
            UpdateCounterInt(score, personalBestString, personalBestText.GetComponent<Text>());
            personalBestText.GetComponent<Text>().color = Color.green;
            personalBestAnimator.Play("Personal Best"); //Makes it rotate left and right

            personalBestParent.transform.localScale = personalBestScale * 1.5f;

            yield return new WaitForSeconds(2f);

            respawnText.enabled = true;
            respawnActive = true;
        }

        else //If the personal best has not been beat, do the following
        {
            UpdateCounterInt(score, finalScoreString, finalScoreText.GetComponent<Text>());
            personalBestParent.transform.localScale = personalBestScale;
            personalBestParent.SetActive(true);
            finalScoreParent.SetActive(true);
            finalScoreText.SetActive(true);
            personalBestText.SetActive(false);

            yield return new WaitForSeconds(1f);

            personalBestText.SetActive(true);
            personalBestText.GetComponent<Text>().color = Color.cyan;
            personalBestAnimator.Play("PB Text Default"); //Makes it stationary

            yield return new WaitForSeconds(1f);

            respawnText.enabled = true;
            respawnActive = true;
        }
    }

    public void ResetUI() //Called by PlayerAliveChecker to reset UI on respawn
    {
        respawnActive = false;
        scoreCounter.enabled = true;
        scoreCounter.text = "Score: 0m";
        deathText.enabled = false;
        respawnText.enabled = false;
        deathBackground.SetActive(false);
        personalBestParent.SetActive(false);
        finalScoreParent.SetActive(false);
    }
}
