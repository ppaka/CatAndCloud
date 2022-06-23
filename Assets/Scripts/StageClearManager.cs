using System;
using UnityEngine;

public class StageClearManager : MonoBehaviour
{
    private int _triggerCount;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _triggerCount++;
            if (_triggerCount == GameManager.players)
            {
                SceneLoader.Instance.ChangeScene("GameScene_Boss");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _triggerCount--;
        }
    }
}