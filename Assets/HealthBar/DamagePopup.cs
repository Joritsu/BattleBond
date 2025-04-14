using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [Header("References")]
    public TMP_Text damageText;   // Drag a TextMeshPro UI text reference here if not auto-found

    [Header("Follow & Canvas")]
    public Transform target;      // Which Transform in the world to follow
    public Canvas uiCanvas;       // The Screen Space � Overlay canvas

    [Header("Movement & Fade")]
    public float moveSpeed = 1f;  // Moves upward in UI coordinates
    public float fadeSpeed = 1f;  // How fast the text alpha fades
    public float lifetime = 2f;   // Destroy after X seconds if you like

    // Internal
    private RectTransform rectTransform;
    private float timer;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        // If not assigned in Inspector, try to find the text automatically
        if (damageText == null)
        {
            damageText = GetComponentInChildren<TMP_Text>();
        }
    }

    void Update()
    {
        // 1) Continuously follow the target�s current position on-screen
        if (target != null && uiCanvas != null)
        {
            // Convert the target's world position to a screen position
            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);

            // Convert the screen position to the Canvas's local position
            RectTransform canvasRect = uiCanvas.GetComponent<RectTransform>();

            Vector2 uiPos;
            // For Screen Space � Overlay, 'camera' can be null or Camera.main, either is fine
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null, out uiPos);

            // Update the popup's anchored position
            rectTransform.anchoredPosition = uiPos;
        }

        // 2) Move upward a bit each frame in UI space
        rectTransform.anchoredPosition += new Vector2(0, moveSpeed * Time.deltaTime);

        // 3) Fade out the text over time
        Color newColor = damageText.color;
        newColor.a -= fadeSpeed * Time.deltaTime;
        damageText.color = newColor;

        // 4) Destroy after lifetime or once fully transparent
        timer += Time.deltaTime;
        if (timer >= lifetime || newColor.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Initialize the popup�s target, canvas, and damage text.
    /// Called immediately after instantiation by the Health script.
    /// </summary>
    public void InitializePopup(Transform target, Canvas canvas, int damage, bool healing)
    {
        this.target = target;
        this.uiCanvas = canvas;
        if (!healing)
        {
            SetDamage(damage, '-');
        }
        else
        {
            SetDamage(damage, '+');
        }
    }

    public void SetDamage(int damage, char heal_or_dmg)
    {
        if (damageText != null)
        {
            damageText.text = "-" + damage + " k";
        }
    }
}
