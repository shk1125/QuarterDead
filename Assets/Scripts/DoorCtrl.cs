using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCtrl : MonoBehaviour
{
    bool isOpened;

    Animator anim;
    PlayerCtrl playerCtrl;


    private void Start()
    {
        anim = GetComponent<Animator>();
        isOpened = false;
    }

    public virtual void OpenDoor()
    {
        if (!isOpened)
        {
            anim.SetTrigger("open");
            isOpened = true;
            Invoke("CloseDoor", 3.0f);
        }
    }

    public virtual void CloseDoor()
    {
        anim.SetTrigger("close");
    }

    public void DoorClosed()
    {
        isOpened = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerCtrl = collision.GetComponent<PlayerCtrl>();
            playerCtrl.doorCtrl = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerCtrl.doorCtrl = null;
        }
    }




}
