using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{

    //Field to display the BestPlayer info
    [SerializeField] TextMeshProUGUI bestPlayerAndScore;


    //Static variables for holding the best player data
    static int bestScore;
    static string bestPlayer;

    // Field to get current PlayerName
    [SerializeField] TMP_InputField playerNameInput;

    private void Awake()
    {
        LoadRank();
        SetPlayerName();
        SetBestRank();
    }


    public void SetPlayerName()
    {
        DataManager.Instance.playerName = playerNameInput.text;
    }


    //Load infos from record json file and update BestPlayer informations to print
    //on the Menu page
    [System.Serializable]
    class RankSaveData
    {
        public int bestScore;
        public string bestPlayer;
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

    //Methods to Start and Exit the game using buttons within the UI
    //Don't forget to assign these 2 functions to the buttons in UnityEditor !!!

    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();      
#endif
    }
}
