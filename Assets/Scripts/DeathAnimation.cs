using System.Collections;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    private RectTransform circlePanel; 
    [SerializeField] private float animationTime = 1.0f;
    public bool isAnimating = false;
    private float timeElapsed;
    private Vector3 originalScale;
    private Vector3 deathScale = new Vector3(2, 2, 2); // Scale for death animation
    private Vector3 targetScale;

    private void Start()
    {
        circlePanel = GetComponent<RectTransform>();
        originalScale = new Vector3(40, 40, 40); // Set the original scale
    }

    void Update()
    {
        if (isAnimating)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed < animationTime)
            {
                float progress = timeElapsed / animationTime;
                progress = 1 - Mathf.Pow(1 - progress, 2); // Smoother step interpolation

                circlePanel.localScale = Vector3.Lerp(originalScale, targetScale, progress);
            }
            else
            {
                circlePanel.localScale = targetScale;
                isAnimating = false;
                timeElapsed = 0;
            }

            if (Vector3.Distance(circlePanel.localScale, targetScale) < 0.1f)
            {
                circlePanel.localScale = targetScale;
                isAnimating = false;
                timeElapsed = 0;
            }
        }
    }

    public void TriggerDeathAnimation()
    {
        originalScale = circlePanel.localScale; // Store current scale
        targetScale = deathScale; // Scale down for death animation
        timeElapsed = 0;
        isAnimating = true;
    }

    public void TriggerRespawnAnimation()
    {
        originalScale = circlePanel.localScale; // Store current scale (death size)
        targetScale = new Vector3(40, 40, 40); // Scale back up to original size
        timeElapsed = 0;
        isAnimating = true;
    }
}