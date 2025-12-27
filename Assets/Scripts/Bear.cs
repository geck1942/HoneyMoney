using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;

public class Bear : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;
    
    public BearAction action = BearAction.Idle;
    public Transform forestWaypoint;
    public Transform hiveWaypoint;
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
        switch (this.action)
        {
            case BearAction.Hide:
                this.GoToForest();
                break;
            case BearAction.GoToHive:
                this.GoToHive();
                break;
            case BearAction.StealHoney:
                this.StealHoney();
                break;
        }
    }

    public bool IsLoaded()
    {
        return this.load >= this.capacity;
    }


    public void GoToForest()
    {
        this.agent.destination = this.forestWaypoint.transform.position;
        Vector3 direction = this.agent.destination - this.transform.position;
        if (direction.magnitude < 0.5f)
        {
        this.animator.Play("Sit");
            this.action = BearAction.Idle;
        }
    }

    public void StealHoney()
    {
        float askingQuantity = this.harvestSpeed * Time.deltaTime;
        if(askingQuantity + this.load > this.capacity)
            askingQuantity = this.capacity - this.load;
        this.load += askingQuantity;
        if (this.load >= 1f)
        {
            this.load = 0;
            this.hive.honey = 0;
            StartCoroutine(this.FinishedEating());
        }
    }

    public void GoToHive()
    {
        this.animator.Play("Walk");
        this.agent.speed = 4f;
        this.agent.destination = this.hiveWaypoint.position;
        StartCoroutine(this.WalkingToHive());
    }

    private IEnumerator WalkingToHive()
    {
        this.action = BearAction.Walking;
        Vector3 direction = this.agent.destination - this.transform.position;
        ScenarioController.Instance.RaiseScenarioAnimation("bear", "walk");
        while (this.agent.remainingDistance == 0f || this.agent.remainingDistance > this.agent.stoppingDistance)
        {
            Debug.Log(this.agent.remainingDistance);
            yield return null;
        }
        ScenarioController.Instance.RaiseScenarioAnimation("bear", "arrived");
            
        this.action = BearAction.PrepareToSteal;
        this.agent.destination = this.transform.position;
        this.transform.LookAt(this.hive.transform.position);
        yield return null;
        ScenarioController.Instance.RaiseScenarioAnimation("bear", "excited");
        this.animator.Play("Eyes_Excited");
        this.animator.Play("Idle_A");
        yield return new WaitForSeconds(0.3f);
        this.animator.Play("Idle_B");
        yield return new WaitForSeconds(0.45f);
        this.animator.Play("Idle_C");
        yield return new WaitForSeconds(0.45f);
        this.animator.Play("Attack");
        this.animator.Play("Eyes_Happy");
        this.action = BearAction.StealHoney;
        ScenarioController.Instance.RaiseScenarioAnimation("bear", "eat");
    }

    private IEnumerator FinishedEating()
    {
        this.action = BearAction.CelebrateSteal;
        this.animator.Play("Idle_A");
        yield return new WaitForSeconds(0.25f);
        ScenarioController.Instance.RaiseScenarioAnimation("bear", "lookout");
        this.animator.Play("Eyes_LookOut");
        yield return new WaitForSeconds(0.5f);
        this.animator.Play("Eyes_Blink");
        this.action = BearAction.Hide;
        this.animator.Play("Run");
        ScenarioController.Instance.RaiseScenarioAnimation("bear", "leave");
        yield return new WaitForSeconds(0.8f);
        ScenarioController.Instance.RaiseScenarioAnimation("bear", "roll");
        this.animator.Play("Roll");
        this.agent.speed = 10f;
    }

}
