using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseController<PlayerController>
{
    public Player player;
    public Transform proximityRadar;

    public readonly Inventory Inventory = new Inventory();
    
    public List<GameObject> honeyJars = new List<GameObject>();
    
    public delegate void ProximityEvent(IInteractable target);
    public delegate void ProximityEvent<TInteractable>(TInteractable target)
        where TInteractable : IInteractable;
    public delegate void LootEventHandler(string resource, float quantity, IInteractable target);
    public event ProximityEvent OnInteractableReached;
    public event ProximityEvent OnInteractableLeft;
    
    public event ProximityEvent<Beehive> OnBeehiveReached;
    public event ProximityEvent<Beehive> OnBeehiveLeft;
    public event LootEventHandler OnLoot;
    
    
    private IInteractable _activeInteractable = null;
    private Coroutine _lootingCoroutine = null; 
    
    void Start()
    {
        this.OnBeehiveReached += this.StartLooting;
        this.OnBeehiveLeft += this.StopLooting;
        this.OnLoot += AdjustBackpack;
    }

    /// <summary>
    /// Shows the honey pots on the back of the player upon loot.
    /// </summary>
    private void AdjustBackpack(string resource, float quantity, IInteractable target)
    {
        // A finite list of gameobjects is used. This is not amazing (cause limited)
        // But great for performances.
        for (int i = 0; i < honeyJars.Count; i++)
        {
            honeyJars[i].SetActive(this.Inventory.Honey >= i + 1);
        }
            
    }

    /// <summary>
    /// Can the player buy this stuff?
    /// </summary>
    /// <param name="item">The stuff to buy</param>
    /// <param name="quantity">Optional quantity</param>
    public bool PlayerCanBuy(Item item, int quantity = 1)
    {
        if (item.priceInBucks)
            return this.Inventory.Has("buck", item.price * quantity);
        else
            return this.Inventory.Has("money", item.price * quantity);
    }

    void Update()
    {
        // Check proximity 
        // This is BAD coding => Use some colliders and raycasts, but no time for that.
        this.FindProximityInteraction();
    }

    /// <summary>
    /// BAD proximity calculation, with hard-listed items from the scene :(
    /// </summary>
    public void FindProximityInteraction()
    {
        // SQR for great perfs :)
        float closestSqrDistance =  float.MaxValue;
        IInteractable closestInteractable = null;
        IInteractable lastClosestInteractable = this._activeInteractable;
        
        foreach (var target in FarmController.Instance.GetAllInteractables())
        {
            float sqrDistance = (target.Transform.position - this.proximityRadar.position).sqrMagnitude;
            if (sqrDistance < closestSqrDistance && sqrDistance <= target.InteractionDistance * target.InteractionDistance)
            {
                closestSqrDistance = sqrDistance;
                closestInteractable = target;
            }
        }

        if (closestInteractable != null)
        {
            if (lastClosestInteractable == null)
            {
                this._activeInteractable = closestInteractable;
                this.OnInteractableReached?.Invoke(closestInteractable);
                if(closestInteractable is Beehive beehive)
                    this.OnBeehiveReached?.Invoke(beehive);
            }
            else if (closestInteractable != lastClosestInteractable)
            {
                this._activeInteractable = closestInteractable;
                this.OnInteractableLeft?.Invoke(lastClosestInteractable);
                if(lastClosestInteractable is Beehive beehiveLeft)
                    this.OnBeehiveLeft?.Invoke(beehiveLeft);
                this.OnInteractableReached?.Invoke(closestInteractable);
                if(closestInteractable is Beehive beehiveReached)
                    this.OnBeehiveReached?.Invoke(beehiveReached);
                    
                
            }
        }
        else if(this._activeInteractable != null)
        {
            this._activeInteractable = null;
            this.OnInteractableLeft?.Invoke(lastClosestInteractable);
            if(lastClosestInteractable is Beehive beehiveLeft)
                this.OnBeehiveLeft?.Invoke(beehiveLeft);
        }
    }

    public void StartLooting(Beehive beehive)
    {
        this._lootingCoroutine = StartCoroutine(this.LootBeehiveAsync(beehive));
    }

    public void StopLooting(Beehive beehive)
    {
        StopCoroutine(this._lootingCoroutine);
    }

    /// <summary>
    /// Main Loot method. Negative values are supported.
    /// </summary>
    /// <param name="resource">Resource to add to inventory</param>
    /// <param name="quantity">Optional quantity</param>
    /// <param name="source">Source of the loot to show animation from</param>
    /// <returns>True if quantity was increased</returns>
    public bool Loot(string resource, float quantity = 1, IInteractable source = null)
    {
        bool increased = this.Inventory.Add(resource, quantity);
        if (increased)
            this.OnLoot.Invoke(resource, quantity, source);
        return increased;
    }

    /// <summary>
    /// Makes the player loot a honey pot once every 250ms. 
    /// </summary>
    /// <param name="beehive"></param>
    IEnumerator LootBeehiveAsync(Beehive beehive)
    {
        int looted = 0;
        while (beehive != null)
        {
            looted = beehive.LootHoney(1);
            if(looted > 0)
                if (this.Loot("honey", looted, beehive))
                {
                    this.player.animator.Play("Loot");
                }
                    
            yield return new WaitForSeconds(0.25f);
        }
    }
}
