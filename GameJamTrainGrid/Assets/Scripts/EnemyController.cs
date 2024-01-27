using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    private Rigidbody2D rb;
    private Animator animator;
    private Transform currentPoint;
    public float speed;

    private void OnCollisionEnter2D(Collision2D collision)
    {


        Debug.Log(collision);
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

            playerController.DecreaseHealth();
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentPoint = pointB;
        animator.SetBool("isWalking", true);

    }

    private void Update()
    {
        Vector2 point = currentPoint.position-transform.position;
        if(currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
            

        }
        else
        {
            rb.velocity =new Vector2(-speed,0);
        }

        if(Vector2.Distance(transform.position, currentPoint.position) < 0.5f &&currentPoint==pointB)
        {
            flip();
            currentPoint = pointA;
            
        }
        else if(Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA)
        {
            flip();
            currentPoint = pointB;
            
        }
    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
