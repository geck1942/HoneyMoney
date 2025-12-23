using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MerchantPanel : MonoBehaviour
{
    public TMPro.TextMeshProUGUI honeyText;
    public TMPro.TextMeshProUGUI sellButtonText;
    public Button sellButton;
    void Start()
    {
        PlayerController.Instance.OnLoot += (resource, quantity, beehive) => Refreh();
        
        this.Refreh();
    }

    public void Refreh()
    {
        this.honeyText.text = "Honey: <b>" + PlayerController.Instance.Inventory.Honey.ToString("0") + "</b>\n" +
                              "<i>price:</i> " + FarmController.Instance.HoneyPrice.ToString("0") + "$";
        float sell = PlayerController.Instance.Inventory.Honey * FarmController.Instance.HoneyPrice;
        this.sellButtonText.text = "SELL for <b>" + sell.ToString("0") + "$</b>";
        sellButton.interactable = PlayerController.Instance.Inventory.Honey >= 1f;
    }

    public void SellHoney()
    {
        float soldQuantity = PlayerController.Instance.Inventory.Honey;
        float price = FarmController.Instance.HoneyPrice * soldQuantity;
        PlayerController.Instance.Loot("honey", -soldQuantity, FarmController.Instance.merchant);
        PlayerController.Instance.Loot("money", price, FarmController.Instance.merchant);
    }
}
