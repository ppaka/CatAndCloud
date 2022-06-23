using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

enum AttackMode
{
    FireBullet,
    FireLaser,
    StarAttack
}

public class Another : MonoBehaviour
{
    public GameObject virtualCam;
    public BulletA bulletPrefab;
    
    private AttackMode _lastAttack;
    private Coroutine _coroutine;

    private PlayerController[] _playerControllers;

    private float _totalTime, _maxTime = 60;

    private void Start()
    {
        Invoke("FindPlayer", 2f);
    }

    private void FindPlayer()
    {
        _playerControllers = FindObjectsOfType<PlayerController>();
    }

    public void StartAttack()
    {
        StartCoroutine(Timer());
        _coroutine = StartCoroutine(Attack());
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            _totalTime += Time.deltaTime;
            print(_totalTime);
            if (_totalTime >= _maxTime)
            {
                StopCoroutine(_coroutine);
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            var currentAttackMode = AttackMode.FireBullet;
            var value = Random.value;
            for (var i = 0; i < 3; i++)
            {
                if (value < (i + 1) / 3f)
                {
                    currentAttackMode = (AttackMode)i;
                    break;
                }
                currentAttackMode = (AttackMode)i;
            }

            yield return new WaitForSeconds(2);

            if (currentAttackMode == AttackMode.FireBullet)
            {
                for (int j = 0; j < 30; j++)
                {
                    for (var i = 0; i < GameManager.players; i++)
                    {
                        var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                        bullet.moveDirection = (_playerControllers[i].transform.position - transform.position).normalized;
                    }
                    yield return new WaitForSeconds(0.1f);
                    if (j == 9) yield return new WaitForSeconds(0.9f);
                    else if (j == 19) yield return new WaitForSeconds(0.9f);
                    else if (j == 29) yield return new WaitForSeconds(0.9f);
                }
            }
            
            yield return null;
        }
    }
}