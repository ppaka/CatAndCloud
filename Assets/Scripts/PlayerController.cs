using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigid2D;
    private Animator _animator;
    float jumpForce = 680;
    public float walkForce = 30;

    float maxWalkSpeed = 2;
    private static readonly int IsJumping = Animator.StringToHash("isJumping");

    public GameObject worldOutBottomLine;
    public Transform spawnPoint;
    private bool _isJumping;
    public KeyCode jumpKey = KeyCode.Space, leftKey = KeyCode.A, rightKey = KeyCode.D;
    private CloudGenerator _cloudGenerator;

    private void Start()
    {
        _rigid2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        Application.targetFrameRate = 60;
        _cloudGenerator = FindObjectOfType<CloudGenerator>();
        spawnPoint.position = _cloudGenerator.firstCloudSpawnPosition;
        transform.position = _cloudGenerator.firstCloudSpawnPosition;
    }

    private void Update()
    {
        if (Input.GetKeyDown(jumpKey) && !_isJumping)
        {
            _rigid2D.AddForce(transform.up * jumpForce);
            _animator.SetBool(IsJumping, true);
            _isJumping = true;
        }

        var key = 0;
        if (Input.GetKey(rightKey)) key = 1;
        if (Input.GetKey(leftKey)) key = -1;

        var speedx = Mathf.Abs(_rigid2D.velocity.x);
        if (speedx < maxWalkSpeed)
        {
            _rigid2D.AddForce(transform.right * key * walkForce);
        }
        if (key != 0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }
        _animator.speed = speedx * 0.5f;

        if (_animator.GetBool(IsJumping))
        {
            _animator.speed = 1;
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
            _animator.SetBool(IsJumping, false);
            _isJumping = false;
        }
        else if (col.CompareTag("Player"))
        {
            _animator.SetBool(IsJumping, false);
            _isJumping = false;
        }
    }
}
