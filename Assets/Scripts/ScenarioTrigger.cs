using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ScenarioTrigger : MonoBehaviour
{
    public List<Bee> SendBeesToHives = new List<Bee>();
    public ScenarioState GotoState = ScenarioState.None;
    public List<GameObject> activateItems = new List<GameObject>();
    public List<GameObject> deactivateItems = new List<GameObject>();
    public bool isBuildTrigger = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (Bee bee in SendBeesToHives)
                bee.beeAction = BeeAction.GoToHive;
        }

        foreach (GameObject item in activateItems)
            item.SetActive(true);
        
        foreach (GameObject item in deactivateItems)
            item.SetActive(false);
        
        if(this.GotoState !=  ScenarioState.None)
            ScenarioController.Instance.GoToStep(this.GotoState);

        if (this.isBuildTrigger && ScenarioController.Instance.currentStep is ScenarioStep_Build buildStep)
        {
            buildStep.StartBuild();
        }
    }
}
