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


    /// <summary>
    /// Change the camera according to the UI, to always center on the Player
    /// </summary>
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

    /// <summary>
    /// Sets a specific camera for a gien duration
    /// </summary>
    public void SwitchToSpecialCamera(CinemachineVirtualCamera camera, float duration)
    {
        camera.Priority = 2;
        StartCoroutine(this.holdCamera(camera, duration));
    }
    private IEnumerator holdCamera(CinemachineVirtualCamera camera, float duration)
    {
        yield return new WaitForSeconds(duration);
        this.CancelSpecialCamera(camera);
    }

    /// <summary>
    /// Go back to normal Camera
    /// </summary>
    public void CancelSpecialCamera(CinemachineVirtualCamera camera)
    {
        camera.Priority = 0;
    }
}
