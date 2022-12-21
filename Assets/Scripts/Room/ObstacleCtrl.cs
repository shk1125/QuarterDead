using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCtrl : MonoBehaviour
{
    protected PlayerCtrl playerCtrl;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerCtrl = collision.GetComponent<PlayerCtrl>();
            StartCoroutine("GiveDamage");
        }
    }

    

    IEnumerator GiveDamage()
    {
        playerCtrl.TakeDamage(10);
        yield return new WaitForSeconds(3f);
        StartCoroutine("GiveDamage");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StopCoroutine("GiveDamage");
        }
    }
}
