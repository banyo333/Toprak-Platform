using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using Slider = UnityEngine.UI.Slider;
public class BossScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public int BossCurrentHealth = 400;
    private bool isFacingRight = false;
    public GameObject Player;
    public SpriteRenderer bossSprite;
    public bool playerInAttackZone = false;
    public Animator animator;
    public bool isAttacking = false;
    public float walkSpeed = 2f;
    public GameObject AttackPoint;
    public float attackRange = 1.5f;
    AudioManager audioManager;
    public bool checkForPlayerSide = true;
    public Slider slider;
    public PlayerScript playerScript;

    private void Awake()
    {
        rb = this.GameObject().GetComponent<Rigidbody2D>();
        bossSprite = this.GameObject().GetComponent<SpriteRenderer>();
        animator =   this.GameObject().GetComponent<Animator>();
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }
    
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        slider.minValue = 0;
        slider.maxValue = 400;
        playerScript = Player?.GetComponent<PlayerScript>();
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   private void FixedUpdate()
   {
       if (playerScript.playerIsDead == false)
       {
           
   
    // Only allow movement if not attacking
    if (checkForPlayerSide)
    {
        // Check if player is to the left or right of the boss to flip
        if (Player.transform.position.x < rb.transform.position.x)
        {
            if (isFacingRight)
            {
                bossSprite.flipX = false;
                isFacingRight = false;
            }
        }
        else if (Player.transform.position.x > rb.transform.position.x)
        {
            if (!isFacingRight)
            {
                bossSprite.flipX = true;
                isFacingRight = true;
            }
        }
    }
    if (!isAttacking) 
    {
      

        if (!playerInAttackZone)
        {
            animator.SetBool("Walk", true);
            rb.linearVelocity = new Vector2(walkSpeed * (isFacingRight ? 1 : -1), rb.linearVelocity.y); // Move boss
        }
    }

    // When player is in attack zone and boss is not attacking, perform the attack
    if (playerInAttackZone && !isAttacking)
    {
        animator.SetBool("Walk", false);
        StartCoroutine(AttackReset());
    }

    slider.value = BossCurrentHealth ;
    // If boss is dead, handle death logic
    if (BossCurrentHealth <= 0)
    {
        StartCoroutine(Dead());
        playerScript.WinUIScreen();
    }
       }
}

IEnumerator AttackReset()
{
    checkForPlayerSide = false;

    isAttacking = true;

    // Play attack animation
    animator.SetTrigger("Attack");
    yield return new WaitForSeconds(1f); // Attack animation duration

    // Play sound effect for the attack
    audioManager.PlaySFX(audioManager.bossHit);

    // Move the AttackPoint based on the direction of the boss
    AttackPoint.transform.localPosition = new Vector3((isFacingRight ? 0.45f : -0.45f), -0.3f, 0);
    yield return new WaitForSeconds(0.1f); // Duration of attack hitbox activation

    // Reset AttackPoint back to default position
    AttackPoint.transform.localPosition = Vector3.zero;

    // Reset attack state

    // Optional: Add attack cooldown here if needed
    yield return new WaitForSeconds(1f);
    checkForPlayerSide = true;
    yield return new WaitForSeconds(1f);

    isAttacking = false;// Cooldown before the next attack can happen
}
    
    IEnumerator Dead()
    {
        audioManager.PlaySFX(audioManager.bossDeath);

        animator.SetBool("Dead",true);
        yield return new WaitForSeconds(3f); // Attack cooldown
        Destroy(this.gameObject);
    }


  

    
}
