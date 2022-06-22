using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;
    float jumpForce = 680;
    public float walkForce = 30;

    float maxWalkSpeed = 2;
    private static readonly int IsJumping = Animator.StringToHash("isJumping");

    public GameObject worldOutBottomLine;
    public Transform spawnPoint;
    private bool _isJumping;
    public KeyCode jumpKey = KeyCode.Space, leftKey = KeyCode.A, rightKey = KeyCode.D;

    private void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (Input.GetKeyDown(jumpKey) && !_isJumping)
        {
            rigid2D.AddForce(transform.up * jumpForce);
            animator.SetBool(IsJumping, true);
            _isJumping = true;
        }

        var key = 0;
        if (Input.GetKey(rightKey)) key = 1;
        if (Input.GetKey(leftKey)) key = -1;

        var speedx = Mathf.Abs(rigid2D.velocity.x);
        if (speedx < maxWalkSpeed)
        {
            rigid2D.AddForce(transform.right * key * walkForce);
        }
        if (key != 0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }
        animator.speed = speedx * 0.5f;

        if (animator.GetBool(IsJumping))
        {
            animator.speed = 1;
        }

        if (worldOutBottomLine.transform.position.y > transform.position.y)
        {
            rigid2D.velocity = new Vector2(0, 0);
            transform.position = spawnPoint.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Cloud"))
        {
            animator.SetBool(IsJumping, false);
            _isJumping = false;
        }
    }
}
