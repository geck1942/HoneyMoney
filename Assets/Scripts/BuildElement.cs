using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildElement : MonoBehaviour, IInteractable
{
    // public Collider Collider;
    public List<Item> neededItems;
    public List<int> neededItemsQuantities;
    
    public List<GameObject> deactivateOnBuild = new List<GameObject>();
    public List<GameObject> activateOnBuild = new List<GameObject>();

    public bool autoBuild  = false;
    public bool isDone = false;

    public Transform Transform => transform;
    public float InteractionDistance => 3f;
    public ScenarioState AvailableAt = ScenarioState.None;

    public bool PlayerCanBuild()
    {
        if(!IsAvailable())
            return false;
        if(this.neededItems.Count == 0)
            return true;

        bool yesTheyCan = true;
        for (int i = 0; i < this.neededItems.Count; i++)
        {
            int quantity = 1;
            if(this.neededItemsQuantities.Count > i)
                quantity = this.neededItemsQuantities[i];
            if (!PlayerController.Instance.Inventory.Has(this.neededItems[i], quantity))
                yesTheyCan = false;
        }
        return yesTheyCan;
    }

    public void Build()
    {
        foreach (GameObject go in deactivateOnBuild)
            go.SetActive(false);
        foreach (GameObject go in activateOnBuild)
            go.SetActive(true);
        
        this.isDone = true;
    }
    
    public bool IsAvailable()
    {
        return ScenarioController.Instance.state >= AvailableAt && !this.isDone;
    }
}
