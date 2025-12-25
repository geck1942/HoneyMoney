    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class ScenarioStep_Bear : ScenarioStep
    {
        public override void Begin()
        {
            base.Begin();
            
            Debug.Log("BEAR");
            ScenarioController.Instance.Bear.action = BearAction.GoToHive;
        }

    }
