using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCtrl : MonoBehaviour
{
    public PlayerCtrl playerCtrl;
    public Animator anim;
    public Rigidbody2D rb;

    protected SpriteRenderer sp;
    protected AudioSource audioSource;
    protected Vector2 dir;
    

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }



    public void PlayChaseSound()
    {
        audioSource.Play();
    }

}
