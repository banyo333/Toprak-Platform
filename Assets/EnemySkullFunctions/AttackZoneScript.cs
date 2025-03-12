using System;
using UnityEngine;

public class AttackZoneScript : MonoBehaviour
{
    public EnemySkullScript _enemySkullScript;
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
            _enemySkullScript.canAttack = true;

            _enemySkullScript.playerInAttackZone = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _enemySkullScript.canAttack = false;


            _enemySkullScript.playerInAttackZone = false;
        }
    }
}
