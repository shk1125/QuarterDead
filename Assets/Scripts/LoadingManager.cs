using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{

    public Image loadingBar;
    
    void Start()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadScene());
    }

    
    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation asyncScene = SceneManager.LoadSceneAsync(2);
        asyncScene.allowSceneActivation = false;
        float timeC = 0;
        while(!asyncScene.isDone)
        {
            yield return null;

            timeC += Time.deltaTime;

            if(asyncScene.progress >= 0.9f)
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1, timeC);
                if(loadingBar.fillAmount == 1.0f)
                {
                    asyncScene.allowSceneActivation = true;
                }
            }
            else
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, asyncScene.progress, timeC);
                if(loadingBar.fillAmount >= asyncScene.progress)
                {
                    timeC = 0;
                }
            }
        }
    }
}
