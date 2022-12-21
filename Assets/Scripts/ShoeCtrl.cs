using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoeCtrl : MonoBehaviour
{
    public int shoeNum;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.instance.TakeShoe(shoeNum);
            transform.parent.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
