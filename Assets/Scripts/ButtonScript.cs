using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    private InputManager _input;
    private Transform leftButton;
    private Image leftButtonImage;
    private Transform rightButton;
    private Image rightButtonImage;
    private Transform upButton;
    private Image upButtonImage;

    [SerializeField] private Sprite leftIdle;
    [SerializeField] private Sprite leftPressed;
    [SerializeField] private Sprite rightIdle;
    [SerializeField] private Sprite rightPressed;
    [SerializeField] private Sprite upIdle;
    [SerializeField] private Sprite upPressed;
    

    private void Awake()
    {
        _input = new InputManager();
        _input.Enable();
    }

    private void Start()
    {
        leftButton = transform.Find("Left Button");
        leftButtonImage = leftButton.GetComponent<Image>();
        rightButton = transform.Find("Right Button");
        rightButtonImage = rightButton.GetComponent<Image>();
        upButton = transform.Find("Up Button");
        upButtonImage = upButton.GetComponent<Image>();

    }

    private void FixedUpdate()
    {
        float moveInput = _input.Movement.Walk.ReadValue<float>();
        float jumpInput = _input.Movement.Jump.ReadValue<float>();
        ButtonChanger(moveInput, jumpInput);
    }

    private void ButtonChanger(float moveInput, float jumpInput)
    {
        if (moveInput == 0)
        {
            rightButtonImage.sprite = rightIdle;
            leftButtonImage.sprite = leftIdle;
        }else if (moveInput < 0)
        {
            leftButtonImage.sprite = leftPressed;
            rightButtonImage.sprite = rightIdle;
        }else if (moveInput > 0)
        {
            rightButtonImage.sprite = rightPressed;
            leftButtonImage.sprite = leftIdle;
        }

        if (jumpInput != 0)
        {
            upButtonImage.sprite = upPressed;
        }else
        {
            upButtonImage.sprite = upIdle;
        }
        
    }
}
