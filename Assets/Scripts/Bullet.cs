using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 10f;
    PlayerMovement playerMovement;
    Rigidbody2D myRigidBody2D;

    float xSpeed = 1;

    // Start is called before the first frame update
    void Awake()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        playerMovement = FindObjectOfType<PlayerMovement>();   
        
        
    }
    void Start()
    {
        xSpeed = playerMovement.transform.localScale.x * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        myRigidBody2D.velocity = new Vector2 (xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        Destroy(gameObject);
    }
}
