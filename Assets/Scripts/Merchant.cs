using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour, IInteractable
{

    public Inventory Inventory = new Inventory();

    public Transform Transform => this.transform;
    public float InteractionDistance => 3.5f;

    void Start()
    {
        
    }
    

}
