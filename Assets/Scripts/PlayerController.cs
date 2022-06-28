using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigid2D;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    float jumpForce = 680;
    public float walkForce = 30;

    private int curHp;
    private int maxHp = 2000;

    float maxWalkSpeed = 2;
    private static readonly int IsJumping = Animator.StringToHash("isJumping");

    public GameObject worldOutBottomLine;
    public Transform spawnPoint;
    private bool _isJumping, _isDowning;

    public KeyCode jumpKey = KeyCode.Space,
        leftKey = KeyCode.A,
        rightKey = KeyCode.D,
        downKey = KeyCode.S,
        attackKey = KeyCode.F;

    private CloudGenerator _cloudGenerator;
    public bool canMove = true;
    public bool canAttack = true;

    public Image hpImage;
    public GameObject hpCanvasObject;

    public int atk = 100;
    public LayerMask bossLayer;
    public Transform[] attackTf;

    public Another boss;

    private float timeSinceLastHit = 100, timeSinceLastFire = 100;
    private float _invincibleEffectTime = 0.7f, _invincibleTime = 0.7f;
    private float _defaultTime = 0.7f;

    private void Awake()
    {
        GameManager.alivePlayers = GameManager.players;
    }

    private void OnDestroy()
    {
        GameManager.alivePlayers--;
    }

    private void Attack()
    {
        if (!_spriteRenderer.flipX)
        {
            var col = Physics2D.OverlapCircle(attackTf[0].position, 0.8f, bossLayer);
            if (!col) return;
            boss.GetDamage(atk);
        }
        else
        {
            var col = Physics2D.OverlapCircle(attackTf[1].position, 0.8f, bossLayer);
            if (!col) return;
            boss.GetDamage(atk);
        }
    }

    public bool isInvicible()
    {
        if (timeSinceLastHit < _invincibleTime)
        {
            return true;
        }

        return false;
    }

    public void GetDamage(int dmg)
    {
        if (isInvicible()) return;
        curHp -= dmg;
        print(curHp);

        timeSinceLastHit = 0;
        _invincibleTime = _defaultTime;
        _invincibleEffectTime =_defaultTime;
        hpImage.fillAmount = (float) curHp / maxHp;
        if (curHp <= 0) Destroy(gameObject);
    }

    private void Start()
    {
        _rigid2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Application.targetFrameRate = 60;
        _cloudGenerator = FindObjectOfType<CloudGenerator>();
        spawnPoint.position = _cloudGenerator.firstCloudSpawnPosition;
        transform.position = _cloudGenerator.firstCloudSpawnPosition;
        curHp = maxHp;
    }

    public void Jump()
    {
        _rigid2D.AddForce(transform.up * jumpForce);
        _animator.SetBool(IsJumping, true);
        _isJumping = true;
        _isDowning = false;
    }

    private void Clock()
    {
        timeSinceLastFire += Time.deltaTime;
        timeSinceLastHit += Time.deltaTime;
    }

    private void InvinsibleEffect()
    {
        if (timeSinceLastHit > _invincibleEffectTime)
        {
            var color = _spriteRenderer.color;
            color.a = 1;
            _spriteRenderer.color = color;
        }
        else
        {
            var color = _spriteRenderer.color;
            color.a = Mathf.Cos(timeSinceLastHit * 20f);
            _spriteRenderer.color = color;
        }
    }

    private void Update()
    {
        Clock();
        InvinsibleEffect();

        if (canAttack)
        {
            if (Input.GetKeyDown(attackKey) && !_isJumping)
            {
                Attack();
            }
        }

        if (canMove)
        {
            if (Input.GetKeyDown(jumpKey) && !_isJumping && !_isDowning)
            {
                Jump();
            }

            if (Input.GetKeyDown(downKey) && _isJumping && !_isDowning)
            {
                _rigid2D.AddForce(-transform.up * jumpForce / 1.2f);
                _isDowning = true;
            }

            var key = 0;
            if (Input.GetKey(rightKey)) key = 1;
            if (Input.GetKey(leftKey)) key = -1;

            if (key < 0)
            {
                _spriteRenderer.flipX = true;
            }
            else if (key > 0)
            {
                _spriteRenderer.flipX = false;
            }

            var speedx = Mathf.Abs(_rigid2D.velocity.x);
            if (speedx < maxWalkSpeed)
            {
                _rigid2D.AddForce(transform.right * key * walkForce);
            }

            if (key != 0)
            {
                //transform.localScale = new Vector3(key, 1, 1);
            }

            _animator.speed = speedx * 0.5f;

            if (_animator.GetBool(IsJumping))
            {
                _animator.speed = 1;
            }
        }

        if (worldOutBottomLine.transform.position.y > transform.position.y)
        {
            _rigid2D.velocity = new Vector2(0, 0);
            transform.position = spawnPoint.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Cloud"))
        {
            if (_animator == null) _animator = GetComponent<Animator>();
            _animator.SetBool(IsJumping, false);
            _isJumping = false;
            _isDowning = false;
        }
        else if (col.CompareTag("Player"))
        {
            if (_animator == null) _animator = GetComponent<Animator>();
            _animator.SetBool(IsJumping, false);
            _isJumping = false;
            _isDowning = false;
        }
        else if (col.CompareTag("Boss"))
        {
            if (_animator == null) _animator = GetComponent<Animator>();
            _animator.SetBool(IsJumping, false);
            _isJumping = false;
            _isDowning = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            GetDamage(boss.attack);
        }
        else if (other.CompareTag("Laser"))
        {
            GetDamage(boss.attack);
        }
    }
}