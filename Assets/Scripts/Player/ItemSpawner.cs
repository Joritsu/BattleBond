// PurchasedItemSpawner.cs
using UnityEngine;

public class PurchasedItemSpawner : MonoBehaviour
{
    public Transform[] spawnPoints; // one per ShopManager.items
    void Start()
    {
        for (int i = 0; i < ShopManager.Instance.items.Length; i++)
        {
            if (ShopManager.Instance.IsPurchased(i))
            {
                Instantiate(ShopManager.Instance.items[i].prefab,
                            spawnPoints[i].position,
                            Quaternion.identity);
            }
        }
    }
}
