using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAliveChecker : MonoBehaviour
{
    /* SCRIPT FUNCTION:
     * Checks every frame to see if certain death conditions have been met...
     * Then disables external components like the player movement scripts and the likes.
     * Would also activate certain things like the game over screen, car explosion SFX, etc.
     */ 
    
    public bool isAlive = true;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Player status has been set to " + isAlive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
