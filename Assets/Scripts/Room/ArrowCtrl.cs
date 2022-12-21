using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCtrl : ObstacleCtrl
{

    float speed = 3f;
    Vector2 dir;
    Rigidbody2D rb;
    Vector3 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Quaternion angle = Quaternion.Euler(0, 0, Random.Range(0, 360f));
        transform.rotation = angle;
        dir = new Vector2(transform.up.x, transform.up.y);
        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        Move();
    }


    void Move()
    {
        rb.MovePosition(rb.position + (dir * speed * Time.fixedDeltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (playerCtrl == null)
            {
                playerCtrl = collision.GetComponent<PlayerCtrl>();
            }
            playerCtrl.TakeDamage(10);
        }
        if (collision.tag == "Wall")
        {
            transform.position = startPosition;
            Quaternion angle = Quaternion.Euler(0, 0, Random.Range(0, 360f));
            transform.rotation = angle;
            dir = new Vector2(transform.up.x, transform.up.y);
        }
    }
}
