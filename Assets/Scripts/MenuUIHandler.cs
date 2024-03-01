using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] TMP_InputField playerNameInput;

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

    public void SetPlayerName()
    {
        DataManager.Instance.playerName = playerNameInput.text;
    }

    public void Start()
    {
        StartCoroutine(TestName(2));
    }

    private IEnumerator TestName(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            print("MyInputIs" + playerNameInput.text);
            print("MySavedDataIs" + DataManager.Instance.playerName);
        }
    }

}
