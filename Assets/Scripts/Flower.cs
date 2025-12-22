using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public float pollen = 0f;
    public float pollenCapacity = 10f;
    public float pollenGenerationSpeed = 0.1f;
    public bool startsFull = true;
    
    void Start()
    {
        if(this.startsFull)
            this.pollen = this.pollenCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        this.pollen +=  this.pollenGenerationSpeed * Time.deltaTime;
        if (this.pollen >= this.pollenCapacity)
            this.pollen = this.pollenCapacity;
    }

    /// <summary>
    /// Gives pollen to a bee from an asked quantity.
    /// Bound to the pollen available
    /// </summary>
    /// <param name="askedAmount">Quantity of pollen asked</param>
    /// <returns>The actual amount of pollen given</returns>
    public float GivePollenToBee(float askedAmount)
    {
        if (this.pollen < askedAmount)
        {
            this.pollen = 0;
            return this.pollen;
        }
        else
        {
            this.pollen -= askedAmount;
            return askedAmount;
        }
    }
}
