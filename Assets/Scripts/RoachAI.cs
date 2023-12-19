using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AdaptivePerformance.Provider;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class RoachAI : EnemyClass
{
    private float wanderTimer;
    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _box2D;
    private Animator _animator;
    private bool isShrinking = false;
    

    [SerializeField] Vector3 shrinkRate;
    [SerializeField] Vector3 shrinkThreshold;
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
        if(!isShrinking){Idle();}
        flipDirection();
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
}
