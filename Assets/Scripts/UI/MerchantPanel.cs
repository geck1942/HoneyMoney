using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class MerchantPanel : MonoBehaviour
{
    public RectTransform SellTitle;
    public RectTransform SellContent;
    public TMPro.TextMeshProUGUI honeyText;
    public TMPro.TextMeshProUGUI sellButtonText;
    public Button sellButton;

    public RectTransform itemsContainer;
    private List<BuyableItemPanel> buyableItemPanels = new List<BuyableItemPanel>();
    
    void Start()
    {
        PlayerController.Instance.OnLoot += (resource, quantity, beehive) => Init(null);
    }

    public void Init(Merchant merchant)
    {
        if (PlayerController.Instance.Inventory.Honey >= 1f)
        {
            this.SellTitle.gameObject.SetActive(true);
            this.SellContent.gameObject.SetActive(true);
            this.honeyText.text = "Miel: <b>" + PlayerController.Instance.Inventory.Honey.ToString("0") + " x " + 
                                  FarmController.Instance.HoneyPrice.ToString("0") + "</b>";
            float sell = PlayerController.Instance.Inventory.Honey * FarmController.Instance.HoneyPrice;
            this.sellButtonText.text = "Vendre pour <b>" + sell.ToString("0") + "</b>";
        }
        else
        {
            this.SellTitle.gameObject.SetActive(false);
            this.SellContent.gameObject.SetActive(false);
        }

        for (int i = 0; i < FarmController.Instance.allItems.Count; i++)
        {
            BuyableItemPanel panel = null;
            Item item =  FarmController.Instance.allItems[i];
            if(buyableItemPanels.Count > i)
                panel = buyableItemPanels[i];
            else
            {
                panel = GameObject.Instantiate(UIController.Instance.itemShopPrefab, this.itemsContainer);
                buyableItemPanels.Add(panel);
            }
            panel.Init(FarmController.Instance.allItems[i]);
            panel.gameObject.SetActive(item.IsPurchasable());
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

    }
}
