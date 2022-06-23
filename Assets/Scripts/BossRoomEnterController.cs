using System.Collections;
using UnityEngine;

public class BossRoomEnterController : MonoBehaviour
{
    public Transform otherObjects;
    private bool _isActivated;
        
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_isActivated)
            {
                FindObjectOfType<LastCloud>().DestroyAllClouds();
                otherObjects.gameObject.SetActive(true);
                _isActivated = true;
                Destroy(gameObject);
                FindObjectOfType<BossRoomManager>().ActiveBoss();
            }
        }
    }
}