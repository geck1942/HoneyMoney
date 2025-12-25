using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DefaultNamespace;

public class CameraController : BaseController<CameraController>
{
    
    [SerializeField] CinemachineVirtualCamera regularCamera;
    [SerializeField] CinemachineVirtualCamera onePanelCamera;
    [SerializeField] CinemachineVirtualCamera menuCamera;


    new void Start()
    {
        UIController.Instance.OnUIStateChanged += SwitchCamera;
    }


    private void SwitchCamera(UIState state)
    {
        if ((state & UIState.MenuPanel) > 0)
        {
            this.menuCamera.Priority = 1;
            this.onePanelCamera.Priority = 0;
            this.regularCamera.Priority = 0;
        }
        else if ((state & UIState.ProximityPanel) > 0)
        {
            this.menuCamera.Priority = 0;
            this.onePanelCamera.Priority = 1;
            this.regularCamera.Priority = 0;
        }
        else
        {
            this.menuCamera.Priority = 0;
            this.onePanelCamera.Priority = 0;
            this.regularCamera.Priority = 1;
        }
    }

    public void SwitchToSpecialCamera(CinemachineVirtualCamera camera, float duration)
    {
        camera.Priority = 2;
        StartCoroutine(this.holdCamera(camera, duration));
    }
    private IEnumerator holdCamera(CinemachineVirtualCamera camera, float duration)
    {
        yield return new WaitForSeconds(duration);
        camera.Priority = 0;
    }
}
