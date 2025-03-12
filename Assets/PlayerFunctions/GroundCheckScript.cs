using System;
using UnityEngine;

public class GroundCheckScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public LayerMask GroundLayer;
    [SerializeField]
     PlayerScript playerScript;
     void Start()
     {
         playerScript = GetComponentInParent<PlayerScript>(); // Auto-assign PlayerScript from parent
     }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            playerScript._isInAir = false; // Player is on the ground
            playerScript.animator.SetBool("IsInAir",false);
            playerScript.rb.gravityScale = 1f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground")) // Ground Layer
        {
            playerScript._isInAir = true; // Player left the ground
            playerScript.animator.SetBool("IsInAir",true);
            if (!playerScript._isJumping)
            {
                playerScript.rb.gravityScale = 2f;
            }

        }
    }

}
