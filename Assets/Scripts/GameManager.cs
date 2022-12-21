using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;

    public class GameSetting
    {
        public int roomHorizontal;
        public int roomVertical;
        public int difficulty;
    }

    public Transform canvasTransform;
    public GameObject[] roomPrefabs;
    public List<GameObject> obstaclePrefabs;
    public GameObject playerPrefab;
    public GameObject foodPrefab;
    public GameObject goalPrefab;
    public GameObject shoeImagePrefab;
    public GameObject joystickPrefab;
    public Camera mapCamera;
    public GameObject mapPanel, menuPanel, restartPanel, titlePanel, gameOverPanel;
    public Image healthBar;
    public Text healthText;
    public Text map_PlaytimeText, map_RoomsizeAndDifficultyText;
    public Text menu_PlaytimeText;
    public Text gameOver_PlaytimeText, gameOverText;
    public RawImage minimap;
    public GameObject menuButton, fireShoeButton, openDoorButton;
    public GameObject ShoesScrollviewContent;


    float playTime;
    GameObject[,] rooms;
    List<Image> shoeImageList;
    Color fireShoeColor = new Color(1, 1, 1, 0.5f);
    Color takeShoeColor = new Color(1, 1, 1, 1);
    bool isUsingRestartOrTitle, isUsingMenu, isUsingMap;
    PlayerCtrl playerCtrl;
    JoystickCtrl joystickCtrl;


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


        SetGame();
    }

    void Start()
    {


        Application.targetFrameRate = 60;
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;


        playTime = 0;
        isUsingRestartOrTitle = false;
        isUsingMenu = false;
        isUsingMap = false;


#if UNITY_ANDROID
menuButton.SetActive(true);
fireShoeButton.SetActive(true);
openDoorButton.SetActive(true);
minimap.GetComponent<Button>().enabled = true;
mapPanel.GetComponent<Button>().enabled = true;
#endif

    }


    void Update()
    {
        playTime += Time.deltaTime;

        map_PlaytimeText.text = "플레이 시간 : " + (playTime % 60).ToString("0") + " 초";
    }




    public void SetShoesUI(int shoesCount)
    {
        for (int i = 0; i < shoesCount; i++)
        {
            Image shoeImage = Instantiate(shoeImagePrefab, ShoesScrollviewContent.transform).GetComponent<Image>();
            shoeImageList.Add(shoeImage);
        }
        mapPanel.transform.SetAsLastSibling();
    }

    public void FireShoe(int shoeNum)
    {
        shoeImageList[shoeNum].color = fireShoeColor;
    }

    public void TakeShoe(int shoeNum)
    {
        shoeImageList[shoeNum].color = takeShoeColor;
    }


    public void FireShoeMobile()
    {
        playerCtrl.FireShoeMobile();
    }

    public void OpenDoorMobile()
    {
        playerCtrl.OpenDoorMobile();
    }

    public void UseMap()
    {
        if (!isUsingRestartOrTitle && !isUsingMenu)
        {

            mapPanel.SetActive(!mapPanel.activeSelf);
            isUsingMap = !isUsingMap;
        }
    }

    public void UseMenu()
    {
        if (!isUsingRestartOrTitle)
        {
            menu_PlaytimeText.text = "플레이 시간 : " + (playTime % 60).ToString("0") + " 초";
            healthBar.transform.parent.gameObject.SetActive(!healthBar.transform.parent.gameObject.activeSelf);
            for (int i = 0; i < shoeImageList.Count; i++)
            {
                shoeImageList[i].enabled = !shoeImageList[i].enabled;
            }
            minimap.enabled = !minimap.enabled;

            mapPanel.SetActive(false);
            isUsingMap = false;
            menuPanel.SetActive(!menuPanel.activeSelf);
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            isUsingMenu = !isUsingMenu;

#if UNITY_ANDROID
menuButton.SetActive(!menuButton.activeSelf);
fireShoeButton.SetActive(!fireShoeButton.activeSelf);
joystickCtrl.gameObject.SetActive(!joystickCtrl.gameObject.activeSelf);
openDoorButton.SetActive(!openDoorButton.activeSelf);
#endif



        }
    }


    void SetGame()
    {

        rooms = new GameObject[TitleManager.GameSetting.roomHorizontal, TitleManager.GameSetting.roomVertical];
        Vector3 roomPosition;

        rooms[0, 0] = Instantiate(roomPrefabs[0]);
        roomPosition = rooms[0, 0].transform.position;
        for (int i = 1; i < TitleManager.GameSetting.roomVertical - 1; i++)
        {
            rooms[0, i] = Instantiate(roomPrefabs[1]);
            rooms[0, i].transform.position += new Vector3(16f * i, 0, 0);
            roomPosition = rooms[0, i].transform.position;
        }
        rooms[0, TitleManager.GameSetting.roomVertical - 1] = Instantiate(roomPrefabs[2]);
        rooms[0, TitleManager.GameSetting.roomVertical - 1].transform.position = roomPosition + new Vector3(16f, 0, 0);
        for (int j = 1; j < TitleManager.GameSetting.roomHorizontal - 1; j++)
        {
            rooms[j, 0] = Instantiate(roomPrefabs[3]);
            rooms[j, 0].transform.position += new Vector3(0, -8f * j, 0);
        }
        roomPosition = rooms[1, 0].transform.position;
        for (int k = 1; k < TitleManager.GameSetting.roomHorizontal - 1; k++)
        {
            for (int q = 1; q < TitleManager.GameSetting.roomVertical - 1; q++)
            {
                rooms[k, q] = Instantiate(roomPrefabs[4]);
                rooms[k, q].transform.position = roomPosition + new Vector3(16f * q, -8f * (k - 1), 0);
            }
        }
        roomPosition = rooms[0, TitleManager.GameSetting.roomVertical - 1].transform.position;
        for (int s = 1; s < TitleManager.GameSetting.roomHorizontal - 1; s++)
        {
            rooms[s, TitleManager.GameSetting.roomVertical - 1] = Instantiate(roomPrefabs[5]);
            rooms[s, TitleManager.GameSetting.roomVertical - 1].transform.position = roomPosition + new Vector3(0, -8f * s, 0);
        }



        roomPosition = rooms[TitleManager.GameSetting.roomHorizontal - 2, 0].transform.position;

        rooms[TitleManager.GameSetting.roomHorizontal - 1, 0] = Instantiate(roomPrefabs[6]);
        rooms[TitleManager.GameSetting.roomHorizontal - 1, 0].transform.position = roomPosition + new Vector3(0, -8f, 0);


        roomPosition = rooms[TitleManager.GameSetting.roomHorizontal - 1, 0].transform.position;
        for (int d = 1; d < TitleManager.GameSetting.roomVertical - 1; d++)
        {
            rooms[TitleManager.GameSetting.roomHorizontal - 1, d] = Instantiate(roomPrefabs[7]);
            rooms[TitleManager.GameSetting.roomHorizontal - 1, d].transform.position = roomPosition + new Vector3(16f * d, 0, 0);
        }


        roomPosition = rooms[TitleManager.GameSetting.roomHorizontal - 1, TitleManager.GameSetting.roomVertical - 2].transform.position;
        rooms[TitleManager.GameSetting.roomHorizontal - 1, TitleManager.GameSetting.roomVertical - 1] = Instantiate(roomPrefabs[8]);
        rooms[TitleManager.GameSetting.roomHorizontal - 1, TitleManager.GameSetting.roomVertical - 1].transform.position = roomPosition + new Vector3(16f, 0, 0);



        mapCamera.transform.position = new Vector3((rooms[0, 0].transform.position.x + rooms[0, TitleManager.GameSetting.roomVertical - 1].transform.position.x) / 2f,
            (rooms[0, 0].transform.position.y + rooms[TitleManager.GameSetting.roomHorizontal - 1, 0].transform.position.y) / 2f, -10f);

        int roomSizeX = rooms.GetLength(0);
        int roomSizeY = rooms.GetLength(1);




        if (roomSizeX == 3 && roomSizeY == 3)
        {
            mapCamera.orthographicSize = 13f;
        }
        if (roomSizeY > 3)
        {
            mapCamera.orthographicSize = 16f;
        }
        if (roomSizeX > 3)
        {
            mapCamera.orthographicSize = 21f;
        }
        if (roomSizeY > 5)
        {
            mapCamera.orthographicSize = 22f;
        }
        if (roomSizeY > 7)
        {
            mapCamera.orthographicSize = 28f;
        }
        if (roomSizeX > 5)
        {
            mapCamera.orthographicSize = 30f;
        }
        if (roomSizeX > 7)
        {
            mapCamera.orthographicSize = 37f;
        }


        bool isPlayerInstantiated = false;
        int playerPercentage = rooms.Length;

        bool isGoalInstantiated = false;
        int goalPercentage = rooms.Length;

        int foodPercentage = 30;

        int obstaclesCount = TitleManager.instance.inputDifficulty.value + 2;

        for (int i = 0; i < rooms.GetLength(0); i++)
        {
            for (int j = 0; j < rooms.GetLength(1); j++)
            {
                if (i == 0 && j == 0)
                {
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i, j + 1].GetComponent<RoomCtrl>());
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i + 1, j].GetComponent<RoomCtrl>());
                }
                else if (i == 0 && j != 0 && j != (rooms.GetLength(1) - 1))
                {
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i, j - 1].GetComponent<RoomCtrl>());
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i, j + 1].GetComponent<RoomCtrl>());
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i + 1, j].GetComponent<RoomCtrl>());
                }
                else if (i == 0 && j == (rooms.GetLength(1) - 1))
                {
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i, j - 1].GetComponent<RoomCtrl>());
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i + 1, j].GetComponent<RoomCtrl>());
                }
                else if (i != 0 && i != (rooms.GetLength(0) - 1) && j == 0)
                {
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i - 1, j].GetComponent<RoomCtrl>());
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i, j + 1].GetComponent<RoomCtrl>());
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i + 1, j].GetComponent<RoomCtrl>());
                }
                else if (i != 0 && i != (rooms.GetLength(0) - 1) && j != 0 && j != (rooms.GetLength(1) - 1))
                {
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i - 1, j].GetComponent<RoomCtrl>());
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i + 1, j].GetComponent<RoomCtrl>());
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i, j - 1].GetComponent<RoomCtrl>());
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i, j + 1].GetComponent<RoomCtrl>());
                }
                else if (i != 0 && i != (rooms.GetLength(0) - 1) && j == (rooms.GetLength(1) - 1))
                {
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i - 1, j].GetComponent<RoomCtrl>());
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i, j - 1].GetComponent<RoomCtrl>());
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i + 1, j].GetComponent<RoomCtrl>());
                }
                else if (i == (rooms.GetLength(0) - 1) && j == 0)
                {
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i - 1, j].GetComponent<RoomCtrl>());
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i, j + 1].GetComponent<RoomCtrl>());
                }
                else if (i == (rooms.GetLength(0) - 1) && j != 0 && j != (rooms.GetLength(1) - 1))
                {
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i, j - 1].GetComponent<RoomCtrl>());
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i - 1, j].GetComponent<RoomCtrl>());
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i, j + 1].GetComponent<RoomCtrl>());
                }
                else if (i == (rooms.GetLength(0) - 1) && j == (rooms.GetLength(1) - 1))
                {
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i - 1, j].GetComponent<RoomCtrl>());
                    rooms[i, j].GetComponent<RoomCtrl>().nearRooms.Add(rooms[i, j - 1].GetComponent<RoomCtrl>());
                }






                if (!isPlayerInstantiated)
                {
                    if (i == rooms.GetLength(0) - 1 && j == rooms.GetLength(1) - 1)
                    {
                        playerCtrl = Instantiate(playerPrefab).GetComponent<PlayerCtrl>();
                        playerCtrl.transform.position = rooms[i, j].transform.position;
                        rooms[i, j].GetComponent<RoomCtrl>().isStartRoom = true;
                        continue;
                    }


                    if (Random.Range(0, playerPercentage) == 0)
                    {
                        playerCtrl = Instantiate(playerPrefab).GetComponent<PlayerCtrl>();
                        playerCtrl.transform.position = rooms[i, j].transform.position;
                        rooms[i, j].GetComponent<RoomCtrl>().isStartRoom = true;
                        isPlayerInstantiated = true;
                        continue;
                    }
                    else
                    {
                        playerPercentage--;
                    }
                }


                if (!isGoalInstantiated)
                {
                    if (i == rooms.GetLength(0) - 2 && j == rooms.GetLength(1) - 2 && isPlayerInstantiated == false)
                    {
                        InstantiateGoal(i, j);
                        isGoalInstantiated = true;
                        rooms[i, j].GetComponent<RoomCtrl>().isFinishRoom = true;
                        continue;
                    }
                    else if (i == rooms.GetLength(0) - 1 && j == rooms.GetLength(1) - 1 && isPlayerInstantiated == true)
                    {
                        InstantiateGoal(i, j);
                        rooms[i, j].GetComponent<RoomCtrl>().isFinishRoom = true;
                        continue;
                    }


                    if (Random.Range(0, goalPercentage) == 0)
                    {
                        InstantiateGoal(i, j);
                        isGoalInstantiated = true;
                        rooms[i, j].GetComponent<RoomCtrl>().isFinishRoom = true;
                        continue;
                    }
                    else
                    {
                        goalPercentage--;
                    }






                }


                if (Random.Range(0, foodPercentage) == 0)
                {
                    GameObject food = Instantiate(foodPrefab, rooms[i, j].transform);
                    rooms[i, j].GetComponent<RoomCtrl>().obstacles.Add(food);
                    continue;
                }



                List<GameObject> remainObstacles = new List<GameObject>();

                for (int x = 0; x < obstaclePrefabs.Count; x++)
                {
                    remainObstacles.Add(obstaclePrefabs[x].gameObject);
                }


                for (int k = 0; k < Random.Range(0, obstaclesCount); k++)
                {
                    int obstacleNum = Random.Range(0, remainObstacles.Count);
                    GameObject obstacle = Instantiate(remainObstacles[obstacleNum], rooms[i, j].transform);
                    rooms[i, j].GetComponent<RoomCtrl>().obstacles.Add(obstacle);
                    remainObstacles.RemoveAt(obstacleNum);
                }


            }
        }
        shoeImageList = new List<Image>();
        map_RoomsizeAndDifficultyText.text = "방 크기 : " + rooms.GetLength(0).ToString() + " X " + rooms.GetLength(1).ToString() + "\n";
        switch (TitleManager.instance.inputDifficulty.value)
        {
            case 0:
                {
                    map_RoomsizeAndDifficultyText.text += "난이도 : 쉬움";
                    break;
                }
            case 1:
                {
                    map_RoomsizeAndDifficultyText.text += "난이도 : 보통";
                    break;
                }
            case 2:
                {
                    map_RoomsizeAndDifficultyText.text += "난이도 : 어려움";
                    break;
                }
        }


