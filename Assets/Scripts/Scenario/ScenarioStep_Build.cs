    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class ScenarioStep_Build : ScenarioStep
    {
        public List<GameObject> removeProps = new List<GameObject>();
        public List<GameObject> addProps = new List<GameObject>();
        public float buildDuration = 0f; 
        
        public bool triggerNavMeshUpdate = false;


        public void StartBuild()
        {
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
            yield return new WaitForSeconds(buildDuration);
            PlayerController.Instance.player.animator.Play("Idle");
            foreach (var prop in addProps)
            {
                prop.SetActive(true);
                yield return null;
            }
            ScenarioController.Instance.GoToNextStep();
        }

    }
