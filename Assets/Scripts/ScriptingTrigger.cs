using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ScriptingTrigger : MonoBehaviour
{
    public List<Bee> SendBeesToHives = new List<Bee>();

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (Bee bee in SendBeesToHives)
                bee.action = BeeAction.GoToHive;
        }
    }
}
