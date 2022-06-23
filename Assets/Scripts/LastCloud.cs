using System;
using TMPro;
using UnityEngine;

public class LastCloud : MonoBehaviour
{
    private int _playerCount;
    public TMP_Text alertText;
    private bool _isActivated;

    public void DestroyAllClouds()
    {
        Destroy(GameObject.Find("Clouds").gameObject);
    }

    private void Update()
    {
        if (!_isActivated)
        {
            if (_playerCount == GameManager.players)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    FindObjectOfType<BossRoomManager>().enterController.gameObject.SetActive(true);

                    var players = FindObjectsOfType<PlayerController>();
                    foreach (var player in players)
                    {
                        player.Jump();
                        player.canMove = false;
                    }

                    _isActivated = true;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _playerCount++;
            if (GameManager.players == 1)
            {
                if (_playerCount == 1)
                {
                    alertText.gameObject.SetActive(true);
                    alertText.text = "준비가 되었다면 Space키를 눌러주세요";
                }
                else
                {
                    alertText.gameObject.SetActive(false);
                }
            }
            else if (GameManager.players == 2)
            {
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

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _playerCount--;
            if (GameManager.players == 1)
            {
                if (_playerCount == 1)
                {
                    alertText.gameObject.SetActive(true);
                    alertText.text = "준비가 되었다면 Space키를 눌러주세요";
                }
                else
                {
                    alertText.gameObject.SetActive(false);
                }
            }
            else if (GameManager.players == 2)
            {
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
}