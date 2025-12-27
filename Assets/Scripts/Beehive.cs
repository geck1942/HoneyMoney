using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beehive : MonoBehaviour, IInteractable
{

    public float pollenCapacity = 10f;
    public float processingSpeed = 1f;
    public float honeyCapacity = 5f;
    public float processingRatio = 1f;
    public float pollen = 0f;
    public float honey = 0f;
    public int level = 1;
    
    public Transform Transform => this.transform;
    public float InteractionDistance => 2.5f;
  
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float remainingHoney = this.honeyCapacity - this.honey;
        // Process pollen into honey
        if (remainingHoney > 0 && this.pollen > 0)
        {
            float remainingPollen = remainingHoney / this.processingRatio;
            float processdPollen = this.processingSpeed * Time.deltaTime;
            if(processdPollen > this.pollen)
                processdPollen = this.pollen;
            if(processdPollen > remainingPollen)
                processdPollen = remainingPollen;
            
            float generatedHoney = processdPollen * processingRatio;
            this.pollen -= processdPollen;
            this.honey += generatedHoney;
        }
    }

    /// <summary>
    /// Increase the Hive's pollen quantity from the given amount.
    /// The hives is bound to the maximum quantity
    /// </summary>
    /// <param name="givenQuantity">Pollen given to the hive</param>
    /// <returns>The amounbt of pollen actually taken</returns>
    public float GetPollenFromBee(float givenQuantity)
    {
        float remainingPollenQuantity = this.pollenCapacity - this.pollen;
        if (remainingPollenQuantity < givenQuantity)
        {
            this.pollen = this.pollenCapacity;
            return remainingPollenQuantity;   
        }
        else
        {
            this.pollen += givenQuantity;
            return givenQuantity;
        }
    }

    public int GetHoneyProcessingPercent()
    {
        if (this.honey >= this.honeyCapacity)
            return 100;
        if (this.honey == 0f)
            return 0;
        return Mathf.RoundToInt((this.honey - Mathf.Floor(this.honey)) * 100);
    }

    public int LootHoney(int quantity = 1)
    {
        if (this.honey < 1)
            return 0;
        if (this.honey < quantity)
            quantity = Mathf.FloorToInt(this.honey);
        
        this.honey -= quantity;
        return quantity;
    }

    public void Upgrade()
    {
        if (this.level == 1)
        {
            this.level = 2;
            this.honeyCapacity = 5f;
            this.processingSpeed = 0.2f;
        }
    }
}
