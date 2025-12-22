using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : BaseController<UIController>
{
    public BeehivePanel beehivePanel;
    public Transform proximityCircle;

    public GameObject lootPrefab;
    
    void Start()
    {
        base.Start();

        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.OnBeehiveReached += OnBeehiveReached;
            PlayerController.Instance.OnBeehiveLeft += OnBeehiveLeft;
            PlayerController.Instance.OnLoot += ShowLoot;
        }
    }
    
    private void OnBeehiveLeft(Beehive beehive)
    {
        this.beehivePanel.target = null;
        this.beehivePanel.gameObject.SetActive(false);
        this.proximityCircle.gameObject.SetActive(false);
    }

    private void OnBeehiveReached(Beehive beehive)
    {
        this.beehivePanel.target = beehive;
        this.beehivePanel.gameObject.SetActive(true);

        this.proximityCircle.position = beehive.transform.position + (Vector3.up * 0.01f);
        this.proximityCircle.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowLoot(Beehive target, int quantity)
    {
        var loot = GameObject.Instantiate(lootPrefab, target.transform.position, Quaternion.identity);
        StartCoroutine(ShowLootCoroutine(loot));
    }

    IEnumerator ShowLootCoroutine(GameObject lootPrefab)
    {
        yield return new WaitForSeconds(1f);
        GameObject.Destroy(lootPrefab);
    }
}
