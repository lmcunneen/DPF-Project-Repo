using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWave : MonoBehaviour
{
    public float waveSpeed;
    public float maxWaveSpeed;
    float waveDirectionMask;
    
    // Start is called before the first frame update
    void Start()
    {
        waveSpeed = .1f;
        maxWaveSpeed = 2f;
        waveDirectionMask = gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (true)
        {
            transform.Translate(Vector2.up * waveDirectionMask * waveSpeed);
            waveDirectionMask += -196.9f;
        }
    }
}
