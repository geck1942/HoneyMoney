using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class FarmController : BaseController<FarmController>
{
    public List<Beehive> hives = new List<Beehive>();
    public List<Bee> bees = new List<Bee>();
    public List<Flower> flowers = new List<Flower>();
    public List<BuildElement> builds = new List<BuildElement>();
    public List<Transform> chickenTargets = new List<Transform>();
    
    public Merchant merchant;
    public List<NavMeshSurface> surfaces = new List<NavMeshSurface>();
    public List<Item> allItems = new List<Item>();

    public float HoneyPrice = 1f;
    
    private Random _random =  new Random();
    private Coroutine _refreshNavigationCoroutine;
    
    public delegate void BuildEventHandler(BuildElement element);
    public event BuildEventHandler OnBuild;
    
    void Start()
    {
        this.flowers = GameObject.FindGameObjectsWithTag("Flower")
            .Select(obj => obj.GetComponent<Flower>())
            .ToList();

        this.hives = GameObject.FindGameObjectsWithTag("Beehive")
            .Select(obj => obj.GetComponent<Beehive>())
            .ToList();
        
        this._random.InitState();
        PlayerController.Instance.OnInteractableReached += PlayerReachedInteractable;
    }

    private void PlayerReachedInteractable(IInteractable target)
    {
        if(target is BuildElement buildElement 
           && buildElement.autoBuild 
           && buildElement.PlayerCanBuild())
            this.Build(buildElement);
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
        
        foreach (var build in this.builds)
            if(build != null && build.gameObject.activeSelf && !build.isDone)
                yield return build;
     
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

    public void Build(BuildElement target)
    {
        target.Build();
        for(int i= 0; i < target.neededItems.Count; i++)
        {
            Item item = target.neededItems[i];
            int quantity = 1;
            if(target.neededItemsQuantities.Count > i)
                quantity = target.neededItemsQuantities[i];
            PlayerController.Instance.Loot(item.itemName, -quantity);
        }
        this.OnBuild?.Invoke(target);
    }

    public Transform GetRandomChickenTarget()
    {
        return this.chickenTargets.OrderBy(t => this._random.NextDouble()).FirstOrDefault();
    }
}
