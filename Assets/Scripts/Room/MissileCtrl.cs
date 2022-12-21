using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCtrl : ObstacleCtrl
{
    Rigidbody2D rb;
    float speed = 3f;
    Vector2 dir;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dir = new Vector2(transform.up.x, transform.up.y);
        dir.Normalize();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + (dir * speed * Time.fixedDeltaTime));
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(playerCtrl == null)
            {
                playerCtrl = collision.GetComponent<PlayerCtrl>();
            }
            playerCtrl.TakeDamage(10);
            gameObject.SetActive(false);
            transform.position = transform.parent.transform.position;
        }
        if(collision.tag == "Wall")
        {
            gameObject.SetActive(false);
            transform.position = transform.parent.transform.position;
        }
    }
}
