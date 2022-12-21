using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public float speed = 10;
    public DoorCtrl doorCtrl;
    public GameObject shoePrefab;
    public List<GameObject> shoesList;
    public int MaxHealth = 100;
    public AudioClip getHitSound, eatingSound;


    int health;
    int shoesCount = 2;
    float fireShoeForce = 300;
    float xInput, yInput;

    bool isWalkingDown, isWalkingUp, isWalkingSide;
    bool isUsingMenu;
    bool isDead;
    bool isWalking;

    Vector2 dir;

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Animator anim;
    AudioSource audioSource;




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        xInput = 0;
        yInput = 0;
        isWalking = false;
        isWalkingDown = true;
        isWalkingUp = false;
        isWalkingSide = false;
        isDead = false;
        health = MaxHealth;



        for (int i = 0; i < shoesCount; i++)
        {
            GameObject shoe = Instantiate(shoePrefab);
            shoe.transform.GetChild(0).GetComponent<ShoeCtrl>().shoeNum = i;
            shoesList.Add(shoe);

        }
        GameManager.instance.SetShoesUI(shoesCount);

    }


    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN

        if (!isDead)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                GameManager.instance.UseMap();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.instance.UseMenu();

            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (doorCtrl != null)
                {
                    doorCtrl.OpenDoor();
                }
            }
            if (Input.GetKeyDown(KeyCode.F))
            {

                for (int i = 0; i < shoesList.Count; i++)
                {
                    if (!shoesList[i].activeSelf)
                    {
                        GameManager.instance.FireShoe(i);
                        StartCoroutine(FireShoe(shoesList[i]));
                        break;
                    }
                }
            }
        }
#endif
    }

    private void FixedUpdate()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (!isDead)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                Move();
            }
            else
            {
                isWalking = false;
                anim.SetBool("isWalking", isWalking);
            }
        }
#endif
    }

    void Move()
    {
        isWalking = true;
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        dir = new Vector2(xInput, yInput);
        dir.Normalize();
        rb.MovePosition(rb.position + (dir * speed * Time.fixedDeltaTime));


        if (xInput != 0)
        {
            if (xInput > 0)
            {
                spriteRenderer.flipX = true;

            }
            else
            {
                spriteRenderer.flipX = false;
            }
            isWalkingSide = true;
            isWalkingUp = false;
            isWalkingDown = false;
        }
        if (yInput != 0)
        {
            if (yInput > 0)
            {
                isWalkingUp = true;
                isWalkingDown = false;
            }
            else
            {
                isWalkingUp = false;
                isWalkingDown = true;
            }
            isWalkingSide = false;
        }



        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isWalkingUp", isWalkingUp);
        anim.SetBool("isWalkingSide", isWalkingSide);
        anim.SetBool("isWalkingDown", isWalkingDown);


    }


    IEnumerator FireShoe(GameObject shoe)
    {
        Rigidbody2D shoeRb = shoe.GetComponent<Rigidbody2D>();
        shoe.transform.position = transform.position;
        shoe.SetActive(true);

        if (isWalkingUp)
        {
            shoeRb.AddForce(Vector2.up * fireShoeForce);
        }
        else if (isWalkingDown)
        {
            shoeRb.AddForce(Vector2.down * fireShoeForce);
        }
        else if (isWalkingSide)
        {
            if (spriteRenderer.flipX)
            {

                shoeRb.AddForce(Vector2.right * fireShoeForce);
            }
            else
            {
                shoeRb.AddForce(Vector2.left * fireShoeForce);
            }
        }

        yield return new WaitForSeconds(1f);

        shoe.transform.GetChild(0).gameObject.SetActive(true);

    }



    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            isDead = true;
            if (isWalkingUp)
            {
                anim.SetTrigger("deadUp");
            }
            else if (isWalkingDown)
            {
                anim.SetTrigger("deadDown");
            }
            else if (isWalkingSide)
            {
                anim.SetTrigger("deadSide");
            }
        }
        else
        {
            if (audioSource.clip != getHitSound)
            {
                audioSource.clip = getHitSound;
            }
            audioSource.Play();
        }

        GameManager.instance.SetHealthUI(health, MaxHealth);
    }

    public void TakeFood(int food)
    {
        if (audioSource.clip != eatingSound)
        {
            audioSource.clip = eatingSound;
        }
        audioSource.Play();
        health += food;
        if (health > MaxHealth)
        {
            health = MaxHealth;
        }
        GameManager.instance.SetHealthUI(health, MaxHealth);
    }


    public void FireShoeMobile()
    {

        for (int i = 0; i < shoesList.Count; i++)
        {
            if (!shoesList[i].activeSelf)
            {

                GameManager.instance.FireShoe(i);
                StartCoroutine(FireShoe(shoesList[i]));
                break;

            }
        }

    }


    public void OpenDoorMobile()
    {
        if (doorCtrl != null)
        {
            doorCtrl.OpenDoor();
        }
    }

    public void MoveMobile(Vector3 joystickDir)
    {
        if (!isDead)
        {

            isWalking = true;




            dir = (Vector2)joystickDir;
            rb.MovePosition(rb.position + (dir * speed * Time.fixedDeltaTime));

            if (dir.y >= -0.75f && dir.y <= 0.75f)
            {
                if (dir.x >= 0.75f)
                {
                    spriteRenderer.flipX = true;

                }
                else if (dir.x <= -0.75f)
                {
                    spriteRenderer.flipX = false;
                }
                isWalkingSide = true;
                isWalkingUp = false;
                isWalkingDown = false;
            }

            if (dir.x > -0.75f && dir.x < 0.75f)
            {
                if (dir.y > 0.75f)
                {
                    isWalkingUp = true;
                    isWalkingDown = false;
                }
                else if (dir.y < -0.75)
                {
                    isWalkingUp = false;
                    isWalkingDown = true;
                }
                isWalkingSide = false;
            }




            anim.SetBool("isWalking", isWalking);
            anim.SetBool("isWalkingUp", isWalkingUp);
            anim.SetBool("isWalkingSide", isWalkingSide);
            anim.SetBool("isWalkingDown", isWalkingDown);
        }
    }

    public void StopMobile()
    {
        isWalking = false;
        anim.SetBool("isWalking", isWalking);
    }



    public void GameOver()
    {
        GameManager.instance.GameOver(isDead);
    }


}
