using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MerchantPanel : MonoBehaviour
{
    public RectTransform SellTitle;
    public RectTransform SellContent;
    public TMPro.TextMeshProUGUI honeyText;
    public TMPro.TextMeshProUGUI sellButtonText;
    public Button[] buyButttons;
    public Button sellButton;
    void Start()
    {
        PlayerController.Instance.OnLoot += (resource, quantity, beehive) => Refreh();
        
        this.Refreh();
    }

    public void Refreh()
    {
        if (PlayerController.Instance.Inventory.Honey >= 1f)
        {
            this.SellTitle.gameObject.SetActive(true);
            this.SellContent.gameObject.SetActive(true);
            this.honeyText.text = "Honey: <b>" + PlayerController.Instance.Inventory.Honey.ToString("0") + " x " + 
                                  FarmController.Instance.HoneyPrice.ToString("0") + "</b>";
            float sell = PlayerController.Instance.Inventory.Honey * FarmController.Instance.HoneyPrice;
            this.sellButtonText.text = "SELL for <b>" + sell.ToString("0") + "</b>";
        }
        else
        {
            this.SellTitle.gameObject.SetActive(false);
            this.SellContent.gameObject.SetActive(false);
        }

        foreach (Button button in this.buyButttons)
        {
            string strprice = button.gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
            float price = float.Parse(strprice);
            button.interactable = PlayerController.Instance.Inventory.Money >= price;
        }
    }

    public void SellHoney()
    {
        float soldQuantity = PlayerController.Instance.Inventory.Honey;
        float price = FarmController.Instance.HoneyPrice * soldQuantity;
        PlayerController.Instance.Loot("honey", -soldQuantity, FarmController.Instance.merchant);
        PlayerController.Instance.Loot("money", price, FarmController.Instance.merchant);
    }

    public void BuyItem(RectTransform item)
    {
        string itemname = item.gameObject.name;
        string strprice = item.gameObject.GetComponentInChildren<Button>()
            .GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
        float price = float.Parse(strprice);

        PlayerController.Instance.Loot("money", -price, FarmController.Instance.merchant);
        PlayerController.Instance.Loot(itemname, 1, FarmController.Instance.merchant);
    
    }
}
