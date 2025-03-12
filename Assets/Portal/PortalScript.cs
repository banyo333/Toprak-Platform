using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PortalScript : MonoBehaviour
{
    public GameObject[] enemies; // Store all enemy objects
    AudioManager audioManager;
    public GameObject Player;
    public GameObject Boss;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        InvokeRepeating("CheckForEnemies", 0f, 3f); // Run every 3 seconds
    }

    private void Update()
    {
      
    }

    private void CheckForEnemies()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player")&& enemies.Length ==0 )
        {
            audioManager.ChangeToBossMusic();
            audioManager.PlaySFX(audioManager.whatIsDead);
            SceneManager.LoadScene("BossScene", LoadSceneMode.Single);
            Boss.GetComponent<BossScript>().playerScript = Player.GetComponent<PlayerScript>();



        }
           

            
        
    }
}