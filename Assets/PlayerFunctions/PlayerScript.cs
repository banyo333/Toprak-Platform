using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Security.Cryptography;
using NUnit.Framework.Internal;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using Random = UnityEngine.Random;
using Slider = UnityEngine.UI.Slider;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public ParticleSystem healParticle;
    [SerializeField]
    public Rigidbody2D rb;

    public bool playerIsDead = false;
    private SpriteRenderer _spriteRenderer;
    private CapsuleCollider2D playerCollider;
    public GameObject popUpDamage;
     AudioManager audioManager;
    public GameObject enemySkull;
    public GameObject boss;
    //UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI 
    public Slider slider;
    public bool EscapeMenuIsActive = false;

    public GameObject EscapeMenu;
    public GameObject GameOverUI;
    public GameObject WinUI;
    //UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI UI 

    
    //ATTACK ATTACK ATTACK ATTACK ATTACK ATTACK ATTACK ATTACK ATTACK ATTACK ATTACK ATTACK 
    public bool IsAttacking;
    private bool _canAttack = true; // Flag to track attack cooldown
    //ATTACK ATTACK ATTACK ATTACK ATTACK ATTACK ATTACK ATTACK ATTACK ATTACK ATTACK ATTACK 


    //ANIMATION ANIMATION ANIMATION ANIMATION ANIMATION ANIMATION ANIMATION ANIMATION 
    public Animator animator;
    //ANIMATION ANIMATION ANIMATION ANIMATION ANIMATION ANIMATION ANIMATION ANIMATION 

    
    //ROLL ROLL ROLL ROLL ROLL ROLL ROLL ROLL ROLL ROLL ROLL ROLL ROLL ROLL ROLL ROLL 
    public float rollSpeed = 6f;
    public bool _isRolling = false;

    public bool canRoll=true;
    //ROLL ROLL ROLL ROLL ROLL ROLL ROLL ROLL ROLL ROLL ROLL ROLL ROLL ROLL ROLL ROLL 
    
    
    //MOVE INPUT MOVE INPUT MOVE INPUT MOVE INPUT MOVE INPUT MOVE INPUT MOVE INPUT 
    private Vector2 moveInput;
    [SerializeField]
    public bool _isFacingRight=true;
    //MOVE INPUT MOVE INPUT MOVE INPUT MOVE INPUT MOVE INPUT MOVE INPUT MOVE INPUT 
    
    //JUMP JUMP JUMP JUMP JUMP JUMP JUMP JUMP JUMP JUMP JUMP JUMP JUMP JUMP JUMP 
    public bool _isJumping = false;
    public float JumpSpeed = 5f;
    //JUMP JUMP JUMP JUMP JUMP JUMP JUMP JUMP JUMP JUMP JUMP JUMP JUMP JUMP JUMP 

    //Crouch Crouch Crouch Crouch Crouch Crouch Crouch Crouch Crouch Crouch Crouch 
    public bool  _IsCrouch = false;

    public float CrouchSpeed = 2f;
    //Crouch Crouch Crouch Crouch Crouch Crouch Crouch Crouch Crouch Crouch Crouch 

    //RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN
    [SerializeField]
    private float RunSpeed = 4f;
    private bool _isRunning;
    //RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN

    //(Air State) (Air State) (Air State) (Air State) (Air State) (Air State) (Air State)  
    public CircleCollider2D GroundSphere;
    [SerializeField]
    public bool _isInAir = false;
    public LayerMask GroundLayer;
    //(Air State) (Air State) (Air State) (Air State) (Air State) (Air State) (Air State) 
    
    //HEAL HEAL HEAL HEAL HEAL HEAL HEAL HEAL HEAL HEAL HEAL HEAL HEAL HEAL HEAL HEAL HEAL 
    public int playerMaxHealth = 100;
    public int playerCurrentHealth=100;
    //HEAL HEAL HEAL HEAL HEAL HEAL HEAL HEAL HEAL HEAL HEAL HEAL HEAL HEAL HEAL HEAL HEAL 
    
    //ATTACK POINT ATTACK POINT ATTACK POINT ATTACK POINT ATTACK POINT ATTACK POINT ATTACK POINT 
    public CircleCollider2D AttackPoint;
    public float AttackPointRadius;
    public float AttackRange = 1f;
    public LayerMask skullLayerMask;
    public LayerMask BossLayerMask;

    //ATTACK POINT ATTACK POINT ATTACK POINT ATTACK POINT ATTACK POINT ATTACK POINT ATTACK POINT 

    void Start()
    {
        slider.maxValue = 100;
        slider.minValue = 0;
        audioManager = FindObjectOfType<AudioManager>();
        DontDestroyOnLoad(audioManager);
    }

    private void Awake()
    {
        rb= this.gameObject.GetComponent<Rigidbody2D>();
        animator = this.gameObject.GetComponent<Animator>();
        _spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        playerCollider = this.gameObject.GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerIsDead)
        {
            slider.value = playerCurrentHealth;
        }
       
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the Unity Editor
#endif
    }
    private void FixedUpdate()
    {
        if (!playerIsDead)
        {


            //Stop movement while attacking
            if (IsAttacking)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
                return;
            }



            //does character facing right ?
            if (!_isRolling)
            {
                _isFacingRight = (moveInput.x != 0) ? (moveInput.x > 0) : _isFacingRight;
            }

            _spriteRenderer.flipX = !_isFacingRight;

            //Running movement
            if (_isRunning)
            {
                if (_isRolling && !_isInAir)
                {
                    float rollDirection = _isFacingRight ? 1 : -1;
                    rb.linearVelocity = new Vector2(rollDirection * rollSpeed, rb.linearVelocityY);
                }
                else if (_IsCrouch)
                {
                    rb.linearVelocity = new Vector2(moveInput.x * CrouchSpeed, rb.linearVelocityY);
                }
                else
                {
                    rb.linearVelocity = new Vector2(moveInput.x * RunSpeed, rb.linearVelocityY);
                }
            }
            else if (!_isRunning)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
            }
        }
            if (playerCurrentHealth <= 0)
            {
                animator.SetBool("IsDead", true);
                playerIsDead = true;
                StartCoroutine(deathHalfTime(0.4F));

                StartCoroutine(DisableAnimatorAfterDelay(0.85f)); 
                rb.linearVelocityX = 0;
                StartCoroutine(GameOverUIC(2F));

            }
            

    }

    public void WinUIScreen()
    {
        StartCoroutine(WinUIC(2f));
    }
    IEnumerator WinUIC(float delay)
    {
        yield return new WaitForSeconds(delay);
        WinUI.SetActive(true);
    }
    IEnumerator GameOverUIC(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameOverUI.SetActive(true);
    }
    IEnumerator deathHalfTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerCollider.size = new Vector2(0.19F, 0.2F);

    }
    IEnumerator DisableAnimatorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.enabled = false; // Stops animation completely
    }
