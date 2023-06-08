using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWave : MonoBehaviour
{
    Rigidbody2D rb;
    public float waveSpeed;
    public float maxWaveSpeed;
    float waveDirectionMask;
    
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
}
