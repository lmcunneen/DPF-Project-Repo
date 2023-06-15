using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAliveChecker : MonoBehaviour
{
    /* SCRIPT FUNCTION:
     * Holds function that when called, activates the death state (OnDeathFunction) and respawn logic (in Update)
     * Then disables external components like the player movement scripts and the likes.
     * Also activates certain things like the game over screen, car explosion SFX, etc.
     */

    public GameObject vehicle;
    public GameObject respawnPoint;
    public GameObject deathWave;
    public GameObject waveOrigin;
    public GameObject carBumper;
    public KeyCode respawnKey; //Set as 'R' in the inspector
    public int finalScore;
    private bool respawnActive;

    private void Start()
    {
        respawnActive = false;
    }

    private void Update()
    {
        respawnActive = gameObject.GetComponent<UpdateUIElement>().respawnActive;

        if (Input.GetKey(respawnKey) && respawnActive == true)
        {
            respawnActive = false;
            //Resets position of non-static objects (ie. the player and the wave)
            vehicle.transform.position = respawnPoint.transform.position;
            vehicle.transform.rotation = respawnPoint.transform.rotation;
            deathWave.transform.position = waveOrigin.transform.position;
            //Reenables the movement and grappling hook
            vehicle.GetComponent<VehicleMovement>().enabled = true;
            vehicle.GetComponent<GrapplingHook>().enabled = true;
            vehicle.GetComponent<GrapplingHook>().grapplePointObject.SetActive(true);
            //Resets UI, highest stored score and counter text colour
            gameObject.GetComponent<UpdateUIElement>().ResetUI();
            carBumper.GetComponent<FindDistanceTravelled>().highestScore = 0;
            carBumper.GetComponent<FindDistanceTravelled>().scoreCounter.color = Color.red;
        }
    }

    public void OnDeathFunction()
    {
        Debug.Log("Ack! Player am dead!");
        //Disables vehicle movement, grappling hook, and rendering components
        vehicle.GetComponent<VehicleMovement>().enabled = false;
        vehicle.GetComponent<GrapplingHook>().enabled = false;
        vehicle.GetComponent<GrapplingHook>().springJoint.enabled = false;
        vehicle.GetComponent<GrapplingHook>().grapplePointObject.SetActive(false);
        vehicle.GetComponent<GrapplingHook>().lineRenderer.enabled = false;
        //Finds the final score for death screen
        finalScore = carBumper.GetComponent<FindDistanceTravelled>().highestScore;
        StartCoroutine(gameObject.GetComponent<UpdateUIElement>().DisplayDeathElements(finalScore));
    }
}
