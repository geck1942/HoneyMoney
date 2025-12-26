using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strawberry : MonoBehaviour, IInteractable
{
    public List<GameObject> AllProps;
    public List<GameObject> GrowingProps;
    public List<GameObject> GrownProps;
    public List<GameObject> EatenProps;
    
    public StrawberryStatus state =  StrawberryStatus.Empty;
    
    void Start()
    {
        this.SetStatus(this.state);
    }

    // Update is called once per frame
    public void SetStatus(StrawberryStatus status)
    {
        this.state = status;
        HideAllProps();
        switch (this.state)
        {
            case  StrawberryStatus.Growing:
                foreach(GameObject prop in GrowingProps)
                    prop.gameObject.SetActive(true);
                break;
            case  StrawberryStatus.Grown:
                foreach(GameObject prop in GrownProps)
                    prop.gameObject.SetActive(true);
                break;
            case  StrawberryStatus.Eaten:
                foreach(GameObject prop in EatenProps)
                    prop.gameObject.SetActive(true);
                break;
        }
    }

    public void HideAllProps()
    {
        foreach(GameObject prop in AllProps)
            prop.gameObject.SetActive(false);
    }

    public Transform Transform => this.transform;
    public float InteractionDistance => 1f;
}

public enum StrawberryStatus
{
    Empty = 0,
    Growing = 1,
    Grown = 2,
    Eaten = 3,
}