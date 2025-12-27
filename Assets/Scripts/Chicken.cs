using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = Unity.Mathematics.Random;

public class Chicken : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;
    public Animator animator;

    public Coroutine GoingCoroutine;
    private Random _random;
    
    public void Start()
    {
        if(this.target != null && this.agent != null)
            this.agent.SetDestination(this.target.position);
        this.animator.Play("Walk");
        this._random.InitState((uint)this.GetHashCode());

    }

    public void Update()
    {
        if(this.GoingCoroutine ==  null)
            this.StartCycle();
    }

    public void StartCycle()
    {
        if(this.GoingCoroutine != null)
            StopCoroutine(this.GoingCoroutine);

        this.agent.speed = _random.NextFloat(1, 3);
        this.target = FarmController.Instance.FindGrain();
        this.GoingCoroutine = StartCoroutine(this.Cycle());
    }

    public IEnumerator Cycle()
    {
        int rnd = _random.NextInt(1, 12);
        this.agent.SetDestination(this.target.position);
        switch (rnd % 3)
        {
            case 0:
                this.animator.Play("Walk");
                break;
            case 1:
                this.animator.Play("Run");
                break;
            case 2:
                this.animator.Play("Fly");
                break;
        }
        while (this.agent.remainingDistance == 0f || this.agent.remainingDistance > this.agent.stoppingDistance)
        {
            yield return null;
        }
        switch (rnd % 2)
        {
            case 0:
                this.animator.Play("Eat");
                break;
            case 1:
                this.animator.Play("Idle_A");
                break;
        }
        yield return new WaitForSeconds(_random.NextInt(1, 30));
        this.GoingCoroutine = null;

    }
}