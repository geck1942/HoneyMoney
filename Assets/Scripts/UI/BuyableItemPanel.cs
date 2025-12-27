using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyableItemPanel : MonoBehaviour
{
    public TMPro.TextMeshProUGUI title;
    public Image image;
    public Button buyButton;
    public TMPro.TextMeshProUGUI buttonPriceText;
    public Image buttonCoinIcon;
    public Image buttonBuckIcon;

    private Item activeItem;
    
    public void Init(Item item)
    {
        this.activeItem = item;
        this.image.sprite = item.icon;
        this.title.text = item.itemTitle;
        this.buttonPriceText.text = item.price.ToString("0");
        this.buttonCoinIcon.gameObject.SetActive(!item.priceInBucks);
        this.buttonBuckIcon.gameObject.SetActive(item.priceInBucks);

        this.buyButton.interactable = PlayerController.Instance.PlayerCanBuy(item);
    }

    public void Buy()
    {
        PlayerController.Instance.Loot("money", -activeItem.price, FarmController.Instance.merchant);
        PlayerController.Instance.Loot(activeItem.name, 1, FarmController.Instance.merchant);

    }
}
