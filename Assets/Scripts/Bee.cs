using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;

public class Bee : MonoBehaviour
{
    public NavMeshAgent agent;
    
    public BeeAction beeAction = BeeAction.Idle;
    public Flower target;
    public Beehive hive;
    
    public float load = 0f;
    public float capacity = 1f;
    public float harvestSpeed = 0.1f;
    public float discardSpeed = 0.1f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.FixCycle();
        switch (this.beeAction)
        {
            case BeeAction.GoToFlower:
                this.GoToFlower();
                break;
            case BeeAction.HarvestPollen:
                this.HarvestPollen();
                break;
            case  BeeAction.GoToHive:
                this.GoToHive();
                break;
            case BeeAction.ProcessHoney:
                this.DiscardPollen();
                break;
        }
    }
    private void FixCycle()
    {
        switch (this.beeAction)
        {
            case BeeAction.Idle:
                this.StartCycle();
                break;
            case BeeAction.GoToFlower:
            case BeeAction.HarvestPollen:
                if (this.target == null)
                    this.beeAction = BeeAction.Idle;
                break;
            case BeeAction.GoToHive:
            case BeeAction.ProcessHoney: 
                if(this.hive == null)
                    this.beeAction = BeeAction.Idle;
                break;
        }
    }

    public bool IsLoaded()
    {
        return this.load >= this.capacity;
    }

    public void StartCycle()
    {
        if (this.IsLoaded())
        {
            this.beeAction = BeeAction.GoToHive;
        }
        else
        {
            float lookingForQuantity = this.capacity - this.load;
            Flower targetFlower = FarmController.Instance?.FindFlower(lookingForQuantity);
            if (targetFlower != null)
            {
                this.target = targetFlower;
                this.beeAction = BeeAction.GoToFlower;
            }
        }
    }

    public void GoToFlower()
    {
        this.agent.destination = this.target.transform.position;
        Vector3 direction = this.agent.destination - this.transform.position;
        if (direction.magnitude < 0.5f)
        {
            this.beeAction = BeeAction.HarvestPollen;
        }
    }

    public void HarvestPollen()
    {
        float askingQuantity = this.harvestSpeed * Time.deltaTime;
        if(askingQuantity + this.load > this.capacity)
            askingQuantity = this.capacity - this.load;
        this.load += this.target.GivePollenToBee(askingQuantity);
        if (this.load >= this.capacity)
        {
            this.load = capacity;
            this.beeAction = BeeAction.Idle;
        }
    }

    public void GoToHive()
    {
        this.agent.destination = this.hive.transform.position;
        Vector3 direction = this.agent.destination - this.transform.position;
        if (direction.magnitude < 0.1f)
        {
            this.beeAction = BeeAction.ProcessHoney;
        }

    }

    public void DiscardPollen()
    {
        float discardPollen = this.discardSpeed * Time.deltaTime;
        this.load -= this.hive.GetPollenFromBee(discardPollen);
        if (this.load <= 0)
        {
            this.load = 0;
            this.beeAction = BeeAction.Idle;
        }
    }
}