//MOVEMENT FUNCTIONS MOVEMENT FUNCTIONS MOVEMENT FUNCTIONS MOVEMENT FUNCTIONS MOVEMENT FUNCTIONS MOVEMENT FUNCTIONS MOVEMENT FUNCTIONS MOVEMENT FUNCTIONS
  public void OnRoll(InputAction.CallbackContext context)
  {
      if (!playerIsDead &&  context.performed && !_isInAir && !_IsCrouch && !IsAttacking && _isRunning&&canRoll)
      {
          _isRolling = true;
          canRoll = false;
          //RunSpeed = rollSpeed;
          animator.SetTrigger("IsRolling");
          playerCollider.size = new Vector2(0.1984104f, 0.24f); // Smaller collider size

          float rollDirection = _isFacingRight ? 1 : -1; // Determine roll direction
          StartCoroutine(RollWaitTime());
          
        
      }
  }
    

  public IEnumerator RollWaitTime() // Change to IEnumerator
  {

      yield return new WaitForSeconds(0.5f); // Duration of roll
      playerCollider.size = new Vector2(0.1984104f, 0.3804622f);  // Reset the collider size
      animator.ResetTrigger("IsRolling");

          
      RunSpeed = 4f; // Reset speed after rolling
      _isRolling = false;

      yield return new WaitForSeconds(0.3f); // Duration of roll
      canRoll = true;

  }


  public void OnJump(InputAction.CallbackContext context)
    {
        if (!playerIsDead &&context.started && !_isInAir && !_isRolling&&!_IsCrouch)
        {
            _isJumping = true;
            animator.SetTrigger("IsJumping");
            rb.AddForce(new Vector2(0,JumpSpeed),ForceMode2D.Impulse);
            rb.gravityScale = 1f;
            animator.ResetTrigger("IsRolling"); // Reset to prevent stacking rolls
            
        }

        if (context.canceled)
        {
            _isJumping = false;
            rb.gravityScale = 2f;
            
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!playerIsDead &&context.performed )
        {
            
            moveInput = context.ReadValue<Vector2>();
            _isRunning = true;
            animator.SetBool("IsRunning",true);

        }

        if (context.canceled)
        {
            _isRunning = false;
            animator.SetBool("IsRunning",false);

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!playerIsDead &&other.CompareTag("Spikes"))
        {
            playerCurrentHealth = 0;
            Debug.Log(playerCurrentHealth);
        }

        if (!playerIsDead &&other.CompareTag("Heal"))
        {
            healParticle.Play();
            playerCurrentHealth += 50;
            if (playerCurrentHealth > 100)
            {
                playerCurrentHealth = 100;
            }
            Destroy(other.gameObject);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!playerIsDead &&context.performed && !_isRolling && _canAttack)
        {
            if (!_IsCrouch)
            {
                IsAttacking = true;
                int attacknumber = UnityEngine.Random.Range(1, 3); // Generates 1 or 2

                if (attacknumber == 1)
                {
                    animator.SetTrigger("Attack1");
                }
                else if (attacknumber == 2)
                {
                    animator.SetTrigger("Attack2");
                }
                
                _canAttack = false; // Prevent further attacks
                StartCoroutine(ResetAttackCooldown()); // Start cooldown timer
            }
            else if (_IsCrouch)
            {
                IsAttacking = true;
                animator.SetTrigger("CrouchAttack");
                _canAttack = false; // Prevent further attacks
                StartCoroutine(ResetAttackCooldown()); // Start cooldown timer
            }
           
        }

        if (context.canceled)
        {
            animator.ResetTrigger("CrouchAttack");
            animator.ResetTrigger("Attack1");
            animator.ResetTrigger("Attack2");
        }
    }

    void Attack()
    {
        if (!playerIsDead)
        {
            
            Collider2D[] boss = Physics2D.OverlapCircleAll(AttackPoint.transform.position, AttackPointRadius, BossLayerMask );

        Collider2D[] skull = Physics2D.OverlapCircleAll(AttackPoint.transform.position, AttackPointRadius, skullLayerMask );

        if ( IsAttacking)
        {
            // Get the EnemySkullScript attached to this specific enemy
            foreach (Collider2D skullGameObject in skull )
            {
                skullGameObject.GetComponent<EnemySkullScript>().enemySkullHealth-=50;
                skullGameObject.GetComponent<EnemySkullScript>().SkullAnimator.SetTrigger("Hit");
                audioManager.PlaySFX(audioManager.playerHit);
                Vector3 popUpPosition = skullGameObject.transform.position + new Vector3(4, 1f, 0); // Move it 1 unit up
                GameObject popUp = Instantiate(popUpDamage, popUpPosition, Quaternion.identity);

                popUp.GetComponentInChildren<TMP_Text>().text = 50.ToString();
                if (gameObject.transform.position.x > skullGameObject.transform.position.x) 
                    popUp.GetComponent<PopUpDamage>().HitFromRight = true;

            }

                
      
        
        foreach (Collider2D bossGameObject in boss )
        {
                // Get the EnemySkullScript attached to this specific enemy
                bossGameObject.GetComponent<BossScript>(). BossCurrentHealth -= 50;
                bossGameObject.GetComponent<BossScript>().animator.SetTrigger("Hit");
                audioManager.PlaySFX(audioManager.playerHit);

                Vector3 popUpPosition = bossGameObject.transform.position + new Vector3(4, 1f, 0); // Move it 1 unit up
                GameObject popUp1 = Instantiate(popUpDamage, popUpPosition, Quaternion.identity);
                if (popUp1 == null)
                {
                    Debug.Log("POPUP YOK");
                }
                popUp1.GetComponentInChildren<TMP_Text>().text = 50.ToString();
                if (gameObject.transform.position.x > bossGameObject.transform.position.x)
                    popUp1.GetComponent<PopUpDamage>().HitFromRight = true;
            }
        }
        
        }
    }
    
    private IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(0.2f); // Wait 0.4 seconds before allowing next attack
        if (_isFacingRight)
        {
            AttackPoint.transform.position = new Vector3(rb.transform.position.x + AttackRange, rb.transform.position.y , 0);

        }
        else if (!_isFacingRight)
        {
            AttackPoint.transform.position = new Vector3(rb.transform.position.x - AttackRange, rb.transform.position.y , 0);

        }
        yield return new WaitForSeconds(0.2f); // Wait 0.4 seconds before allowing next attack

        AttackPoint.transform.position = new Vector3(rb.transform.position.x , rb.transform.position.y , 0);

        _canAttack = true; // Reset attack availability
        IsAttacking = false;

    }

    public void PLayerTakesDamage(int damage)
    {
        Debug.Log("Damage received: " + damage);
        if (!playerIsDead)
        {
            
      
        Vector3 popUpPosition = gameObject.transform.position + new Vector3(4, 2f, 0); // Move it 1 unit up
        GameObject popUp = Instantiate(popUpDamage, popUpPosition, Quaternion.identity);
        TMP_Text damageText = popUp.GetComponentInChildren<TMP_Text>();
        audioManager.PlaySFX(audioManager.playerGetsHit);

        // Set text and color
        damageText.text = damage.ToString();
        damageText.color = Color.red;

        // Determine the closest enemy/boss (whichever is attacking)
        bool hitFromRight = true; // Default

        if (enemySkull != null)
        {
            hitFromRight = gameObject.transform.position.x > enemySkull.transform.position.x;
        }
        else if (boss != null)
        {
            hitFromRight = gameObject.transform.position.x > boss.transform.position.x;
        }

        popUp.GetComponent<PopUpDamage>().HitFromRight = !hitFromRight;
    }
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (!EscapeMenuIsActive)
        {
            EscapeMenu.SetActive(true);
            EscapeMenuIsActive = true;
            Time.timeScale = 0; 

        }
        else if (EscapeMenuIsActive)
        {
            EscapeMenu.SetActive(false);
            EscapeMenuIsActive = false;
            Time.timeScale = 1; 

        }
            
      
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed && !playerIsDead)
        {
            if (_IsCrouch && !_isInAir && !_isRolling && !IsAttacking)
            {
                // Exit crouch
                _IsCrouch = false;
                animator.SetBool("Crouch", false);
                playerCollider.size = new Vector2(0.1984104f, 0.3804622f);  // Reset the collider size
            
                // Optionally, adjust the position to prevent falling
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z); // Raise slightly if needed
            }
            else if (!_IsCrouch && !_isInAir && !_isRolling && !IsAttacking)
            {
                // Start crouch
                _IsCrouch = true;
                animator.SetBool("Crouch", true);

                // Adjust position to prevent falling into ground
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z); // Lower slightly if needed
            
                // Second crouch size adjustment
                playerCollider.size = new Vector2(0.1984104f, 0.28f); // Smaller collider size
            }
        }
    }

}
