using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float arrowSpeed = 10f;
    float xSpeed;
    int pointsForKillingEnemy = 100;

    Rigidbody2D myRigidbody;
    PlayerMovement player;
    
    

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        

        xSpeed = player.transform.localScale.x * arrowSpeed;
    }

    void Update()
    {
        if (xSpeed > 0)
        {
            myRigidbody.velocity = new Vector2(xSpeed, 0f);
            
        }
        else
        {
            myRigidbody.velocity = new Vector2(xSpeed, 0f);
            gameObject.transform.localScale = new Vector2(Mathf.Sign(xSpeed), 1f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            FindObjectOfType<GameSession>().AddToScore(pointsForKillingEnemy);
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Invoke("DestroyArrow", 10f * Time.deltaTime);
    }

    void DestroyArrow()
    {
        Destroy(gameObject);
    }

}
