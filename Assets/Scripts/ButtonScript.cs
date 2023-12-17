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

    [SerializeField] private Sprite leftIdle;
    [SerializeField] private Sprite leftPressed;
    [SerializeField] private Sprite rightIdle;
    [SerializeField] private Sprite rightPressed;
    

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

    }

    private void FixedUpdate()
    {
        float moveInput = _input.Movement.Walk.ReadValue<float>();
        ButtonChanger(moveInput);
    }

    private void ButtonChanger(float moveInput)
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
    }
}
