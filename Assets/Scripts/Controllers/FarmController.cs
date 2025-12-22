using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class FarmController : BaseController<FarmController>
{
    // Start is called before the first frame update
    public List<Beehive> hives = new List<Beehive>();
    public List<Bee> bees = new List<Bee>();
    public List<Flower> flowers = new List<Flower>();
    
    private Random _random =  new Random();
    void Start()
    {
        base.Start();
        this.flowers = GameObject.FindGameObjectsWithTag("Flower")
            .Select(obj => obj.GetComponent<Flower>())
            .ToList();

        this._random.InitState();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Flower FindFlower(float quantity)
    {
        if (flowers.Count == 0)
            return null;
        return  flowers[_random.NextInt(flowers.Count)];
    }
}
