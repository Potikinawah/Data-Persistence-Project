using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    [SerializeField] TextMeshProUGUI bestPlayerAndScore;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    // static variables to track the best player et score
    static int bestScore;
    static string bestPlayer;


    void Awake()
    {
        LoadRank();
    }

    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        SetBestRank();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        DataManager.Instance.score = m_Points;
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        CheckRank();    
    }

    void CheckRank()
    {
        int currentScore = DataManager.Instance.score;

        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            bestPlayer = DataManager.Instance.playerName;
            bestPlayerAndScore.text = $"Best Score - {bestPlayer} : {bestScore}";

            SaveRank();
        }
    }

    void SetBestRank()
    {
        if (bestPlayer == null && bestScore == 0)
        {
            bestPlayerAndScore.text = "";
        }
        else
        {
            bestPlayerAndScore.text = $"Best Score - {bestPlayer} : {bestScore}";
        }
    }

    [System.Serializable]
    class RankSaveData
    {
        public int bestScore;
        public string bestPlayer;
    }

    public void SaveRank()
    {
        RankSaveData data = new RankSaveData();
        data.bestScore = bestScore;
        data.bestPlayer = bestPlayer;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/rankSaveFile.json", json);
    }

    public void LoadRank()
    {
        string path = Application.persistentDataPath + "/rankSaveFile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            RankSaveData data = JsonUtility.FromJson<RankSaveData>(json);

            bestScore = data.bestScore;
            bestPlayer = data.bestPlayer;
        }
    }

}
