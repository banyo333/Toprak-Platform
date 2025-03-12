using System;
using Unity.VisualScripting;
using UnityEngine;

public class SkullAttackPointScript : MonoBehaviour
{
    public CircleCollider2D AttackPoint;
    public EnemySkullScript SkullScript;
    public GameObject Player;
    public PlayerScript PlayerScript;

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        PlayerScript = Player.GetComponent<PlayerScript>();

    }

    private void Awake()
    {
        AttackPoint.GetComponent<CircleCollider2D>();
        
    }

    private void Update()
    {

        if (SkullScript.IsAttacking && (SkullScript.playerInWakeZone||SkullScript.playerInAttackZone )) // Move only when the skull is attacking
        {
            float offset = 1.7f; // Distance of attack point from the skull
            AttackPoint.transform.position = new Vector2(
                SkullScript.rb.transform.position.x + (SkullScript.skullIsFacingRight ? offset : -offset),
                SkullScript.rb.transform.position.y
            );
        }
        else
        {
            AttackPoint.transform.position = new Vector2(
                SkullScript.rb.transform.position.x,
                SkullScript.rb.transform.position.y);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player && !PlayerScript._isRolling && !SkullScript.skullIsDeath)
        {
            PlayerScript.animator.SetTrigger("Hit");
            PlayerScript.playerCurrentHealth -= 40;
            PlayerScript.PLayerTakesDamage(40);

        }
    }
}