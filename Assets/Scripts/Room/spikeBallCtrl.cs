using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeBallCtrl : ObstacleCtrl
{
    

    float RotateSpeed = 2.5f;
    float Radius = 2.5f;

    Vector2 centre;
    float angle;


    private void Awake()
    {
        centre = transform.position;
    }



    void Update()
    {
        if(gameObject.activeSelf)
        {
            Move();
        }
    }


    void Move()
    {
        angle += RotateSpeed * Time.deltaTime;

        var offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * Radius;
        transform.position = centre + offset;
    }
}
