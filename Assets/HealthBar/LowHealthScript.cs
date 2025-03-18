using UnityEngine;
using UnityEngine.UI;
public class LowHealthScript : MonoBehaviour
{
    [Header("References")]
    public Health playerHealth;    // The Health script on your Player
    public Image overlayImage;     // The UI Image (LowHealthOverlay)

    [Header("Effect Settings")]
    [Range(0f, 1f)]
    public float startEffectThreshold = 0.3f; // Start effect at 30% health
    public float maxAlpha = 0.8f;            // Overlay alpha at 0% health

    // Optional: Set a color with some alpha, the script will override alpha at runtime
    public Color effectColor = new Color(0.5f, 0, 0, 1);

    private void Update()
    {
        // Calculate health percentage (1 = full health, 0 = no health)
        float healthPercent = (float)playerHealth.currentHealth / playerHealth.maxHealth;

        if (healthPercent < startEffectThreshold)
        {
            // 1) How far below the threshold are we? (0 = at threshold, 1 = at zero health)
            float normalized = 1 - (healthPercent / startEffectThreshold);

            // 2) Lerp from 0 alpha at threshold to maxAlpha at 0% health
            float targetAlpha = Mathf.Lerp(0f, maxAlpha, normalized);

            // 3) Update the overlay color (keep the RGB from effectColor)
            Color currentColor = effectColor;
            currentColor.a = targetAlpha;
            overlayImage.color = currentColor;
        }
        else
        {
            // If health is above threshold, hide the overlay
            Color currentColor = overlayImage.color;
            currentColor.a = 0f;
            overlayImage.color = currentColor;
        }
    }
}
