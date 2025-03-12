using UnityEngine;
using UnityEngine.UI;

public class HealthWarningEffect : MonoBehaviour
{
    public Image redBorderPanel;  // Assign the UI Panel
    public float lowHealthThreshold = 30f;  // When to start showing effect
    public float pulseSpeed = 1.5f;  // Speed of transparency pulsing
    private float minAlpha = 0.15f;   // Minimum transparency (30%)
    private float maxAlpha = 0.32f;   // Maximum transparency (40%)

    private PlayerScript playerScript;
    private float targetAlpha = 0f;

    void Start()
    {
        playerScript = GetComponent<PlayerScript>();

        // Ensure the panel starts invisible
        SetBorderAlpha(0f);
    }

    void Update()
    {
        // If player health is below threshold, apply effect
        if (playerScript.playerCurrentHealth <= lowHealthThreshold)
        {
            float alphaValue = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(Time.time * pulseSpeed, 1));
            SetBorderAlpha(alphaValue);
        }
        else
        {
            SetBorderAlpha(0f); // Hide when health is fine
        }
    }

    void SetBorderAlpha(float alpha)
    {
        Color panelColor = redBorderPanel.color;
        panelColor.a = alpha;
        redBorderPanel.color = panelColor;
    }
}