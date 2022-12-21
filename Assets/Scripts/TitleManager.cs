using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class TitleManager : MonoBehaviour
{
    public InputField inputHorizontal, inputVertical;
    public Dropdown inputDifficulty;
    public Text errorText;
    public static TitleManager instance;
    public GameObject titlePanel, howToPlayPanel, settingPanel, scorePanel, resetScorePanel;
    public Image howToPlayImage;
    public List<Sprite> howToPlayImageList;
    public GameObject howToPlayNextButton, howToPlayPreviousButton;
    public GameObject scoreTextPrefab;
    public GameObject scrollviewContent;

    int howToPlayImageNum;




    public static class GameSetting
    {
        public static int roomHorizontal;
        public static int roomVertical;
        public static int difficulty;
    }

    List<ScoreClass> scoreList;

    [System.Serializable]
    class ScoreClass
    {
        public ScoreClass(int roomHorizontal, int roomVertical, float playTime, string difficulty)
        {
            this.roomHorizontal = roomHorizontal;
            this.roomVertical = roomVertical;
            this.playTime = playTime;
            this.difficulty = difficulty;
        }
        public int roomHorizontal, roomVertical;
        public float playTime;
        public string difficulty;
    }


    [System.Serializable]
    class ScoreListClass
    {
        public ScoreListClass(List<ScoreClass> scoreList)
        {
            this.scoreList = scoreList;
        }

        public List<ScoreClass> scoreList;
    }


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }


    void Start()
    {

        SetScorePanel();


#if UNITY_ANDROID
howToPlayImageList.RemoveAt(0);
#endif



        howToPlayImageNum = 0;
        howToPlayImage.sprite = howToPlayImageList[howToPlayImageNum];
    }

    public void StartGame()
    {
        if (inputHorizontal.text.Length != 0 && inputVertical.text.Length != 0)
        {

            GameSetting.roomHorizontal = int.Parse(inputHorizontal.text);
            GameSetting.roomVertical = int.Parse(inputVertical.text);
            GameSetting.difficulty = inputDifficulty.value;

            if (GameSetting.roomHorizontal > 2 && GameSetting.roomVertical > 2)
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(ErrorTextEnable("방의 크기는 3 X 3 이상이어야 합니다."));
            }


        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(ErrorTextEnable("방의 크기를 입력하세요."));
        }

    }



    IEnumerator ErrorTextEnable(string error)
    {
        errorText.text = error;
        errorText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        errorText.gameObject.SetActive(false);
    }



    public void UseSetting()
    {
        titlePanel.SetActive(!titlePanel.activeSelf);
        settingPanel.SetActive(!settingPanel.activeSelf);
    }

    public void HowToPlay()
    {
        howToPlayImageNum = 0;
        howToPlayImage.sprite = howToPlayImageList[howToPlayImageNum];
        howToPlayPreviousButton.SetActive(false);
        howToPlayNextButton.SetActive(true);

        titlePanel.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

    public void HowToPlay_NextImage()
    {
        howToPlayImageNum++;
        howToPlayImage.sprite = howToPlayImageList[howToPlayImageNum];

        if (howToPlayImageNum == 1)
        {
            howToPlayPreviousButton.SetActive(true);
        }
        if (howToPlayImageNum == (howToPlayImageList.Count - 1))
        {
            howToPlayNextButton.SetActive(false);
        }

    }

    public void HowToPlay_PreviousImage()
    {
        howToPlayImageNum--;
        howToPlayImage.sprite = howToPlayImageList[howToPlayImageNum];

        if (howToPlayImageNum == 0)
        {
            howToPlayPreviousButton.SetActive(false);
        }
        if (howToPlayImageNum == (howToPlayImageList.Count - 2))
        {
            howToPlayNextButton.SetActive(true);
        }

    }

    public void UseScore()
    {
        titlePanel.SetActive(!titlePanel.activeSelf);
        scorePanel.SetActive(!scorePanel.activeSelf);
    }

    public void BackToTitle_HowToPlay()
    {
        howToPlayPanel.SetActive(false);
        titlePanel.SetActive(true);
    }





    void SetScorePanel()
    {

        string path = Application.persistentDataPath + "/" + "Score" + ".Json";
        FileInfo file = new FileInfo(path);
        

        if(file.Exists)
        {
            string scoreListFromJson = File.ReadAllText(path);
            scoreList = JsonUtility.FromJson<ScoreListClass>(scoreListFromJson).scoreList;
            if (scoreList.Count != 0)
            {
                for (int i = 0; i < scoreList.Count; i++)
                {
                    GameObject score = Instantiate(scoreTextPrefab, scrollviewContent.transform);
                    

                    Text scoreText = score.transform.GetChild(0).GetComponent<Text>();


                    scoreText.text = "방 크기 : " + scoreList[i].roomHorizontal.ToString() + " X " + scoreList[i].roomVertical.ToString()
                        + "     " + "난이도 : " + scoreList[i].difficulty
                        + "     " + "플레이 시간 : " + scoreList[i].playTime.ToString("F1") + "초";
                }
            }
        }

    }

    public void UseResetScore()
    {
        resetScorePanel.SetActive(!resetScorePanel.activeSelf);
        scorePanel.SetActive(!scorePanel.activeSelf);
    }

    public void ResetScore()
    {
        
        scoreList.Clear();
        string path = Application.persistentDataPath + "/" + "Score" + ".Json";
        FileInfo file = new FileInfo(path);

        if (file.Exists)
        {
            ScoreListClass scoreListClass = new ScoreListClass(scoreList);
            string scoreListToJson = JsonUtility.ToJson(scoreListClass);
            File.WriteAllText(path, scoreListToJson);
        }


        for(int i = 0; i < scrollviewContent.transform.childCount; i++)
        {
            Destroy(scrollviewContent.transform.GetChild(i).gameObject);
        }
        

        resetScorePanel.SetActive(!resetScorePanel.activeSelf);
        titlePanel.SetActive(!titlePanel.activeSelf);
        
    }


    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
