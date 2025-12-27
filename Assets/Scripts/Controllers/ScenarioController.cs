using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScenarioController : BaseController<ScenarioController>
{
    public ScenarioState state = ScenarioState.Intro;
    public Bear Bear;
    
    public List<ScenarioStep> steps = new List<ScenarioStep>();
    public ScenarioStep currentStep = null;
    
    public delegate void ScenarioEvent(ScenarioState state, ScenarioStep step);
    public event ScenarioEvent OnScenarioNextStep;

    public delegate void AnimationEvent(string actor, string status);
    public event AnimationEvent OnScenarioAnimation;


    public void Start()
    {
        PlayerController.Instance.OnLoot += ActUponLoot;
        
        this.GoToStep(ScenarioState.Intro);
    }

    public void GoToNextStep()
    {
        this.GoToStep(this.state + 1);

    }
    public void GoToStep(ScenarioState newScenarioState)
    {

        if (this.state != newScenarioState)
        {
            if (this.currentStep != null)
                this.currentStep.End();

            this.state = newScenarioState;
            ScenarioStep newStep = this.steps.FirstOrDefault(step => step.State == newScenarioState);
            if (newStep != null)
            {
                this.currentStep = newStep;
                newStep.Begin();
            }
            else
            {
                this.currentStep = null;
            }
            
            this.OnScenarioNextStep?.Invoke(this.state, this.currentStep);
        }
        
    }
     
    /// <summary>
    ///  TODO: Remove this and use new ScenarioStep properties
    /// </summary>
    public void ActUponLoot(string resource, float quantity, IInteractable target)
    {
        switch (this.state)
        {
            case ScenarioState.CollectHoney1:
                if(resource == "honey" && quantity > 0)
                    this.GoToStep(ScenarioState.GotoMerchant1);
                break;
            case ScenarioState.SellHoney1:
                if(resource == "money" && quantity > 0)
                    this.GoToStep(ScenarioState.GoToHives2);
                break;
        }
    }

    public void RaiseScenarioAnimation(string actor, string status)
    {
        this.OnScenarioAnimation?.Invoke(actor, status);
    }
}

public enum ScenarioState
{
    None = -1,
    Intro = 0,
    GotoHives1 = 1,
    CollectHoney1 = 2,
    GotoMerchant1 = 3,  
    SellHoney1 = 4,
    GoToHives2 = 5,
    BearIncoming = 6,
    BuyPlanks = 7,
    FixFence = 8,
    CollectHoney2 = 9,
    BearIncoming2 = 10,
    BuyStawberries = 11,
    PlantStrawberries = 12,
    ExplainStrawberries = 13,
    ExplainGame = 14,
    Done = 15
    
}
