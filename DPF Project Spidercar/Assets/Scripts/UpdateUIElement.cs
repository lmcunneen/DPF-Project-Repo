using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUIElement : MonoBehaviour
{
    public Text scoreCounter;
    public Text deathText;
    public Text respawnText;
    public GameObject deathBackground;
    public bool respawnActive;

    private void Awake()
    {
        ResetUI();
    }

    public void UpdateCounterInt(int number)
    {
        scoreCounter.text = "Score: " + number + "m";
    }

    public IEnumerator DisplayDeathElements()
    {
        scoreCounter.enabled = false;
        deathText.enabled = true;
        deathBackground.SetActive(true);
        yield return new WaitForSeconds(2f);
        respawnText.enabled = true;
        respawnActive = true;
    }

    public void ResetUI()
    {
        respawnActive = false;
        scoreCounter.enabled = true;
        scoreCounter.text = "Score: 0m";
        deathText.enabled = false;
        respawnText.enabled = false;
        deathBackground.SetActive(false);
    }
}
