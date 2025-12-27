    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class ScenarioStep_Build : ScenarioStep, IInteractable
    {
        public List<GameObject> removeProps = new List<GameObject>();
        public List<GameObject> addProps = new List<GameObject>();
        public float buildDuration = 0f;
        public float itemNeededQuantity = 0;
        public string itemNeeded = "";
        public bool triggerNavMeshUpdate = false;


        public void StartBuildIfInvetory()
        {
            if (!string.IsNullOrEmpty(this.itemNeeded))
            {
                if(!PlayerController.Instance.Inventory.Has(itemNeeded, itemNeededQuantity))
                    return;
                
                PlayerController.Instance.Loot(itemNeeded, -itemNeededQuantity);
            }
            StartCoroutine(BuildRoutine());
        }

        private IEnumerator BuildRoutine()
        {
            
            foreach (var prop in removeProps)
            {
                prop.SetActive(false);
                yield return null;
            }
            PlayerController.Instance.player.animator.Play("Build");
            ScenarioController.Instance.RaiseScenarioAnimation("player", "build");
            yield return new WaitForSeconds(buildDuration);
            PlayerController.Instance.player.animator.Play("Idle");
            foreach (var prop in addProps)
            {
                prop.SetActive(true);
                yield return null;
            }
            ScenarioController.Instance.GoToNextStep();
        }

        public Transform Transform => this.transform;
        public float InteractionDistance => 2f;
    }
