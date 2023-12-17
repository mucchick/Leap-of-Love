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
    public InputActionReference input;
    private Rigidbody2D _rb;
    private BoxCollider2D _box2d;
    private Transform _transform;
    private Animator _animator;
    
    [SerializeField]  private float forceMultiplier;
    [SerializeField]  private float topSpeed;

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
        float moveInput = _inputManager.Movement.Walk.ReadValue<float>();
        Walk(moveInput);
        _animator.SetFloat("Speed", Math.Abs(_rb.velocity.x));
        
    }

    private void Walk(float moveInput)
    {
        if (_rb.velocity.x < -0.1)
        {
            _transform.rotation = Quaternion.Euler(0, 180, 0);        }

        if (_rb.velocity.x > 0.1)
        {
            _transform.rotation = Quaternion.Euler(0, 0, 0);        }
        if (_rb.velocity.x < topSpeed && _rb.velocity.x > -topSpeed)
        {
            _rb.AddForce(new Vector2(moveInput * forceMultiplier, 0));
        }
    }
}
