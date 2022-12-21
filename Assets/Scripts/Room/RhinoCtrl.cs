using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoCtrl : MonsterCtrl
{
    public AudioClip hitWallSound;


    float chaseForce = 200f;

    BoxCollider2D boxCollider;
    

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        
    }

    void FixedUpdate()
    {
        if (playerCtrl != null)
        {
            Chase();
        }
    }

    void Chase()
    {
        anim.SetBool("isChasing", true);

        dir = playerCtrl.transform.position - transform.position;
        dir.Normalize();
        if (dir.x > 0)
        {
            sp.flipX = true;
        }
        else if (dir.x < 0)
        {
            sp.flipX = false;
        }
        rb.AddForce(dir * chaseForce * Time.fixedDeltaTime);
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
            anim.SetTrigger("hitWall");
            rb.velocity = Vector2.zero;

            AudioClip chaseSoundClip = audioSource.clip;
            audioSource.clip = hitWallSound;
            audioSource.Play();

            StartCoroutine(BackToChaseSound(chaseSoundClip));


        }
    }


    IEnumerator BackToChaseSound(AudioClip chaseSoundClip)
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        audioSource.clip = chaseSoundClip;
    }

}
