using UnityEngine;

public class PopUpDamage : MonoBehaviour
{
    public Vector2 InitialVelocity;
    public bool HitFromRight = false;
    public Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (HitFromRight)
            InitialVelocity.x *= -1;
        rb.linearVelocity = InitialVelocity;
        Destroy(this.gameObject,1.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
