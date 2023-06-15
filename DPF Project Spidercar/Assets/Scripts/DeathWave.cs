using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWave : MonoBehaviour
{
    /* SCRIPT FUNCTION:
     * Holds all functionality for the death wave, including movement and activating death state
     */

    Rigidbody2D rb;
    public float waveSpeed;
    public float maxWaveSpeed;

    public GameObject gameManager;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        maxWaveSpeed = 15f;
    }

    void FixedUpdate()
    {
        if (rb.velocity.magnitude < maxWaveSpeed) //If the wave is moving less than its max speed...
        {
            rb.AddForce(Vector2.right * maxWaveSpeed); //Move the wave to the right
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Car Graphic") //If the collider is under the player, execute the death function
        {
            gameManager.GetComponent<PlayerAliveChecker>().OnDeathFunction();
        }
    }
}
