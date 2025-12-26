using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseController<PlayerController>
{
    public Player player;
    public Transform proximityRadar;

    public Inventory Inventory = new Inventory();
    
    public List<GameObject> honeyJars = new List<GameObject>();
    
    public delegate void ProximityEvent(IInteractable target);
    public delegate void ProximityEvent<TInteractable>(TInteractable target)
        where TInteractable : IInteractable;
    public delegate void LootEvent(string resource, float quantity, IInteractable target);
    public event ProximityEvent OnInteractableReached;
    public event ProximityEvent OnInteractableLeft;
    
    public event ProximityEvent<Beehive> OnBeehiveReached;
    public event ProximityEvent<Beehive> OnBeehiveLeft;
    public event LootEvent OnLoot;
    
    
    private IInteractable _activeInteractable = null;
    private Coroutine _lootingCoroutine = null; 
    
    void Start()
    {
        this.OnBeehiveReached += this.StartLooting;
        this.OnBeehiveLeft += this.StopLooting;

        this.OnLoot += AdjustBackpack;
    }

    private void AdjustBackpack(string resource, float quantity, IInteractable target)
    {
        for (int i = 0; i < honeyJars.Count; i++)
        {
            honeyJars[i].SetActive(this.Inventory.Honey >= i + 1);
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        // Check proximity 
        // This is bad coding => Use some colliders and raycasts
        this.FindProximityInteraction();
        
    }
    
    

    public void FindProximityInteraction()
    {
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
    /// 
    /// </summary>
    /// <param name="resource"></param>
    /// <param name="quantity"></param>
    /// <param name="target"></param>
    /// <returns>True if quantity was increased</returns>
    public bool Loot(string resource, float quantity, IInteractable target = null)
    {
        bool increased = this.Inventory.Add(resource, quantity);
        if (increased)
            this.OnLoot.Invoke(resource, quantity, target);
        return increased;
    }

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
