using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;

public class Bear : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;
    
    public BearAction action = BearAction.Idle;
    public Transform forestWaypoint;
    public GameObject target;
    
    public float load = 0f;
    public float capacity = 1f;
    public float harvestSpeed = 0.1f;
    public float discardSpeed = 0.1f;

    private Coroutine autoStealCoroutine = null;
    
    // Start is called before the first frame update
    void Start()
    {
        ScenarioController.Instance.OnScenarioNextStep += InstanceOnOnScenarioNextStep;
    }

    private void InstanceOnOnScenarioNextStep(ScenarioState state, ScenarioStep step)
    {
        if (state == ScenarioState.ExplainGame)
        {
            this.autoStealCoroutine = StartCoroutine(TriggerStealEvery30Sec());
        }
    }

    private IEnumerator TriggerStealEvery30Sec()
    {
        while (ScenarioController.Instance.state == ScenarioState.ExplainGame)
        {
            yield return new  WaitForSeconds(60f);
            this.action = BearAction.GoToTarget;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (this.action)
        {
            case BearAction.Hide:
                this.GoToForest();
                break;
            case BearAction.GoToTarget:
                this.FindTarget();
                this.GoToTarget();
                break;
            case BearAction.StealHoney:
                this.StealHoney();
                break;
        }
    }

    public void FindTarget()
    {
        Strawberry strawberry = FarmController.Instance.strawberries
            .FirstOrDefault(s => s != null && s.state == StrawberryStatus.Grown);
        if(strawberry != null)
            this.target = strawberry.gameObject;
        else
            this.target = FarmController.Instance.hives.OrderBy(x => x.honey).Last().gameObject;
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
            if (target.gameObject.TryGetComponent(out Beehive hive))
            {
                hive.honey = 0;
            }
            else if (target.gameObject.TryGetComponent(out Strawberry strawberry))
            {
                strawberry.SetStatus(StrawberryStatus.Eaten);
            }
            
            StartCoroutine(this.FinishedEating());
        }
    }

    public void GoToTarget()
    {
        this.animator.Play("Walk");
        this.agent.speed = 4f;
        this.agent.destination = this.target.transform.position;
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
        this.transform.LookAt(this.target.transform.position);
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
        
        // Different animation if honey or strawberry
        if(this.target.gameObject.GetComponent<Beehive>() != null)
        {
            ScenarioController.Instance.RaiseScenarioAnimation("bear", "roll");
            this.animator.Play("Roll");
        }
        else
        {
            ScenarioController.Instance.RaiseScenarioAnimation("bear", "run");
            this.animator.Play("Run");
        }
        this.agent.speed = 10f;
    }

}
