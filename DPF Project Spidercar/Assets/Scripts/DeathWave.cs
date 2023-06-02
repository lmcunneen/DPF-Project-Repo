using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWave : MonoBehaviour
{
    private Rigidbody2D rb;
    public float waveSpeed;
    public float maxWaveSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        waveSpeed = 1f;
        maxWaveSpeed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude < maxWaveSpeed)
        {
            rb.AddForce(Vector2.up * waveSpeed * Time.fixedDeltaTime);
        }
    }
}
