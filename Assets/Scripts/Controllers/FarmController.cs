using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class FarmController : BaseController<FarmController>
{
    // Start is called before the first frame update
    public List<Beehive> hives = new List<Beehive>();
    public List<Bee> bees = new List<Bee>();
    public List<Flower> flowers = new List<Flower>();
    public Merchant merchant;
    public List<NavMeshSurface> surfaces = new List<NavMeshSurface>();

    public float HoneyPrice = 1f;
    
    private Random _random =  new Random();
    private Coroutine _refreshNavigationCoroutine;
    void Start()
    {
        this.flowers = GameObject.FindGameObjectsWithTag("Flower")
            .Select(obj => obj.GetComponent<Flower>())
            .ToList();

        this.hives = GameObject.FindGameObjectsWithTag("Beehive")
            .Select(obj => obj.GetComponent<Beehive>())
            .ToList();
        
        this._random.InitState();
    }


    public Flower FindFlower(float quantity)
    {
        if (flowers.Count == 0)
            return null;
        return  flowers[_random.NextInt(flowers.Count)];
    }

    public IEnumerable<IInteractable> GetAllInteractables()
    {
        foreach (var hive in this.hives)
            if(hive != null)
                yield return hive;
        
        if(merchant != null)
            yield return this.merchant;
    }

    public void RefreshNavigation()
    {
        if(this._refreshNavigationCoroutine != null)
            StopCoroutine(this._refreshNavigationCoroutine);
        _refreshNavigationCoroutine = StartCoroutine(RefreshNavigationRoutine());
    }

    private IEnumerator RefreshNavigationRoutine()
    {
        yield return null;
    }
    
}
