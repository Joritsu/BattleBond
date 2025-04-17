// ShopUI.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    public Button[] buyButtons;        // assign in Inspector: one per item
    public TMP_Text[] costTexts;       // optional: text fields showing cost
    public Image[]  itemIcons;      // assign these in the Inspector

    public Button proceedButton;

    void Start()
    {
        for (int i = 0; i < buyButtons.Length; i++)
        {
            int idx = i;
            costTexts[i].text = $"${ShopManager.Instance.items[i].cost}";
            buyButtons[i].onClick.AddListener(() => OnBuy(idx));
            UpdateBuyButton(idx);
        }
        proceedButton.onClick.AddListener(ShopManager.Instance.ProceedToNextLevel);
    }

    void OnBuy(int i)
    {
        if (ShopManager.Instance.BuyItem(i))
            UpdateBuyButton(i);
    }

    void UpdateBuyButton(int i)
    {
        bool owned = ShopManager.Instance.IsPurchased(i);
        buyButtons[i].interactable = !owned;
        buyButtons[i].GetComponentInChildren<TMP_Text>().text = owned ? "Owned" : "Buy";
    }
}
