using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicCtrl : MonoBehaviour
{

    AudioSource backgroundMusic;


    void Awake()
    {
        var remainObject = FindObjectsOfType<BackgroundMusicCtrl>();
        if(remainObject.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        backgroundMusic = GetComponent<AudioSource>();
        backgroundMusic.Play();
    }


}
