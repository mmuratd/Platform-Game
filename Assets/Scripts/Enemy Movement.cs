using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    float speed = 1f;

    Rigidbody2D myRigidbody2D;
    BoxCollider2D myBoxCollider2D;
    PolygonCollider2D myPolygonCollider2D;

    // Start is called before the first frame update
    void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        myPolygonCollider2D = GetComponent<PolygonCollider2D>();
        
    }
    
    // Update is called once per frame
    void Update()
    {
        enemyMove();
        FlipSprite();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground","Climbing","Hazard")))
        {
            speed = -speed;
            FlipSprite();
        }
        
    }
    void OnTriggerExit2D(Collider2D other) {
        if (!myPolygonCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            speed = -speed;
            FlipSprite();
        }
    }

    void enemyMove()
    {
        myRigidbody2D.velocity = new Vector2(speed, 0f);
    }

    public void FlipSprite()
    {
        bool hasHorizontalSpeed = myRigidbody2D.velocity.x != 0;
        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody2D.velocity.x), 1f);
        }
    }
}
