using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AdaptivePerformance.Provider;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class RoachAI : EnemyClass
{
    private bool isChasing;
    private float wanderTimer;
    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _box2D;
    private Animator _animator;
    private bool isShrinking = false;
    
    
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float followRange = 3f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 shrinkRate;
    [SerializeField] private Vector3 shrinkThreshold;
    [SerializeField] private float wanderInterval;
    [SerializeField] private float moveSpeed;
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _box2D = GetComponent<BoxCollider2D>();
        wanderTimer = wanderInterval;
    }

    private void Update()
    {
        wanderTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        _animator.SetFloat("Speed",Math.Abs(_rigidbody2D.velocity.x));
        if(!isShrinking && !isChasing){Idle();}
        flipDirection();
        Pathfinding();
        checkEdges();
    }

    private void Idle()
    {
        float movingTimer = Random.Range(0, 2);
        if (wanderTimer <= 0)
        {
            while (movingTimer > 0)
            {
                movingTimer -= Time.deltaTime;
                _rigidbody2D.velocity = new Vector2(moveSpeed * Random.Range(-1,2),_rigidbody2D.velocity.y);
            }

            wanderTimer = wanderInterval;
        }
    }

    private void flipDirection()
    {
        if (_rigidbody2D.velocity.x < -0.1)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (_rigidbody2D.velocity.x > 0.1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public override void Hurt()
    {
        Debug.Log("Hurt Called from Roach");
        _animator.SetBool("Hurt", true);
        _box2D.enabled = false;
        _rigidbody2D.bodyType = RigidbodyType2D.Static;
        if (!isShrinking)
        {
            StartCoroutine(ShrinkObject());
        }
    }

    private IEnumerator ShrinkObject()
    {
        isShrinking = true;
        yield return new WaitForSeconds(1);

        while (transform.localScale.x > shrinkThreshold.x && transform.localScale.y > shrinkThreshold.y)
        {
            transform.localScale -= shrinkRate * Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
        isShrinking = false;
    }

    private void checkEdges()
    {
        Vector3 offset = new Vector3(0.5f, 0,0);
        RaycastHit2D left = Physics2D.Raycast(transform.position - offset, Vector2.down,followRange ,platformLayer);
        RaycastHit2D right = Physics2D.Raycast(transform.position + offset, Vector2.down, followRange,platformLayer);
        if (left.collider == null && _rigidbody2D.velocity.x < 0)
        {
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
        }
        if (right.collider == null && _rigidbody2D.velocity.x > 0)
        {
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
        }
    }

    private void Pathfinding()
    {
        Vector3 offset = new Vector3(0, 0.5f,0);
        RaycastHit2D left = Physics2D.Raycast(transform.position - offset, Vector2.left,followRange ,playerLayer);
        RaycastHit2D right = Physics2D.Raycast(transform.position - offset, Vector2.right, followRange,playerLayer);
        float direction = (playerTransform.position.x - transform.position.x) /
                          Mathf.Abs((playerTransform.position.x - transform.position.x));
        if (left.collider != null || right.collider != null)
        {
            isChasing = true;
            _rigidbody2D.velocity = new Vector2(moveSpeed * direction , _rigidbody2D.velocity.y);
        }
        else
        {
            isChasing = false;
        }
    }
    
    

    

   
}
