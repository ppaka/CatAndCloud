using System;
using TMPro;
using UnityEngine;

public class LastCloud : MonoBehaviour
{
    private int _playerCount;
    public TMP_Text alertText;

    private void Update()
    {
        if (_playerCount == 2)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // start
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _playerCount++;
            if (_playerCount == 2)
            {
                alertText.gameObject.SetActive(true);
                alertText.text = "준비가 되었다면 Space키를 눌러주세요";
            }
            else if (_playerCount == 1)
            {
                alertText.gameObject.SetActive(true);
                alertText.text = "다른 플레이어를 기다리는 중";
            }
            else
            {
                alertText.gameObject.SetActive(false);
            }
        }
    }
}