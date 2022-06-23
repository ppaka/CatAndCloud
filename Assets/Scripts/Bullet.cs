using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 3f;
    
    private void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        
    }
}