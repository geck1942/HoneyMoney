using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : BaseController<CameraController>
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        // PlayCinematic();
    }
    
    [SerializeField] CinemachineVirtualCamera followCam;
    [SerializeField] CinemachineVirtualCamera cinematicCam;

    [SerializeField] float holdTime = 3f;

    public void PlayCinematic()
    {
        cinematicCam.Priority = followCam.Priority + 10;
        StartCoroutine(CinematicRoutine());
    }

    IEnumerator CinematicRoutine()
    {
        // Blend to cinematic

        yield return new WaitForSeconds(holdTime);

        // Blend back to follow
        cinematicCam.Priority = followCam.Priority - 10;
    }
}
