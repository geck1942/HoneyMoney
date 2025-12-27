using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeehivePanel : MonoBehaviour
{

    public Beehive target;
    
    public TMPro.TextMeshProUGUI honeyCountText;
    public TMPro.TextMeshProUGUI levelText;
    public TMPro.TextMeshProUGUI honeyProgressText;
    public TMPro.TextMeshProUGUI beeProgressText;
    public TMPro.TextMeshProUGUI beeCountText;
    public List<Slider> honeySliders;
    public Button upgradeButton;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.target != null)
        {
            
            this.honeyCountText.text = Mathf.FloorToInt(this.target.honey) + "/" + Mathf.FloorToInt(this.target.honeyCapacity);
            this.honeyProgressText.text = this.target.GetHoneyProcessingPercent().ToString("0") + "%";

            this.levelText.text = this.target.level.ToString();

            for (int h = 1; h <= this.honeySliders.Count; h++)
            {
                Slider slider = this.honeySliders[h - 1];
                slider.gameObject.SetActive(this.target.honeyCapacity >= h);
                
                if(this.target.honey >= h)
                    slider.value = 1f;
                else if (this.target.honey < h - 1)
                    slider.value = 0;
                else
                    slider.value = this.target.honey - h + 1;
            }

            // only level 2 available
            upgradeButton.gameObject.SetActive(target.level == 1);
            upgradeButton.interactable = PlayerController.Instance.Inventory.Money >= 30;

            // Fake information
            if (this.target.level > 1)
                beeCountText.text = "4/4";
            else
                beeCountText.text = "3/3";
            
            
        }
    }

    public void ClickUpgrade()
    {
        PlayerController.Instance.Loot("money", -30);
        this.target.Upgrade();
    }
}
