using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    public bool isFrontSide = true;
    public Gate gate;
    public void OnTriggerEnter(Collider other)
    {
        if(this.isFrontSide)
            gate.OpenFromFront();
        else
            gate.OpenFromBack();
    }
}
