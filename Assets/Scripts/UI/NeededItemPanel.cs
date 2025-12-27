using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeededItemPanel : MonoBehaviour
{
    public TMPro.TextMeshProUGUI quantityText;
    public TMPro.TextMeshProUGUI title;
    public Image itemIcon;

    public void Init(Item item, int quantity)
    {
        this.itemIcon.sprite = item.icon;
        this.title.text = item.itemTitle;
        
        this.quantityText.text = PlayerController.Instance.Inventory.Get(item).ToString("0") + "/" + quantity.ToString();
    }
}
