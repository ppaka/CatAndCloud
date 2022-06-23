using System;
using Cinemachine;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public PlayerController player1, player2;
    public CinemachineTargetGroup group;

    private void Start()
    {
        player1.gameObject.SetActive(true);
        if (GameManager.players == 2)
        {
            player2.gameObject.SetActive(true);
        }
        else if (GameManager.players == 1)
        {
            group.m_Targets[1].target = null;
            group.m_Targets[0].radius = 3;
        }
    }
}