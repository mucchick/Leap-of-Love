using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private InputManager _inputManager;
    private InputActionReference input;
    private Rigidbody2D _rb;
    private BoxCollider2D _box2d;
    private Transform _transform;
    private Animator _animator;
    private float jumpTimeCounter;
    private bool isJumping;
    private bool isAnimating;

    [SerializeField] private DeathAnimation deathAnimation;
    [SerializeField] private float forceMultiplier;
    [SerializeField] private float minJumpForce;
    [SerializeField] private float maxJumpForce;
    [SerializeField] private float maxJumpTime;
    [SerializeField] private float topSpeed;
    [SerializeField] private float friction;
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private LayerMask deathLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject respawnPoint;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private float castDistance;
    
    private void Awake()
    {
        _inputManager = new InputManager();
        _inputManager.Enable();
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        isAnimating = deathAnimation.isAnimating;
        float moveInput = _inputManager.Movement.Walk.ReadValue<float>();
        float jumpInput = _inputManager.Movement.Jump.ReadValue<float>();
        if (!isAnimating)
        {
            Walk(moveInput);
            Jump(jumpInput);
        }
        _animator.SetFloat("Speed", Math.Abs(_rb.velocity.x)); 
        _animator.SetBool("Grounded", isGrounded());
        if (isDead())
        {
            StartCoroutine(Respawn());}
        HurtEnemy();
    }
    

    private void Walk(float moveInput)
    {
        if (_rb.velocity.x < -0.1)
        {
            _transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (_rb.velocity.x > 0.1)
        {
            _transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (_rb.velocity.x < topSpeed && _rb.velocity.x > -topSpeed)
        {
            _rb.AddForce(new Vector2(moveInput * forceMultiplier, 0));
        }

        if (isGrounded())
        {
            if (_rb.velocity.x is > 0.3f or < -0.3f)
            {
                _rb.velocity = new Vector2(_rb.velocity.x * friction, _rb.velocity.y);
            }
            else if(moveInput == 0)
            {
                _rb.velocity = new Vector2(0, _rb.velocity.y);

            }
        }
    }
    private void Jump(float jumpInput)
    {
        if (jumpInput > 0 && isGrounded() && !isJumping)
        {
            isJumping = true;
            jumpTimeCounter = 0;
            _rb.AddForce(new Vector2(0, minJumpForce), ForceMode2D.Impulse);
        }

        if (jumpInput > 0 && isJumping)
        {
            if (jumpTimeCounter < maxJumpTime)
            {
                _rb.AddForce(new Vector2(0, (maxJumpForce - minJumpForce) * Time.fixedDeltaTime / maxJumpTime), ForceMode2D.Impulse);
                jumpTimeCounter += Time.fixedDeltaTime;
            }
        }
        else
        {
            isJumping = false;
        }
    }



    public bool isGrounded()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, platformLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator Respawn()
    {
        
        deathAnimation.TriggerDeathAnimation();
        
        yield return new WaitWhile(() => deathAnimation.isAnimating);

        _rb.bodyType = RigidbodyType2D.Static;
        deathAnimation.TriggerRespawnAnimation();
        transform.position = respawnPoint.transform.position;
        _rb.bodyType = RigidbodyType2D.Dynamic;



    }

    private bool isDead()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, deathLayer))
        {

            return true;

        }

        return false;
    }

    private void HurtEnemy()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, enemyLayer);
        if (hit.collider != null)
        {
            Debug.Log("Hit detected");
            EnemyClass enemy = hit.collider.GetComponent<EnemyClass>();
            if (enemy != null)
            {
                enemy.Hurt();
                _rb.AddForce(new Vector2(0, minJumpForce * 1.5f), ForceMode2D.Impulse);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }
}