#if UNITY_ANDROID

        joystickCtrl = Instantiate(joystickPrefab, canvasTransform).GetComponent<JoystickCtrl>();
        joystickCtrl.playerCtrl = playerCtrl;

#endif


        map_PlaytimeText.text = "플레이 시간 : " + (playTime % 60).ToString("0") + " 초";




    }




    public void SetHealthUI(int health, int maxHealth)
    {
        healthBar.fillAmount = (float)health / (float)maxHealth;
        healthText.text = health.ToString();
    }


    void InstantiateGoal(int i, int j)
    {
        GameObject goal = Instantiate(goalPrefab);
        goal.transform.position = rooms[i, j].transform.position;
        rooms[i, j].GetComponent<RoomCtrl>().obstacles.Add(goal);

        if (i == 0 && j == 0)
        {
            rooms[i, j + 1].GetComponent<RoomCtrl>().isFinishRoomNear = true;
            rooms[i + 1, j].GetComponent<RoomCtrl>().isFinishRoomNear = true;
        }
        else if (i == 0 && j != 0 && j != (rooms.GetLength(1) - 1))
        {
            rooms[i, j - 1].GetComponent<RoomCtrl>().isFinishRoomNear = true;
            rooms[i, j + 1].GetComponent<RoomCtrl>().isFinishRoomNear = true;
            rooms[i + 1, j].GetComponent<RoomCtrl>().isFinishRoomNear = true;
        }
        else if (i == 0 && j == (rooms.GetLength(1) - 1))
        {
            rooms[i, j - 1].GetComponent<RoomCtrl>().isFinishRoomNear = true;
            rooms[i + 1, j].GetComponent<RoomCtrl>().isFinishRoomNear = true;
        }
        else if (i != 0 && i != (rooms.GetLength(0) - 1) && j == 0)
        {
            rooms[i - 1, j].GetComponent<RoomCtrl>().isFinishRoomNear = true;
            rooms[i, j + 1].GetComponent<RoomCtrl>().isFinishRoomNear = true;
            rooms[i + 1, j].GetComponent<RoomCtrl>().isFinishRoomNear = true;
        }
        else if (i != 0 && i != (rooms.GetLength(0) - 1) && j != 0 && j != (rooms.GetLength(1) - 1))
        {
            rooms[i - 1, j].GetComponent<RoomCtrl>().isFinishRoomNear = true;
            rooms[i + 1, j].GetComponent<RoomCtrl>().isFinishRoomNear = true;
            rooms[i, j - 1].GetComponent<RoomCtrl>().isFinishRoomNear = true;
            rooms[i, j + 1].GetComponent<RoomCtrl>().isFinishRoomNear = true;
        }
        else if (i != 0 && i != (rooms.GetLength(0) - 1) && j == (rooms.GetLength(1) - 1))
        {
            rooms[i - 1, j].GetComponent<RoomCtrl>().isFinishRoomNear = true;
            rooms[i, j - 1].GetComponent<RoomCtrl>().isFinishRoomNear = true;
            rooms[i + 1, j].GetComponent<RoomCtrl>().isFinishRoomNear = true;
        }
        else if (i == (rooms.GetLength(0) - 1) && j == 0)
        {
            rooms[i - 1, j].GetComponent<RoomCtrl>().isFinishRoomNear = true;
            rooms[i, j + 1].GetComponent<RoomCtrl>().isFinishRoomNear = true;
        }
        else if (i == (rooms.GetLength(0) - 1) && j != 0 && j != (rooms.GetLength(1) - 1))
        {
            rooms[i, j - 1].GetComponent<RoomCtrl>().isFinishRoomNear = true;
            rooms[i - 1, j].GetComponent<RoomCtrl>().isFinishRoomNear = true;
            rooms[i, j + 1].GetComponent<RoomCtrl>().isFinishRoomNear = true;
        }
        else if (i == (rooms.GetLength(0) - 1) && j == (rooms.GetLength(1) - 1))
        {
            rooms[i - 1, j].GetComponent<RoomCtrl>().isFinishRoomNear = true;
            rooms[i, j - 1].GetComponent<RoomCtrl>().isFinishRoomNear = true;
        }
    }

    public void UseRestartMenu()
    {
        restartPanel.SetActive(!restartPanel.activeSelf);
        isUsingRestartOrTitle = !isUsingRestartOrTitle;
    }

    public void UseTitleMenu()
    {
        titlePanel.SetActive(!titlePanel.activeSelf);
        isUsingRestartOrTitle = !isUsingRestartOrTitle;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene(0);
    }


    public void GameOver(bool isDead)
    {
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        healthBar.transform.parent.gameObject.SetActive(false);
        for (int i = 0; i < shoeImageList.Count; i++)
        {
            shoeImageList[i].enabled = false;
        }
        minimap.enabled = false;
        mapPanel.SetActive(false);


        switch (isDead)
        {
            case true:
                {
                    gameOverText.text = "패배...";
                    break;
                }
            case false:
                {
                    gameOverText.text = "승리!";
                    SaveScore();
                    break;
                }
        }



#if UNITY_ANDROID
menuButton.SetActive(false);
fireShoeButton.SetActive(false);
joystickCtrl.gameObject.SetActive(false);
openDoorButton.SetActive(false);
#endif


        gameOver_PlaytimeText.text = "플레이 시간 : " + (playTime % 60).ToString("0") + " 초";
        gameOverPanel.SetActive(true);
    }

    void SaveScore()
    {
        string difficulty = "";
        switch (TitleManager.instance.inputDifficulty.value)
        {
            case 0:
                {
                    difficulty = "쉬움";
                    break;
                }
            case 1:
                {
                    difficulty = "보통";
                    break;
                }
            case 2:
                {
                    difficulty = "어려움";
                    break;
                }
        }

        ScoreClass score = new ScoreClass(rooms.GetLength(0), rooms.GetLength(1), playTime, difficulty);



        List<ScoreClass> scoreList = new List<ScoreClass>();
        string path = Application.persistentDataPath + "/" + "Score" + ".Json";
        FileInfo file = new FileInfo(path);

        if (file.Exists)
        {
            string scoreListFromJson = File.ReadAllText(path);
            if (JsonUtility.FromJson<ScoreListClass>(scoreListFromJson) != null)
            {
                scoreList = JsonUtility.FromJson<ScoreListClass>(scoreListFromJson).scoreList;
            }
        }



        scoreList.Add(score);
        ScoreListClass scoreListClass = new ScoreListClass(scoreList);
        string scoreListToJson = JsonUtility.ToJson(scoreListClass);
        File.WriteAllText(path, scoreListToJson);

    }

}
