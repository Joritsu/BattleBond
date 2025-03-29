using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Required for TextMeshPro

public class DamagePopup : MonoBehaviour
{
    public TMP_Text damageText; // TextMeshPro instead of UI.Text
    public float moveSpeed = 1f;    // How fast the popup moves upward
    public float fadeSpeed = 1f;    // How fast it fades out
    private RectTransform rectTransform;

    void Awake()
    {
        // Automatically get the Text component if not manually assigned.
        if (damageText == null)
            damageText = GetComponentInChildren<TMP_Text>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Move the popup upward over time.
        rectTransform.anchoredPosition += new Vector2(0, moveSpeed * Time.deltaTime);

        // Fade out the text's alpha value.
        Color newColor = damageText.color;
        newColor.a -= fadeSpeed * Time.deltaTime;
        damageText.color = newColor;

        // Destroy the popup when it's fully transparent.
        if (newColor.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Call this method to set the damage number.
    public void SetDamage(float damage)
    {
        Debug.Log("Setting damage: " + damage);
        string txt = ("-" + damage.ToString() + " k");
        damageText.text = txt;
    }

    // (Optional) Use this method to set custom text instead.
    public void SetDamageText(string text)
    {
        damageText.text = text;
    }
}
