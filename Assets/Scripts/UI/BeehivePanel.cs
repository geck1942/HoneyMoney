using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeehivePanel : MonoBehaviour
{

    public Beehive target;
    
    public TMPro.TextMeshProUGUI honeyCountText;
    public TMPro.TextMeshProUGUI levelText;
    public TMPro.TextMeshProUGUI honeyProgressText;
    public TMPro.TextMeshProUGUI beeProgressText;
    public TMPro.TextMeshProUGUI beeCountText;
    
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

            this.levelText.text = "1";
        }
    }
}
