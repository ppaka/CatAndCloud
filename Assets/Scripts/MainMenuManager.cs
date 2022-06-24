using System;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1;
    }

    public void OnClickPlay()
    {
        GameManager.players = 1;
        SceneLoader.Instance.ChangeScene("GameScene");
    }
    
    public void OnClickPlayCoop()
    {
        GameManager.players = 2;
        SceneLoader.Instance.ChangeScene("GameScene");
    }
    
    public void OnClickExit()
    {
        Application.Quit();
    }
}
