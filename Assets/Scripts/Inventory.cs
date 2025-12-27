using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public float Money => _inventory["money"];
    public float Honey => _inventory["honey"];
    
    private Dictionary<string, float> _inventory =  new Dictionary<string, float>();

    public Inventory()
    {
        _inventory.Add("money", 0);
        _inventory.Add("honey", 0);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns>True if quantity was increased to next integer value</returns>
    public bool Add(string key, float value)
    {
        if (!_inventory.ContainsKey(key))
        {
            _inventory.Add(key, value);
            return value >= 1f;
        }
        bool increased = this._inventory[key] - Mathf.Floor(this._inventory[key]) + value >= 1f;
        _inventory[key] += value;
        return increased;
    }

    public float Get(Item item) => Get(item.itemName);
    public float Get(string key)
    {
        return _inventory.ContainsKey(key) ? _inventory[key] : 0;
    }

    public bool Has(Item item, float quantity) => Has(item.itemName, quantity);
    public bool Has(string key, float quantity)
    {
        if (_inventory.ContainsKey(key))
            return _inventory[key] >= quantity;
        return false;
    }
}