using UnityEngine;

public class BulletA : Bullet
{
    public Vector3 moveDirection = Vector3.right;
    
    protected override void Move()
    {
        base.Move();
        
        transform.position += moveDirection * (Time.deltaTime * moveSpeed);
    }
}