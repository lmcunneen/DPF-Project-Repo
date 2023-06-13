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
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        maxWaveSpeed = 15f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.velocity.magnitude < maxWaveSpeed)
        {
            rb.AddForce(Vector2.right * maxWaveSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.name == "Car Graphic")
            {
                gameManager.GetComponent<PlayerAliveChecker>().OnDeathFunction();
            }
        }
    }
}
