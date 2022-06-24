using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
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

    public CanvasGroup group;
    public Image hpImage;

    public int curHp, maxHp = 20000;
    public int atk = 40, def;

    public Transform[] teleportPosition;

    private PlayerController[] _playerControllers;

    public Image lightImage;

    public void GetDamage(int dmg)
    {
        curHp -= 100/(100+def) * dmg;
        if (curHp <= 0)
        {
            StopCoroutine(_coroutine);
            StartCoroutine(GameEnd());
        }
    }

    private IEnumerator GameEnd()
    {
        Time.timeScale = 0;
            var bossRoom = FindObjectOfType<BossRoomManager>();
            bossRoom.lockToScreenAllPlayers = false;
        foreach (var player in _playerControllers)
        {
            player.canAttack = false;
            player.canMove = false;
            player.hpCanvasObject.SetActive(false);
        }
        group.gameObject.SetActive(false);

        yield return new DOTweenCYInstruction.WaitForCompletion(lightImage.DOFade(1, 0.08f).SetUpdate(true));
        yield return new DOTweenCYInstruction.WaitForCompletion(lightImage.DOFade(0, 0.5f).SetUpdate(true));
        yield return new WaitForSecondsRealtime(0.5f);
        virtualCam.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        DialogueManager.Instance.FadeIn();
        DialogueManager.Instance.Open();
        yield return new WaitForSecondsRealtime(0.2f);
        yield return new DOTweenCYInstruction.WaitForCompletion(DialogueManager.Instance.SetText(".....!?"));
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new DOTweenCYInstruction.WaitForCompletion(DialogueManager.Instance.SetText("!!!!!"));
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        DialogueManager.Instance.FadeOut();
        yield return new DOTweenCYInstruction.WaitForCompletion(DialogueManager.Instance.Close());
        DialogueManager.Instance.dialogText.text = "";
        virtualCam.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(1.5f);
        lightImage.DOFade(1, 5).SetUpdate(true);
        yield return new DOTweenCYInstruction.WaitForCompletion(bossRoom.roomVCam.DOShakePosition(5).SetUpdate(true));
        
        //SceneLoader.Instance.ChangeSceneImmediate();
    }
    
    private void Start()
    {
        curHp = maxHp;
        Invoke(nameof(FindPlayer), 1f);
    }

    private void FindPlayer()
    {
        _playerControllers = FindObjectsOfType<PlayerController>();
        foreach (var player in _playerControllers)
        {
            player.boss = this;
        }
    }

    private void Update()
    {
        hpImage.fillAmount = (float)curHp / maxHp;
    }

    public void StartAttack()
    {
        _coroutine = StartCoroutine(Attack());
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

            yield return new WaitForSeconds(1.8f);

            

            if (currentAttackMode == AttackMode.FireBullet)
            {
                transform.position = teleportPosition[Random.Range(0, teleportPosition.Length)].position;
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