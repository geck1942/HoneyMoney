

using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Items", order = 1)]
public class Item : ScriptableObject
{
    public string itemName;
    public string itemTitle;
    public Sprite icon;
    public float price;
    public ScenarioState buyableAt;
    public bool canBeBought;
    public bool priceInBucks;

    public bool IsPurchasable()
    {
        return this.canBeBought && ScenarioController.Instance.state >= buyableAt;
    }
}