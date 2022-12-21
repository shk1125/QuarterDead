using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomCtrl : MonoBehaviour
{
    public GameObject wall_Death;
    public List<GameObject> obstacles;
    public List<MonsterCtrl> monsters;
    public List<RoomCtrl> nearRooms;
    public TextMeshPro obstaclesRoomCountText;
    public bool isStartRoom;
    public bool isFinishRoom;
    public bool isFinishRoomNear;

    bool isActive;
    int obstaclesRoomCount = 0;

    PlayerCtrl playerCtrl;
    
    

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player" || collision.tag == "Shoe")
        {

            if (Random.Range(1, 21) == 1 && !isActive && !isStartRoom && !isFinishRoom && !isFinishRoomNear)
            {
                StartCoroutine(DeathWallEnable());
            }

            if (!isActive && !isStartRoom)
            {
                for (int i = 0; i < obstacles.Count; i++)
                {
                    obstacles[i].gameObject.SetActive(true);
                }
            }
            switch (collision.tag)
            {
                case "Player":
                    if (playerCtrl == null)
                    {
                        playerCtrl = collision.GetComponent<PlayerCtrl>();
                    }
                    for (int i = 0; i < obstacles.Count; i++)
                    {
                        MonsterCtrl monster = obstacles[i].GetComponent<MonsterCtrl>();
                        if (monster != null)
                        {
                            monster.playerCtrl = playerCtrl;
                            monster.PlayChaseSound();
                            monsters.Add(monster);

                        }
                    }
                    break;
            }

            if(!obstaclesRoomCountText.gameObject.activeSelf)
            {
                for(int i = 0; i < nearRooms.Count; i++)
                {
                    if(nearRooms[i].obstacles.Count != 0 && nearRooms[i].obstacles[0].tag != "Foods")
                    {
                        obstaclesRoomCount++;
                    }

                }
                obstaclesRoomCountText.text = obstaclesRoomCount.ToString();
                obstaclesRoomCountText.gameObject.SetActive(true);
            }

            isActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.tag == "Player")
        {
            for(int i = 0; i < monsters.Count; i++)
            {
                monsters[i].playerCtrl = null;
                monsters[i].anim.SetBool("isChasing", false);
                monsters[i].rb.velocity = Vector2.zero;
                monsters[i].transform.position = monsters[i].transform.parent.position;
            }
        }
        
    }


    IEnumerator DeathWallEnable()
    {
        yield return new WaitForSeconds(0.5f);
        wall_Death.SetActive(true);
    }
}
