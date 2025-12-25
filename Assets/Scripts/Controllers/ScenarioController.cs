using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScenarioController : BaseController<ScenarioController>
{
    public ScenarioState state = ScenarioState.None;
    public Bear Bear;
    
    public List<ScenarioStep> steps = new List<ScenarioStep>();
    public ScenarioStep currentStep = null;
    
    public delegate void ScenarioEvent(ScenarioState state, ScenarioStep step);
    public event ScenarioEvent OnScenarioNextStep;


    public void Start()
    {
        PlayerController.Instance.OnLoot += ActUponLoot;
        ScenarioController.Instance.OnScenarioNextStep += ActUponScenarioStep;
        
        this.GoToStep(ScenarioState.GotoHives1);
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

    public void ActUponScenarioStep(ScenarioState newScenarioState, ScenarioStep step)
    {
        // CustomCode
    }

}

public enum ScenarioState
{
    None = 0,
    GotoHives1 = 1,
    CollectHoney1 = 2,
    GotoMerchant1 = 3,  
    SellHoney1 = 4,
    GoToHives2 = 5,
    BearIncoming = 6,
    BuyPlanks = 7,
    FixFence = 8,
    BearIncoming2 = 9,
    BuyStawberries = 10
    
    
}
