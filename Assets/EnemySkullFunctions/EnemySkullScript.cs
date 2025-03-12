using System;
using System.IO.Enumeration;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class EnemySkullScript : MonoBehaviour
{
    public GameObject Heart;
    public Rigidbody2D rb;
    private SpriteRenderer _spriteRenderer;
    private GameObject Player;
    [SerializeField]
    private PlayerScript _playerScript;

    public Slider slider;
    public Transform sliderTransform;
    private Transform PlayerTransform;
    public Animator SkullAnimator;
    private CapsuleCollider2D enemySkullCollider;
    public bool canAttack = false;
    public bool IsAttacking = false;
    public float runSpeed = 1.5f;
    public bool playerInWakeZone = false;
    public bool playerInAttackZone = false;
    public bool skullIsFacingRight = true;
    public int lookDirectionInt;
    public bool canRun = false;
    public bool skullIsFacingPlayer = true;
    public int enemySkullHealth = 100;
    
    public float attackCooldown = 1.5f; // Add cooldown time
    private bool isOnCooldown = false; // Flag to prevent instant re-attacks
    public bool skullIsDeath = false;
    void Start()
    {
        slider.minValue = 0;
        slider.maxValue = 100f;
        Player = GameObject.FindWithTag("Player");
        _playerScript = Player.GetComponent<PlayerScript>();
        if (_spriteRenderer.flipX == true)
        {
            skullIsFacingPlayer = true;
        }
        else if (_spriteRenderer.flipX == false)
        {
            skullIsFacingPlayer = false;
        }
    }

    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        _spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        SkullAnimator = this.gameObject.GetComponent<Animator>();
        enemySkullCollider = this.gameObject.GetComponent<CapsuleCollider2D>();
       
    }

    // Update is called once per frame
    void Update()
    {
       
       
        
        if (_playerScript.playerIsDead == false)
        {
            
        slider.value = enemySkullHealth;
        slider.transform.position = rb.transform.position;
        // Make the slider follow the enemy's position in world space
        Vector3 worldPosition = rb.transform.position + Vector3.up * .7f; // Adjust the height above the enemy
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition); // Convert world position to screen space
        slider.transform.position = screenPosition; // Set slider's position in screen space
        if (!skullIsDeath)
        {
        PlayerTransform = Player.transform;
      
        if (playerInWakeZone && !(!skullIsFacingPlayer && _playerScript._IsCrouch && !playerInAttackZone) )
        {
          
            if (PlayerTransform.position.x < rb.transform.position.x)
            {
                skullIsFacingRight = false;
                lookDirectionInt = -1;
            }
            else if(PlayerTransform.position.x >= rb.transform.position.x)
            {
                skullIsFacingRight = true;
                lookDirectionInt = 1;
            }
           
            _spriteRenderer.flipX = !skullIsFacingRight; 
            }
            //Facing Right Check
            if (!playerInAttackZone)
            {
                rb.linearVelocityX = runSpeed * lookDirectionInt;
            }

        

        if (canAttack && playerInAttackZone && !isOnCooldown)
        {
            StartCoroutine(AttackPlayer());
        }
        
        if (canRun && !playerInAttackZone)
        {
            SkullAnimator.SetBool("Run",true);
        }
        else if (!canRun ||playerInAttackZone)
        {
            SkullAnimator.SetBool("Run",false);
        }

        if (enemySkullHealth <= 0)
        {
            SkullAnimator.SetBool("Die",true);
            skullIsDeath = true;
            StartCoroutine(Die());
        } 
        }
        }
    }
    private IEnumerator Die()
    {
        enemySkullCollider.size = new Vector2(0.17f, 0.17f);
        yield return new WaitForSeconds(1.42f); // Attack animation time
        Instantiate(Heart,new Vector3(gameObject.transform.position.x,gameObject.transform.position.y+1,gameObject.transform.position.z),Quaternion.identity);

        Destroy(this.gameObject);
    }
    
    private IEnumerator AttackPlayer()
    {
        if (_playerScript.playerIsDead == false)
        {
            
       
        isOnCooldown = true;
       

        SkullAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f); // Attack animation time
        IsAttacking = true;
        yield return new WaitForSeconds(0.1f); // Attack animation time

        IsAttacking = false;
        yield return new WaitForSeconds(attackCooldown); // Cooldown before attacking again

        isOnCooldown = false; // Allow new attacks
    } }

}  