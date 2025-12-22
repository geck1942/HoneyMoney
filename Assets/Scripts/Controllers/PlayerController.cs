using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController<PlayerController>
{
    public Player player;
    public Transform proximityRadar;
    public float proximityDistance = 1f;
    
    public delegate void ProximityEvent<TInteractable>(TInteractable beehive);
    public delegate void LootEvent(Beehive beehive, int quantity);
    public event ProximityEvent<Beehive> OnBeehiveReached;
    public event ProximityEvent<Beehive> OnBeehiveLeft;
    public event LootEvent OnLoot;
    
    private Beehive _proximityBeehive = null;
    private Coroutine _lootingCoroutine = null; 
    
    void Start()
    {
        base.Start();
        
        this.OnBeehiveReached +=  this.StartLooting;
        this.OnBeehiveLeft += this.StopLooting;
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
        Beehive closestBeehive = null;
        Beehive lastClosestBeehive = this._proximityBeehive;
        
        foreach (var hive in FarmController.Instance.hives)
        {
            float sqrDistance = (hive.transform.position - this.proximityRadar.position).sqrMagnitude;
            if (sqrDistance < closestSqrDistance)
            {
                closestSqrDistance = sqrDistance;
                closestBeehive = hive;
            }
        }

        if (closestSqrDistance < this.proximityDistance * this.proximityDistance)
        {
            if (lastClosestBeehive == null)
            {
                this._proximityBeehive = closestBeehive;
                this.OnBeehiveReached?.Invoke(closestBeehive);
            }
            if (closestBeehive != lastClosestBeehive)
            {
                this._proximityBeehive = closestBeehive;
                this.OnBeehiveLeft?.Invoke(lastClosestBeehive);
                this.OnBeehiveReached?.Invoke(closestBeehive);
                
            }
        }
        else if(this._proximityBeehive != null)
        {
            this._proximityBeehive = null;
            this.OnBeehiveLeft?.Invoke(lastClosestBeehive);
        }
    }

    public void StartLooting(Beehive beehive)
    {
        this._lootingCoroutine = StartCoroutine(this.Loot(beehive));
    }

    public void StopLooting(Beehive beehive)
    {
        StopCoroutine(this._lootingCoroutine);
    }

    IEnumerator Loot(Beehive beehive)
    {
        int looted = 0;
        while (beehive != null)
        {
            looted = beehive.LootHoney(1);
            if(looted > 0)
                this.OnLoot.Invoke(beehive, looted);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
