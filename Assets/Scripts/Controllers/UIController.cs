using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class UIController : BaseController<UIController>
{
    public UIState state = UIState.None;
    public InventoryPanel inventoryPanel;
    public HivesPanel hivesPanel;
    public ShopPanel shopPanel;
    public TMPro.TextMeshProUGUI topBarMoney;
    public TMPro.TextMeshProUGUI topBarBucks;
    
    public BeehivePanel beehivePanel;
    public MerchantPanel merchantPanel;
    
    public Transform proximityCircle;
    public GameObject lootHoneyPrefab;
    public GameObject lootMoneyPrefab;
    
    public delegate void UIStateEvent(UIState state);
    public event UIStateEvent OnUIStateChanged;
    
    void Start()
    {
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.OnInteractableReached += OnInteractableReached;
            PlayerController.Instance.OnInteractableLeft += OnInteractableLeft;
            PlayerController.Instance.OnLoot += ShowLoot;
            PlayerController.Instance.OnLoot += ((resource, quantity, beehive) => RefreshTopBar());
        }
        
        this.RefreshTopBar();
        this.TogglePlayMode(true);
        this.OnInteractableLeft(null);
    }
    
    private void OnInteractableLeft(IInteractable target)
    {
        this.beehivePanel.target = null;
        this.beehivePanel.gameObject.SetActive(false);
        this.merchantPanel.gameObject.SetActive(false);
        this.proximityCircle.gameObject.SetActive(false);
        this.state &= ~UIState.ProximityPanel;
        this.OnUIStateChanged?.Invoke(this.state);
    }

    private void OnInteractableReached(IInteractable target)
    {
        if (target is Beehive beehive)
        {
            this.beehivePanel.target = beehive;
            this.beehivePanel.gameObject.SetActive(true);
        }
        else if (target is Merchant merchant)
        {
            this.merchantPanel.gameObject.SetActive(true);
        }

        this.proximityCircle.position = target.Transform.position + (Vector3.up * 0.01f);
        this.proximityCircle.gameObject.SetActive(true);

        this.state |= UIState.ProximityPanel;
        this.OnUIStateChanged?.Invoke(this.state);
    }

    public void RefreshTopBar()
    {
        this.topBarMoney.text = PlayerController.Instance.Inventory.Money.ToString(("0") + "$");
    }

    public void ShowLoot(string resource, float quantity, IInteractable target)
    {
        if (target != null && quantity > 0)
        {
            GameObject lootIcon = null;
            switch (resource)
            {
                case "honey":
                    lootIcon = GameObject.Instantiate(lootHoneyPrefab, target.Transform.position, Quaternion.identity);
                    break;
                case "money":
                    lootIcon = GameObject.Instantiate(lootMoneyPrefab, target.Transform.position, Quaternion.identity);
                    break;
            }
            StartCoroutine(ShowLootCoroutine(lootIcon));
        }
    }

    IEnumerator ShowLootCoroutine(GameObject lootPrefab)
    {
        yield return new WaitForSeconds(1f);
        GameObject.Destroy(lootPrefab);
    }

    public void TogglePlayMode(bool isOn)
    {
        if (isOn)
        {
            this.hivesPanel.gameObject.SetActive(false);
            this.inventoryPanel.gameObject.SetActive(false);
            this.shopPanel.gameObject.SetActive(false);
            
            this.state &= ~UIState.MenuPanel;
            this.OnUIStateChanged?.Invoke(this.state);
        } 
    }
    public void ToggleInventory(bool isOn)
    {
        if (isOn)
        {
            this.hivesPanel.gameObject.SetActive(false);
            this.inventoryPanel.gameObject.SetActive(true);
            this.shopPanel.gameObject.SetActive(false);

            this.state |= UIState.MenuPanel;
            this.OnUIStateChanged?.Invoke(this.state);
        } 
    }    
    public void ToggleHives(bool isOn)
    {
        if (isOn)
        {
            this.hivesPanel.gameObject.SetActive(true);
            this.inventoryPanel.gameObject.SetActive(false);
            this.shopPanel.gameObject.SetActive(false);

            this.state |= UIState.MenuPanel;
            this.OnUIStateChanged?.Invoke(this.state);

        } 
    }
    public void ToggleShop(bool isOn)
    {
        if (isOn)
        {
            this.hivesPanel.gameObject.SetActive(false);
            this.inventoryPanel.gameObject.SetActive(false);
            this.shopPanel.gameObject.SetActive(true);

            this.state |= UIState.MenuPanel;
            this.OnUIStateChanged?.Invoke(this.state);
        } 
    }

}
