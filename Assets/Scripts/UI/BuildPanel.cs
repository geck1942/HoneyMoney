using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildPanel : MonoBehaviour
{

    public BuildElement target;
    public Button buildButton;
    public RectTransform itemsContainer;
    private List<NeededItemPanel> neededItemsPanels = new List<NeededItemPanel>();

    public void Init(BuildElement target)
    {
        this.target = target;
        int i = 0;
        bool canbuild = true;
        for (; i < target.neededItems.Count; i++)
        {
            NeededItemPanel panel = null;
            Item item =  target.neededItems[i];
            if(neededItemsPanels.Count > i)
                panel = neededItemsPanels[i];
            else
            {
                panel = GameObject.Instantiate(UIController.Instance.neededItemPrefab, this.itemsContainer);
                neededItemsPanels.Add(panel);
            }

            int quantity = 1;
            if(target.neededItemsQuantities.Count > i)
                quantity = target.neededItemsQuantities[i];
            panel.gameObject.SetActive(true);
            panel.Init(item, quantity);
            
            if(!PlayerController.Instance.Inventory.Has(item.name, quantity))
                canbuild = false;
        }

        // hide remaining panels
        for (int j = i; j < this.neededItemsPanels.Count; j++)
        {
            this.neededItemsPanels[j].gameObject.SetActive(false);
        }
        this.buildButton.interactable = canbuild;
    }

    public void ClickOnBuild()
    {
        FarmController.Instance.Build(this.target);
    }
}
