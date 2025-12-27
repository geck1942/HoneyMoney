using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIController : BaseController<UIController>
{
    public UIState state = UIState.None;
    public InventoryPanel inventoryPanel;
    public HivesPanel hivesPanel;
    public ShopPanel shopPanel;
    public TMPro.TextMeshProUGUI topBarHoney;
    public TMPro.TextMeshProUGUI topBarMoney;
    public TMPro.TextMeshProUGUI topBarBucks;
    
    public BeehivePanel beehivePanel;
    public MerchantPanel merchantPanel;
    public BuildPanel buildPanel;
    
    public Transform proximityCircle;
    public GameObject lootHoneyPrefab;
    public GameObject lootMoneyPrefab;
    public BuyableItemPanel itemShopPrefab;
    public NeededItemPanel neededItemPrefab;

    public RectTransform dialog;
    public TMPro.TextMeshProUGUI dialogText;
    public RectTransform joystickBlock;
    private Coroutine dialogCoroutine = null;
    
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

        if (ScenarioController.Instance != null)
        {
            ScenarioController.Instance.OnScenarioNextStep += InstanceOnOnScenarioNextStep;
        }
        
        this.RefreshTopBar();
        this.TogglePlayMode(true);
        this.OnInteractableLeft(null);
        this.HideDialog();
    }

    /// <summary>
    /// Place the joystick under the players finger
    /// </summary>
    /// <param name="eventData"></param>
    public void PlaceJoystick(BaseEventData eventData)
    {
        if(eventData is PointerEventData pointerEventData)
            this.joystickBlock.position = pointerEventData.position;
    }

    /// <summary>
    /// Change UI When new Scenario step is reached.
    /// </summary>
    private void InstanceOnOnScenarioNextStep(ScenarioState scenarioState, ScenarioStep step)
    {
        this.HideDialog();
        this.joystickBlock.gameObject.SetActive(!step.freezePlayer);
        
        if(dialogCoroutine != null)
            StopCoroutine(dialogCoroutine);
        dialogCoroutine = StartCoroutine(ShowDialog(step.message, step.messageDuration));
        
    }

    /// <summary>
    /// Show dialog text for a given duration
    /// </summary>
    private IEnumerator ShowDialog(string message, float duration)
    {
        if (!string.IsNullOrEmpty(message))
        {
            this.dialogText.text = message;
            this.dialog.gameObject.SetActive(true);
            if (duration > 0)
            {
                yield return new WaitForSeconds(duration);
                this.HideDialog();
            }
        }
    }

    public void HideDialog()
    {
        this.dialog.gameObject.SetActive(false);
    }

    /// <summary>
    /// Reset UI when an Interactable object is left by the player
    /// </summary>
    private void OnInteractableLeft(IInteractable target)
    {
        this.beehivePanel.target = null;
        this.beehivePanel.gameObject.SetActive(false);
        this.merchantPanel.gameObject.SetActive(false);
        this.buildPanel.gameObject.SetActive(false);
        this.proximityCircle.gameObject.SetActive(false);
        this.state &= ~UIState.ProximityPanel;
        this.OnUIStateChanged?.Invoke(this.state);
    }

    /// <summary>
    /// Change UI when an Interactable object is reached by the player.
    /// </summary>
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
            this.merchantPanel.Init(merchant);
        }
        else if (target is BuildElement actionElement)
        {
            this.buildPanel.gameObject.SetActive(true);
            this.buildPanel.Init(actionElement);
        }

        // Show Hexhalo around item
        this.proximityCircle.position = target.Transform.position + (Vector3.up * 0.01f);
        this.proximityCircle.gameObject.SetActive(true);

        // Change UI state to adjust Cameras
        this.state |= UIState.ProximityPanel;
        this.OnUIStateChanged?.Invoke(this.state);
    }

    public void RefreshTopBar()
    {
        this.topBarHoney.text = PlayerController.Instance.Inventory.Honey.ToString("0");
        this.topBarMoney.text = PlayerController.Instance.Inventory.Money.ToString("0");
        this.topBarBucks.text = PlayerController.Instance.Inventory.Get("buck").ToString("0");
    }

    /// <summary>
    /// Generate loot icons to show player feedback
    /// </summary>
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
