using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncherCtrl : MonoBehaviour
{
    public GameObject missilePrefab;
    GameObject[] missilePool;

    int poolingCount = 5;
    int poolingPivot;
    float spawnTime = 2.5f;



    private void Awake()
    {
        transform.up = transform.parent.position - transform.position;
        
        missilePool = new GameObject[poolingCount];
        for (int i = 0; i < poolingCount; i++)
        {
            GameObject missile = Instantiate(missilePrefab, transform.position, transform.rotation, transform);
            missilePool[i] = missile;
        }
        poolingPivot = 0;
    }

    void Start()
    {
        StartCoroutine(FireMissile());
        
    }






    IEnumerator FireMissile()
    {
        yield return new WaitForSeconds(spawnTime);
        missilePool[poolingPivot++].SetActive(true);
        if(poolingPivot == poolingCount)
        {
            poolingPivot = 0;
        }
        StartCoroutine(FireMissile());
    }
}
