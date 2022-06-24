using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

enum AttackMode
{
    FireBullet,
    FireLaser,
    HorizontalLaser,
    BigAndSmallLaser
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
    public Transform[] lasers;
    public Transform[] horizontalLasers;
    public Transform bigLaser;
    public Transform[] smallLasers;

    private PlayerController[] _playerControllers;

    public Image lightImage;

    public BossRoomManager bossRoom;

    public SpriteRenderer spriteRenderer;

    public void GetDamage(int dmg)
    {
        curHp -= 100 / (100 + def) * dmg;
        if (curHp <= 0)
        {
            StopCoroutine(_coroutine);
            StartCoroutine(GameEnd());
        }
    }

    private IEnumerator GameEnd()
    {
        foreach (var laser in lasers)
        {
            laser.gameObject.SetActive(false);
        }
        foreach (var laser in horizontalLasers)
        {
            laser.gameObject.SetActive(false);
        }
        Time.timeScale = 0;
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
        yield return new DOTweenCYInstruction.WaitForCompletion(DialogueManager.Instance.SetText("!!!!!!"));
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        DialogueManager.Instance.FadeOut();
        yield return new DOTweenCYInstruction.WaitForCompletion(DialogueManager.Instance.Close());
        DialogueManager.Instance.dialogText.text = "";
        virtualCam.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(1.5f);
        lightImage.DOFade(1, 5).SetUpdate(true);
        yield return new DOTweenCYInstruction.WaitForCompletion(bossRoom.roomVCam
            .DOShakePosition(5, 1f, 20, 90f, false, false).SetEase(Ease.Linear).SetUpdate(true));

        SceneLoader.Instance.ChangeSceneImmediate("Ending");
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
        hpImage.fillAmount = (float) curHp / maxHp;
    }

    public void StartAttack()
    {
        _coroutine = StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            AttackMode[] modes = {AttackMode.FireBullet, AttackMode.FireLaser, AttackMode.FireBullet, AttackMode.HorizontalLaser, AttackMode.BigAndSmallLaser};
            var  currentAttackMode = modes[Random.Range(0, modes.Length)];
            yield return new WaitForSeconds(1.8f);


            if (currentAttackMode == AttackMode.FireBullet)
            {
                spriteRenderer.flipX = false;
                transform.position = teleportPosition[Random.Range(0, teleportPosition.Length)].position;
                for (int j = 0; j < 30; j++)
                {
                    for (var i = 0; i < GameManager.players; i++)
                    {
                        if (!_playerControllers[i]) continue;
                        var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                        bullet.moveDirection =
                            (_playerControllers[i].transform.position - transform.position).normalized;
                    }

                    yield return new WaitForSeconds(0.1f);
                    if (j == 9) yield return new WaitForSeconds(0.9f);
                    else if (j == 19) yield return new WaitForSeconds(0.9f);
                    else if (j == 29) yield return new WaitForSeconds(0.9f);
                }
            }
            else if (currentAttackMode == AttackMode.FireLaser)
            {
                spriteRenderer.flipX = true;
                transform.position = teleportPosition[Random.Range(0, teleportPosition.Length)].position;
                for (var i = 0; i < lasers.Length; i++)
                {
                    lasers[i].gameObject.SetActive(true);

                    bossRoom.roomVCam.DOKill(true);
                    bossRoom.roomVCam
                        .DOShakePosition(0.5f, 1f, 8, 90f, false, true).SetEase(Ease.Linear).SetUpdate(true);
                    yield return new WaitForSeconds(0.8f);
                }

                foreach (var laser in lasers)
                {
                    laser.gameObject.SetActive(false);
                }
                yield return new WaitForSeconds(1f);

                foreach (var laser in lasers)
                {
                    laser.gameObject.SetActive(true);
                    bossRoom.roomVCam.DOKill(true);
                    bossRoom.roomVCam
                        .DOShakePosition(0.2f, 1f, 4, 90f, false, true).SetEase(Ease.Linear).SetUpdate(true);
                }
                yield return new WaitForSeconds(1f);
                foreach (var laser in lasers)
                {
                    laser.gameObject.SetActive(false);
                }
            }
            else if (currentAttackMode == AttackMode.HorizontalLaser)
            {
                spriteRenderer.flipX = false;
                transform.position = teleportPosition[Random.Range(0, teleportPosition.Length)].position;
                if (Random.value > 0.5f)
                {
                    for (var i = 0; i < horizontalLasers.Length; i++)
                    {
                        horizontalLasers[i].gameObject.SetActive(true);

                        bossRoom.roomVCam.DOKill(true);
                        bossRoom.roomVCam
                            .DOShakePosition(0.2f, 1f, 4, 90f, false, true).SetEase(Ease.Linear).SetUpdate(true);
                        yield return new WaitForSeconds(0.4f);
                    }
                }
                else
                {
                    for (var i = horizontalLasers.Length - 1; i >= 2; i--)
                    {
                        horizontalLasers[i].gameObject.SetActive(true);

                        bossRoom.roomVCam.DOKill(true);
                        bossRoom.roomVCam
                            .DOShakePosition(0.2f, 0.7f, 4, 90f, false, true).SetEase(Ease.Linear).SetUpdate(true);
                        yield return new WaitForSeconds(0.23f);
                    }
                }
                
                yield return new WaitForSeconds(0.4f);
                for (var i = 0; i < horizontalLasers.Length; i++)
                {
                    horizontalLasers[i].gameObject.SetActive(false);
                }
                yield return new WaitForSeconds(0.8f);

                for (var i = 0; i < horizontalLasers.Length; i++)
                {
                    horizontalLasers[i].gameObject.SetActive(true);
                }
                bossRoom.roomVCam.DOKill(true);
                bossRoom.roomVCam
                    .DOShakePosition(0.2f, 1.2f, 5, 90f, false, true).SetEase(Ease.Linear).SetUpdate(true);
                yield return new WaitForSeconds(1);
                for (var i = 0; i < horizontalLasers.Length; i++)
                {
                    horizontalLasers[i].gameObject.SetActive(false);
                }
            }
            else
            {
                spriteRenderer.flipX = true;
                transform.position = teleportPosition[Random.Range(0, teleportPosition.Length)].position;
                bigLaser.gameObject.SetActive(true);
                bigLaser.localScale = new Vector3(0.5f, bigLaser.localScale.y, bigLaser.localScale.z);
                bossRoom.roomVCam.DOKill(true);
                bossRoom.roomVCam
                    .DOShakePosition(0.2f, 1f, 4, 90f, false, true).SetEase(Ease.Linear).SetUpdate(true);
                yield return new DOTweenCYInstruction.WaitForCompletion(bigLaser.DOScaleX(6.95f, 0.7f).SetUpdate(true));
                yield return new WaitForSeconds(0.6f);
                smallLasers[0].gameObject.SetActive(true);
                smallLasers[1].gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);
                bigLaser.gameObject.SetActive(false);
                smallLasers[0].gameObject.SetActive(false);
                smallLasers[1].gameObject.SetActive(false);
            }
            yield return null;
        }
    }
}