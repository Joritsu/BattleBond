using UnityEngine;
using UnityEngine.UI;

public class DamageAnimationScript : MonoBehaviour
{
    public float moveSpeed = 1f;    // How fast the popup moves upward
    public float fadeSpeed = 1f;    // How fast it fades out
    private Text damageText;        // Reference to the Text component
    private Color originalColor;    // Store the original color for fading

    void Awake()
    {
        damageText = GetComponent<Text>();
        originalColor = damageText.color;
    }

    void Update()
    {
        // Move upward over time
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);

        // Fade out the text's alpha value
        Color newColor = damageText.color;
        newColor.a -= fadeSpeed * Time.deltaTime;
        damageText.color = newColor;

        // Destroy the popup when it’s fully transparent
        if (newColor.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Call this method to set the damage number
    public void SetDamage(int damage)
    {
        string text = ("- "+ damage +" k");

        damageText.text = text;
    }
}
