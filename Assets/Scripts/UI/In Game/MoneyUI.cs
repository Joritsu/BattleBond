using UnityEngine;
using TMPro;  // make sure you have TextMeshPro imported

public class MoneyUI : MonoBehaviour
{
    [Tooltip("Drag your TextMeshProUGUI component here.")]
    public TMP_Text moneyText;  // or use TextMeshProUGUI

    void Start()
    {
        // Subscribe to the money changed event
        PlayerMoney.Instance.OnMoneyChanged += UpdateDisplay;
        // Initialize display
        UpdateDisplay(PlayerMoney.Instance.CurrentMoney);
    }

    void OnDestroy()
    {
        // Unsubscribe to avoid errors if this object is destroyed
        if (PlayerMoney.Instance != null)
            PlayerMoney.Instance.OnMoneyChanged -= UpdateDisplay;
    }

    void UpdateDisplay(int newAmount)
    {
        // Update the text to show the new amount
        moneyText.text = $"Money: ${newAmount}";
    }
}
