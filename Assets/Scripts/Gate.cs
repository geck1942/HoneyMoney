using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public bool IsActive = true;
    public Transform PivotFront;
    public Transform PivotBack;
    public float DefaultAngle = 0f;
    public float FrontOpenTargetAngle = -83f;
    public float BackOpenTargetAngle = 110f;
    public float Duration = 1f;

    private Coroutine _frontCoroutineOpen = null;
    private Coroutine _frontCoroutineClose = null;
    private Coroutine _backCoroutineOpen = null;
    private Coroutine _backCoroutineClose = null;

    private bool IsOpen;
    
    public void OpenFromFront()
    {
        if (IsActive && !IsOpen &&  _frontCoroutineOpen == null && _backCoroutineOpen == null)
            _frontCoroutineOpen = StartCoroutine(RotateFrontPivot(this.PivotFront.rotation.eulerAngles.y, this.FrontOpenTargetAngle, true));
    }
    public void OpenFromBack()
    {
        if (IsActive && !IsOpen &&  _frontCoroutineOpen == null && _backCoroutineOpen == null)
            _backCoroutineOpen = StartCoroutine(RotateFrontPivot(this.PivotFront.rotation.eulerAngles.y, this.BackOpenTargetAngle, true));
    }

    private IEnumerator RotateFrontPivot(float fromAngle, float toAngle, bool open)
    {
        float t = 0;
        while (t <= 1)
        {
            t+= Time.deltaTime /  Duration;
            if (t >= 1) 
                t = 1;
            this.PivotFront.rotation = Quaternion.Euler(new Vector3(0, Mathf.Lerp(fromAngle, toAngle, t), 0));
            yield return null;
        }
        this.IsOpen = open;
    }
}
