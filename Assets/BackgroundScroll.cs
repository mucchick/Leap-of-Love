using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    private backgroundManager _manager;
    private int index;
    private Vector2 initialState;
    private bool transitionStarted = false;
    
    
    
    
    
    private RectTransform rectTransform;

    private void Start()
    {
        index = SelectIndex();
        rectTransform = GetComponent<RectTransform>();
        _manager = GetComponentInParent<backgroundManager>();
        initialState = rectTransform.anchoredPosition;
    }
    
    void Update()
    {
        if (_manager.currentBackgroundIndex == index && !transitionStarted)
        {
            float newX = rectTransform.anchoredPosition.x - scrollSpeed * Time.deltaTime;
            rectTransform.anchoredPosition = new Vector2(newX, rectTransform.anchoredPosition.y);

            if (newX < 0) // Update this condition based on your transition point
            {
                _manager.TransitionToNextBackground();
                transitionStarted = true;
            }
        }
        else if (_manager.currentBackgroundIndex != index && transitionStarted)
        {
            StartCoroutine(ResetPositionAfterDelay(3.0f));
            transitionStarted = false;
        }
    }
    
    private IEnumerator ResetPositionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        rectTransform.anchoredPosition = initialState;
    }

    private int SelectIndex()
    {
        if (CompareTag("Spring"))
        {
            return 0;
        }

        if (CompareTag("Summer"))
        {
            return 1;
        }

        if (CompareTag("Fall"))
        {
            return 2;
        }

        if (CompareTag("Winter"))
        {
            return 3;
        }

        return -1;
    }
}