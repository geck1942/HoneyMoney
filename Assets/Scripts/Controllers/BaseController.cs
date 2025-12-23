using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class BaseController : MonoBehaviour
{

}

public class BaseController<TController> : BaseController 
    where TController : BaseController
{
    [CanBeNull] public static TController Instance;
    
    public void Awake()
    {
        if (Instance == null && this is TController)
            Instance = this as TController;
    }
}
