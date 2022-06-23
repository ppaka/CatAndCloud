using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
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
