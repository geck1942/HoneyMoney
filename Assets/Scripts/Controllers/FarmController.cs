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
    public Mesh BeehiveLevel1Mesh;
    public Mesh BeehiveLevel2Mesh;
    
    private Random _random =  new Random();
    private Coroutine _refreshNavigationCoroutine;
    
    public delegate void BuildEventHandler(BuildElement element);
    public event BuildEventHandler OnBuild;
    
    void Start()
    {
        this.flowers = GameObject.FindGameObjectsWithTag("Flower")
            .Select(obj => obj.GetComponent<Flower>())
            .ToList();

        this.RefreshHives();
        
        this._random.InitState();
        PlayerController.Instance.OnInteractableReached += PlayerReachedInteractable;
    }

    /// <summary>
    /// Auto build if player is closeby a Build Element
    /// </summary>
    private void PlayerReachedInteractable(IInteractable target)
    {
        if(target is BuildElement buildElement 
           && buildElement.autoBuild 
           && buildElement.PlayerCanBuild())
            this.Build(buildElement);
    }


    /// <summary>
    /// Yields all Interactable objects. Needed for PlayerController to find proximity
    /// </summary>
    public IEnumerable<IInteractable> GetAllInteractables()
    {
        foreach (var hive in this.hives)
            if(hive != null)
                yield return hive;
        
        if(merchant != null)
            yield return this.merchant;
        
        foreach (var build in this.builds)
            if(build != null && build.gameObject.activeSelf && build.IsAvailable())
                yield return build;
     
    }

    /// <summary>
    /// Player Builds a Build Element.
    /// Consumes needed items and deal with ame objects.
    /// </summary>
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
        this.RefreshHives();
        this.OnBuild?.Invoke(target);
    }

    public void RefreshHives()
    {
        this.hives = GameObject.FindGameObjectsWithTag("Beehive")
            .Select(obj => obj.GetComponent<Beehive>())
            .ToList();
    }
    
    /// <summary>
    /// Bees need to find a random flower to get honey from
    /// </summary>
    /// <param name="quantity"></param>
    /// <returns>A random flower from the list</returns>
    public Flower FindFlower(float quantity)
    {
        if (flowers.Count == 0)
            return null;
        return  flowers[_random.NextInt(flowers.Count)];
    }
    /// <summary>
    /// Chickens need to find a place to eat.
    /// Gets a random point from the list.
    /// </summary>
    /// <returns>Any target from the list</returns>
    public Transform FindGrain()
    {
        return this.chickenTargets.OrderBy(t => this._random.NextDouble()).FirstOrDefault();
    }
}
