using System;
using UnityEngine;

public class WakeZoneScript : MonoBehaviour
{
    public EnemySkullScript _enemySkullScript;

    public PlayerScript playerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           
            _enemySkullScript.playerInWakeZone = true;
            _enemySkullScript. canRun = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _enemySkullScript.playerInWakeZone = false;
            _enemySkullScript.canRun = false;
        }
    }
}
