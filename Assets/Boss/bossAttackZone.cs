using Unity.VisualScripting;
using UnityEngine;

public class bossAttackZone : MonoBehaviour
{
    [SerializeField]
    private BossScript _bossScript;

    private GameObject boss;
    private CircleCollider2D _circleCollider2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boss = GameObject.FindWithTag("Boss");
        _bossScript = boss.GetComponent<BossScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            
     
        
            
            _bossScript.playerInAttackZone = true;

        }
    }
   private void OnTriggerExit2D(Collider2D other)
   {
       if (other.CompareTag("Player"))
       {
           _bossScript.playerInAttackZone = false;
       }
   }
}
